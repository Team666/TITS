using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace faceTITS.Keyboard
{
    /// <summary>
    /// Listens keyboard globally.
    /// 
    /// <remarks>Uses WH_KEYBOARD_LL.</remarks>
    /// </summary>
    public class KeyboardListener : IDisposable
    {
        /// <summary>
        /// Creates global keyboard listener.
        /// </summary>
        public KeyboardListener()
        {
            // We have to store the HookCallback, so that it is not garbage collected runtime
            hookedLowLevelKeyboardProc = (NativeMethods.LowLevelKeyboardProc)LowLevelKeyboardProc;

            // Set the hook
            hookId = NativeMethods.SetHook(hookedLowLevelKeyboardProc);

            // Assign the asynchronous callback event
            hookedKeyboardCallbackAsync = new KeyboardCallbackAsync(KeyboardListener_KeyboardCallbackAsync);
        }

        /// <summary>
        /// Destroys global keyboard listener.
        /// </summary>
        ~KeyboardListener()
        {
            Dispose();
        }

        /// <summary>
        /// Fired when any of the keys is pressed down.
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        /// Fired when any of the keys is released.
        /// </summary>
        public event KeyEventHandler KeyUp;

        /// <summary>
        /// Hook ID
        /// </summary>
        private IntPtr hookId = IntPtr.Zero;

        /// <summary>
        /// Asynchronous callback hook.
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        private delegate void KeyboardCallbackAsync(NativeMethods.KeyEvent keyEvent, int vkCode);

        /// <summary>
        /// Actual callback hook.
        /// 
        /// <remarks>Calls asynchronously the asyncCallback.</remarks>
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
                if (wParam.ToUInt32() == (int)NativeMethods.KeyEvent.WM_KEYDOWN ||
                    wParam.ToUInt32() == (int)NativeMethods.KeyEvent.WM_KEYUP ||
                    wParam.ToUInt32() == (int)NativeMethods.KeyEvent.WM_SYSKEYDOWN ||
                    wParam.ToUInt32() == (int)NativeMethods.KeyEvent.WM_SYSKEYUP)
                    hookedKeyboardCallbackAsync.BeginInvoke((NativeMethods.KeyEvent)wParam.ToUInt32(), Marshal.ReadInt32(lParam), null, null);

            return NativeMethods.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// Event to be invoked asynchronously (BeginInvoke) each time key is pressed.
        /// </summary>
        private KeyboardCallbackAsync hookedKeyboardCallbackAsync;

        /// <summary>
        /// Contains the hooked callback in runtime.
        /// </summary>
        private NativeMethods.LowLevelKeyboardProc hookedLowLevelKeyboardProc;

        /// <summary>
        /// HookCallbackAsync procedure that calls accordingly the KeyDown or KeyUp events.
        /// </summary>
        /// <param name="keyEvent">Keyboard event</param>
        /// <param name="vkCode">VKCode</param>
        void KeyboardListener_KeyboardCallbackAsync(NativeMethods.KeyEvent keyEvent, int vkCode)
        {
            switch (keyEvent)
            {
                // KeyDown events
                case NativeMethods.KeyEvent.WM_KEYDOWN:
                    if (KeyDown != null)
                        KeyDown(this, new KeyEventArgs(vkCode, false));
                    break;
                case NativeMethods.KeyEvent.WM_SYSKEYDOWN:
                    if (KeyDown != null)
                        KeyDown(this, new KeyEventArgs(vkCode, true));
                    break;

                // KeyUp events
                case NativeMethods.KeyEvent.WM_KEYUP:
                    if (KeyUp != null)
                        KeyUp(this, new KeyEventArgs(vkCode, false));
                    break;
                case NativeMethods.KeyEvent.WM_SYSKEYUP:
                    if (KeyUp != null)
                        KeyUp(this, new KeyEventArgs(vkCode, true));
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Disposes the hook.
        /// <remarks>This call is required as it calls the UnhookWindowsHookEx.</remarks>
        /// </summary>
        public void Dispose()
        {
            NativeMethods.UnhookWindowsHookEx(hookId);
        }
    }
    /// <summary>
    /// Raw KeyEvent arguments.
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        /// <summary>
        /// Virtual key code.
        /// </summary>
        public int VKCode;

        /// <summary>
        /// The pressed key.
        /// </summary>
        public Key Key;

        /// <summary>
        /// Determines whether the pressed key is a system key.
        /// </summary>
        public bool IsSysKey;

        /// <summary>
        /// Create raw keyevent arguments.
        /// </summary>
        /// <param name="VKCode"></param>
        /// <param name="isSysKey"></param>
        public KeyEventArgs(int VKCode, bool isSysKey)
        {
            this.VKCode = VKCode;
            this.IsSysKey = isSysKey;
            this.Key = System.Windows.Input.KeyInterop.KeyFromVirtualKey(VKCode);
        }
    }

    /// <summary>
    /// Raw keyevent handler.
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="args">raw keyevent arguments</param>
    public delegate void KeyEventHandler(object sender, KeyEventArgs args);

    /// <summary>
    /// Winapi Key interception helper class.
    /// </summary>
    internal static class NativeMethods
    {
        public delegate IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam);
        public static int WH_KEYBOARD_LL = 13;

        public enum KeyEvent : int
        {
            WM_KEYDOWN = 256,
            WM_KEYUP = 257,
            WM_SYSKEYUP = 261,
            WM_SYSKEYDOWN = 260
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, UIntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}