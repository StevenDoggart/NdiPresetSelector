/*
    Originally copied from (but modified):
        https://stackoverflow.com/a/27309185/1359668
*/

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NdiPresetSelector;


internal class GlobalHotKeyManager : IGlobalHotKeyManager
{
    public GlobalHotKeyManager()
    {
        _window.KeyPressed += _window_KeyPressed;
    }


    public event EventHandler<KeyPressedEventArgs> HotKeyPressed;


    public void RegisterHotKey(ModifierKeyEnum modifier, Keys key)
    {
        if (!RegisterHotKey(_window.Handle, _currentId++, (uint)modifier, (uint)key))
            throw new InvalidOperationException("Couldn’t register the hot key.");
    }


    public void Dispose()
    {
        for (int i = _currentId; i > 0; i--)
            UnregisterHotKey(_window.Handle, i);
        _window.Dispose();
    }


    private void _window_KeyPressed(object sender, KeyPressedEventArgs args)
    {
        HotKeyPressed?.Invoke(this, args);
    }


    private Window _window = new();
    private int _currentId;


    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);


    private class Window : NativeWindow, IDisposable
    {
        public Window()
        {
            CreateHandle(new CreateParams());
        }


        public event EventHandler<KeyPressedEventArgs> KeyPressed;


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_HOTKEY)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                ModifierKeyEnum modifier = (ModifierKeyEnum)((int)m.LParam & 0xFFFF);
                KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
            }
        }


        public void Dispose()
        {
            DestroyHandle();
        }


        private static int WM_HOTKEY = 0x0312;
    }
}


internal interface IGlobalHotKeyManager : IDisposable
{
    public event EventHandler<KeyPressedEventArgs> HotKeyPressed;
    public void RegisterHotKey(ModifierKeyEnum modifier, Keys key);
}


internal class KeyPressedEventArgs : EventArgs
{
    public KeyPressedEventArgs(ModifierKeyEnum modifier, Keys key)
    {
        Modifier = modifier;
        Key = key;
    }


    public ModifierKeyEnum Modifier { get; }
    public Keys Key { get; }
}


[Flags]
internal enum ModifierKeyEnum : uint
{
    Alt = 1,
    Control = 2,
    Shift = 4,
    Win = 8
}