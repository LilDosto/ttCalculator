using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MathApp
{
    internal static class Program
    {
        private const string MutexName = @"Global\ttCalculator_SingleInstance_Mutex_v2";
        private static Mutex appMutex;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_RESTORE = 9;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = false;
            try
            {
                appMutex = new Mutex(true, MutexName, out createdNew);
            }
            catch (UnauthorizedAccessException)
            {
                // Fallback to Local mutex if Global is restricted
                appMutex = new Mutex(true, @"Local\ttCalculator_SingleInstance_Mutex_v2", out createdNew);
            }

            if (!createdNew)
            {
                FocusExistingInstance();
                return;
            }

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show("Beklenmeyen bir hata oluştu:\n" + e.Exception.Message, "ttCalculator - Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            try
            {
                ApplicationConfiguration.Initialize();
                using (MainForm mainForm = new MainForm())
                {
                    Application.Run(mainForm);
                }
            }
            finally
            {
                try
                {
                    appMutex?.ReleaseMutex();
                    appMutex?.Dispose();
                }
                catch { }

                // Ensure no background threads or handles keep the process alive
                Environment.Exit(0);
            }
        }

        private static void FocusExistingInstance()
        {
            try
            {
                Process current = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName(current.ProcessName);
                foreach (Process process in processes)
                {
                    if (process.Id != current.Id && process.MainWindowHandle != IntPtr.Zero)
                    {
                        if (IsIconic(process.MainWindowHandle))
                        {
                            ShowWindowAsync(process.MainWindowHandle, SW_RESTORE);
                        }
                        SetForegroundWindow(process.MainWindowHandle);
                        return;
                    }
                }
            }
            catch { }

            MessageBox.Show(
                "ttCalculator zaten çalışıyor!",
                "ttCalculator",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
