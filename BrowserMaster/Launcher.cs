using System.Diagnostics;
using System;
using System.Runtime.InteropServices;



namespace BrowserMaster
{    
    public static class Launcher
    {
        [DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr handle);

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);

        const int SW_RESTORE = 9;

        #region Public Methods
        public static void Launch(string url, BrowserConfigs browsers)
        {
            if (browsers == null)  return;
            foreach (BrowserConfig bc in browsers)
            {
                if (bc == null || !bc.Enabled) continue;
                if (LaunchIfRunning(url, bc)) return;
            }

            foreach (BrowserConfig bc in browsers)
            {
                if (bc == null || !bc.Enabled) continue;
                if (LaunchUrl(url, bc))return;
            }

            throw new Exception("Page could not be opened");
        }
        private static bool LaunchUrl(string url, BrowserConfig bc)
        {
            if (!bc.Installed) { return false; }

            //edge requires http:// - let's do them all
            if(!(url.StartsWith("http://") || url.StartsWith("https://")))
            {
                url = "http://" + url;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(bc.FullPath, bc.GetArgument(url));
            Process p = Process.Start(startInfo);

            IntPtr handle;

            handle = p.MainWindowHandle;

            if(IsIconic(handle))
            {
                ShowWindow(handle, SW_RESTORE);
            }

            SetForegroundWindow(handle);

            return p != null;
        }

        #endregion

        #region Private Methods
        private static bool LaunchIfRunning(string url, BrowserConfig bc)
        {
            Process[] processes = Process.GetProcessesByName(bc.ProcessName);
            if (processes == null || processes.Length == 0) return false;

            return LaunchUrl(url, bc);
        }

        #endregion
    }
}
