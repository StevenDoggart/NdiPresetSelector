using NdiCommander;
using System;
using System.Linq;
using System.Windows.Forms;

namespace NdiPresetSelector;


public partial class MainForm : Form
{
    public MainForm()
    {
        _settingsRepository = StaticFactory.NewSettingsRepository();
        _globalHotKeyManager = StaticFactory.NewGlobalHotKeyManager();
        _ndiWatcher = StaticFactory.NewNdiWatcher();

        InitializeComponent();
        _globalHotKeyManager.HotKeyPressed += _globalHotKeyManager_HotKeyPressed;
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.NumPad1);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.NumPad2);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.NumPad3);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.NumPad4);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.NumPad5);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.NumPad6);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.NumPad7);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.NumPad8);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.NumPad9);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.D1);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.D2);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.D3);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.D4);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.D5);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.D6);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.D7);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.D8);
        _globalHotKeyManager.RegisterHotKey(ModifierKeyEnum.Control, Keys.D9);
        _ndiWatcher.Start("NDI Preset Selector");
        _ndiWatcher.SourcesChanged += _ndiWatcher_SourcesChanged;
    }


    private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        _globalHotKeyManager.Dispose();
        await _ndiWatcher.StopAsync();
    }


    private void _ndiWatcher_SourcesChanged(object sender, EventArgs e)
    {
        Invoke
        (
            () =>
            {
                foreach (INdiSource source in _ndiWatcher.Sources)
                {
                    source.StatusChanged -= NdiSourceStatusChangedEventHandler;
                    source.StatusChanged += NdiSourceStatusChangedEventHandler;
                }
            }
        );
    }


    private void _globalHotKeyManager_HotKeyPressed(object sender, KeyPressedEventArgs e)
    {
        int? presetNumber = e.Key switch
        {
            Keys.NumPad1 => 1,
            Keys.NumPad2 => 2,
            Keys.NumPad3 => 3,
            Keys.NumPad4 => 4,
            Keys.NumPad5 => 5,
            Keys.NumPad6 => 6,
            Keys.NumPad7 => 7,
            Keys.NumPad8 => 8,
            Keys.NumPad9 => 9,
            Keys.D1 => 1,
            Keys.D2 => 2,
            Keys.D3 => 3,
            Keys.D4 => 4,
            Keys.D5 => 5,
            Keys.D6 => 6,
            Keys.D7 => 7,
            Keys.D8 => 8,
            Keys.D9 => 9,
            _ => null
        };
        if (presetNumber.HasValue)
            _selectedSource?.SelectPtzPreset(presetNumber.Value);
    }


    private void _exitMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }


    private void _aboutMenuItem_Click(object sender, EventArgs e)
    {
        AboutForm aboutForm = new();
        aboutForm.ShowDialog();
    }


    private void NdiSourceMenuItemClickEventHandler(object sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem item)
        {
            if (item.Tag is INdiSource ndiSource)
            {
                _selectedSource = ndiSource;
                item.Checked = true;
                foreach (ToolStripMenuItem otherItem in _contextMenu.Items.OfType<ToolStripMenuItem>().Except(new[] { item }))
                    otherItem.Checked = false;
                SaveSourceSelection();
            }
        }
    }


    private void NdiSourceStatusChangedEventHandler(object sender, EventArgs e)
    {
        RefreshContextMenu();
    }


    private void RefreshContextMenu()
    {
        lock (_refreshContextMenuSyncRoot)
        {
            INdiSource[] previousPtzSources = _contextMenu.Items
                .OfType<ToolStripMenuItem>()
                .Select(i => i.Tag)
                .OfType<INdiSource>()
                .ToArray();
            INdiSource[] newPtzSources = _ndiWatcher.Sources
                .Where(s => s.SupportsPtz)
                .Except(previousPtzSources)
                .ToArray();
            if (newPtzSources.Any())
            {
                string savedSourceName = LoadSourceSelection();
                foreach (INdiSource ptzSource in newPtzSources)
                {
                    ToolStripMenuItem item = new(ptzSource.Name);
                    item.Click += NdiSourceMenuItemClickEventHandler;
                    item.Tag = ptzSource;
                    _contextMenu.Items.Insert(0, item);
                    _noSourcesFoundMenuItem.Visible = false;
                    if (ptzSource.Name == savedSourceName)
                    {
                        item.Checked = true;
                        _selectedSource = ptzSource;
                    }
                }
            }
        }
    }


    private void SaveSourceSelection()
    {
        Settings settings = new()
        {
            SelectedSourceName = _selectedSource?.Name
        };
        _settingsRepository.SaveSettings(settings);
    }


    private string LoadSourceSelection()
    {
        Settings settings = _settingsRepository.LoadSettings();
        return settings.SelectedSourceName;
    }


    private ISettingsRepository _settingsRepository;
    private IGlobalHotKeyManager _globalHotKeyManager;
    private INdiWatcher _ndiWatcher;

    private INdiSource _selectedSource;
    private object _refreshContextMenuSyncRoot = new();
}