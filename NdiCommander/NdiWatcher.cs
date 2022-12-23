using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NdiCommander;


internal class NdiWatcher : INdiWatcher
{
    public NdiWatcher(INdiSourceFactory ndiSourceFactory)
    {
        _ndiSourceFactory = ndiSourceFactory;
    }


    public event EventHandler SourcesChanged;


    public IReadOnlyList<INdiSource> Sources
    {
        get
        {
            lock (_syncRoot)
            {
                return _sources.ToArray();
            }
        }
    }


    public bool Watching
    {
        get
        {
            lock (_syncRoot)
            {
                return _task != null;
            }
        }
    }


    public void Dispose()
    {
        StopAsync().GetAwaiter().GetResult();
    }


    public void Start(string receiverName)
    {
        lock (_syncRoot)
        {
            if (!Watching)
            {
                _receiverName = receiverName;
                _tokenSource = new();
                _task = Task.Run(() => Watch(_tokenSource.Token));
            }
            else if (receiverName != _receiverName)
            {
                throw new InvalidOperationException("Already watching under a different receiver name");
            }
        }
    }


    public async Task StopAsync()
    {
        Task task = null;
        lock (_syncRoot)
        {
            if (Watching)
            {
                _tokenSource.Cancel();
                task = _task;
            }
        }
        await task.ConfigureAwait(false);
        foreach (INdiSource source in _sources)
            await source.DisconnectAsync().ConfigureAwait(false);
        lock (_syncRoot)
        {
            _sources.Clear();
            OnSourcesChanged();
            _tokenSource = null;
            _task = null;
        }
    }


    private void Watch(CancellationToken token)
    {
        NdiLib.find_create_t findDesc = new NdiLib.find_create_t()
        {
            p_groups = IntPtr.Zero,
            show_local_sources = false,
            p_extra_ips = IntPtr.Zero
        };
        IntPtr findInstancePtr = NdiLib.find_create_v2(ref findDesc);
        int SourceSizeInBytes = Marshal.SizeOf(typeof(NdiLib.source_t));
        while (!token.IsCancellationRequested)
        {
            if (NdiLib.find_wait_for_sources(findInstancePtr, 500))
            {
                uint count = 0;
                IntPtr sourcesPtr = NdiLib.find_get_current_sources(findInstancePtr, ref count);
                bool changed = false;
                for (int i = 0; i < count; i++)
                {
                    IntPtr p = IntPtr.Add(sourcesPtr, (i * SourceSizeInBytes));  // source ptr + (index * size of a source)
                    NdiLib.source_t src = (NdiLib.source_t)Marshal.PtrToStructure(p, typeof(NdiLib.source_t));  // marshal it to a managed source and assign to our list
                    string name = NdiLib.Utf8ToString(src.p_ndi_name);  // .NET doesn't handle marshaling UTF-8 strings properly

                    // Add it to the list if not already in the list.
                    // We don't have to remove because NDI applications remember any sources seen during each run.
                    // They might be selected and come back when the connection is restored.
                    lock (_sources)
                    {
                        INdiSource source = _sources.FirstOrDefault(s => s.Name == name);
                        if (source == null)
                        {
                            _sources.Add(_ndiSourceFactory.NewNdiSource(_receiverName, name));
                            changed = true;
                        }
                    }
                }
                if (changed)
                    OnSourcesChanged();
            }
        }
    }


    private void OnSourcesChanged()
    {
        SourcesChanged?.Invoke(this, EventArgs.Empty);
    }


    private string _receiverName;
    private INdiSourceFactory _ndiSourceFactory;

    private object _syncRoot = new();
    private List<INdiSource> _sources = new();
    private Task _task;
    private CancellationTokenSource _tokenSource;
}


/// <summary>
///     A utility which runs in the background watching for NDI sources on the network
/// </summary>
public interface INdiWatcher : IDisposable
{
    /// <summary>
    ///     Raised when the list of NDI sources in the the <see cref="Sources"/> property has changed
    /// </summary>
    public event EventHandler SourcesChanged;


    /// <summary>
    ///     Returns true if the utility is currently running in the background, watching for NDI sources
    /// </summary>
    public bool Watching { get; }


    /// <summary>
    ///     The list of NDI sources which have been found, so far, on the network
    /// </summary>
    public IReadOnlyList<INdiSource> Sources { get; }


    /// <summary>
    ///     Begins the utility running in the background, watching for NDI sources on the network
    /// </summary>
    public void Start(string receiverName);


    /// <summary>
    ///     Stops the utility from watching for NDI sources on the network
    /// </summary>
    public Task StopAsync();
}