using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NdiCommander;


internal class NdiSource : INdiSource
{
    public NdiSource(string receiverName, string sourceName)
    {
        if (string.IsNullOrEmpty(receiverName))
            throw new ArgumentNullException(nameof(receiverName));
        if (string.IsNullOrEmpty(sourceName))
            throw new ArgumentNullException(nameof(sourceName));

        _receiverName = receiverName;
        Name = sourceName;
        Status = NdiSourceStatusEnum.Connecting;
        Connect();
    }


    public event EventHandler StatusChanged;


    public string Name { get; }
    public bool SupportsPtz { get; internal set; }
    public string WebControlUrl { get; internal set; }


    public NdiSourceStatusEnum Status 
    {
        get => _status;
        internal set
        {
            if (value != _status)
            {
                _status = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }


    public void SelectPtzPreset(int presetNumber)
    {
        lock (_syncRoot)
        {
            if ((Status == NdiSourceStatusEnum.Connected) && SupportsPtz)
                NdiLib.recv_ptz_recall_preset(_recvInstancePtr, presetNumber - 1, 1);
        }
    }


    internal async Task DisconnectAsync()
    {
        Task task;
        lock (_syncRoot)
        {
            task = _task;
            _tokenSource?.Cancel();
        }
        if(task!=null)
            await task.ConfigureAwait(false);
        lock (_syncRoot)
        {
            Status = NdiSourceStatusEnum.Disconnected;
            if (_recvInstancePtr != IntPtr.Zero)
            {
                NdiLib.recv_destroy(_recvInstancePtr);
                _recvInstancePtr = IntPtr.Zero;
            }
        }
    }


    bool INdiSource.SupportsPtz 
    { 
        get => SupportsPtz; 
        set => SupportsPtz = value; 
    }


    string INdiSource.WebControlUrl 
    { 
        get => WebControlUrl; 
        set => WebControlUrl = value; 
    }


    NdiSourceStatusEnum INdiSource.Status 
    { 
        get => Status; 
        set => Status = value; 
    }


    async Task INdiSource.DisconnectAsync()
    {
        await DisconnectAsync();
    }


    private void Connect()
    {
        lock (_syncRoot)
        {
            NdiLib.source_t source_t = new NdiLib.source_t()
            {
                p_ndi_name = NdiLib.StringToUtf8(Name)
            };
            NdiLib.recv_create_v3_t recvDescription = new NdiLib.recv_create_v3_t()
            {
                source_to_connect_to = source_t,
                color_format = NdiLib.recv_color_format_e.fastest,
                bandwidth = NdiLib.recv_bandwidth_e.metadata_only,
                allow_video_fields = false,
                p_ndi_recv_name = NdiLib.StringToUtf8(_receiverName)
            };
            _recvInstancePtr = NdiLib.recv_create_v3(ref recvDescription);
            Marshal.FreeHGlobal(source_t.p_ndi_name);
            Marshal.FreeHGlobal(recvDescription.p_ndi_recv_name);
            if (_recvInstancePtr != IntPtr.Zero)
            {
                _tokenSource = new CancellationTokenSource();
                _task = Task.Run(() => Watch(_tokenSource.Token));
            }
        }
    }


    private void Watch(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            NdiLib.video_frame_v2_t videoFrame = new NdiLib.video_frame_v2_t();
            NdiLib.audio_frame_v2_t audioFrame = new NdiLib.audio_frame_v2_t();
            NdiLib.metadata_frame_t metadataFrame = new NdiLib.metadata_frame_t();
            NdiLib.frame_type_e frameTypeReceived = NdiLib.recv_capture_v2(_recvInstancePtr, ref videoFrame, ref audioFrame, ref metadataFrame, 1000);
            switch (frameTypeReceived)
            {
                case NdiLib.frame_type_e.status_change:
                    SupportsPtz = NdiLib.recv_ptz_is_supported(_recvInstancePtr);
                    IntPtr webUrlPtr = NdiLib.recv_get_web_control(_recvInstancePtr);
                    if (webUrlPtr == IntPtr.Zero)
                    {
                        WebControlUrl = null;
                    }
                    else
                    {
                        WebControlUrl = NdiLib.Utf8ToString(webUrlPtr);
                        NdiLib.recv_free_string(_recvInstancePtr, webUrlPtr);
                    }
                    Status = NdiSourceStatusEnum.Connected;
                    break;
            }
        }
    }


    private string _receiverName;

    private object _syncRoot = new();
    private Task _task;
    private CancellationTokenSource _tokenSource;
    private IntPtr _recvInstancePtr = IntPtr.Zero;
    private NdiSourceStatusEnum _status;
}


/// <summary>
///     An NDI source (e.g. camera) which has been found on the network
/// </summary>
public interface INdiSource
{
    /// <summary>
    ///     Raised when the <see cref="Status"/> property has changed
    /// </summary>
    public event EventHandler StatusChanged;


    /// <summary>
    ///     The description of the NDI source
    /// </summary>
    public string Name { get; }


    /// <summary>
    ///     Gets whether the NDI source supports remote Pan/Tilt/Zoom control via NDI commands
    /// </summary>
    public bool SupportsPtz { get; internal set; }


    /// <summary>
    ///     The URL to the NDI device's web control page (if applicable).  If no web control page is available, 
    ///     this property will be null.
    /// </summary>
    public string WebControlUrl { get; internal set; }


    /// <summary>
    ///     The current status of our connection to the NDI source
    /// </summary>
    public NdiSourceStatusEnum Status { get; internal set; }


    /// <summary>
    ///     Commands the NDI source (usually a video camera) to transition to the PTZ settings that have been 
    ///     stored under the given preset number
    /// </summary>
    /// <param name="presetNumber">
    ///     A number between 1 and 100, inclusive
    /// </param>
    /// <remarks>
    ///     This will only work when the <see cref="Status"/> is <see cref="NdiSourceStatusEnum.Connected"/> and 
    ///     the <see cref="SupportsPtz"/> property is true.
    /// </remarks>
    public void SelectPtzPreset(int presetNumber);


    /// <summary>
    ///     Disconnects the application (i.e. NDI receiver) from the NDI source
    /// </summary>
    internal Task DisconnectAsync();
}