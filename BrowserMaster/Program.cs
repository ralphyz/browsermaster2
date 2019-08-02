using System;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BrowserMaster
{
    class Program
    {
        #region CONST
        private const string CONFIG_FILE = @"BrowserMaster.xml";
        #region private const string USAGE = "..."
        private const string USAGE = "USAGE:  BrowserMaster <URL> | [OPTIONS]\r\nOpen the specified URL in a configured browser.\r\n\r\n  -d\tCreate the default configuration file\r\n  -e\tOpen the configuration file in your default editor\r\n  -g\tShow the UI regardless of the configuration\r\n  -r\tRegister as the default HTTP handler\r\n  -h\tShow usage";
        #endregion
        #endregion

        private static readonly string ConfigPath;

        static Program()
        {
            ConfigPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //if (!Directory.Exists(ConfigPath)) Directory.CreateDirectory(ConfigPath);
            ConfigPath = Path.Combine(ConfigPath, CONFIG_FILE);
        }


        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (!CheckArguments(args))
                {
                    DisplayUsage();
                    return;
                }
                if (!File.Exists(ConfigPath)) SaveDefaultConfig(ConfigPath);
                Run(args[0]);
            }
            catch (Exception ex)
            {
                DisplayError("There was an error running the BrowswerMaster:", ex);
            }
        }

        #region Error Messages
        private static void DisplayError(string msg) { DisplayError(msg, null); }

        private static void DisplayError(string msg, Exception ex)
        {
            StringBuilder error = new StringBuilder();
            error.Append(msg);
            if (ex != null)
            {
                error.AppendLine();
                error.Append(ex.Message);
#if DEBUG
                error.AppendLine();
                error.Append(ex.StackTrace);
#endif
            }
            MessageBox.Show(error.ToString(), "BrowserMaster Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void DisplayUsage()
        {
            MessageBox.Show(USAGE, "BrowserMaster Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        private static bool CheckArguments(string[] args)
        {
            if (args == null || args.Length != 1 || string.IsNullOrEmpty(args[0]))
            {
                return false;
            }

            if (args[0] == "-r") return true;
            if (args[0] == "-d") return true;
            return true;
        }

        private static void Run(string url)
        {
            switch (url.ToLower())
            {
                case "-d":
                    SaveDefaultConfig(ConfigPath);
                    return;
                case "-e":
                    EditConfig(ConfigPath);
                    return;
                case "-r":
                    string cmdFormat = "\"{0}\" \"%1\"";
                    string icoFormat = "\"{0}\",0";
                    string cmd = string.Format(cmdFormat, Assembly.GetExecutingAssembly().Location);
                    string ico = string.Format(icoFormat, Assembly.GetExecutingAssembly().Location);

                    Registrar.RegisterBrowser(cmd, ico);
                    return;
                case "-h":
                case "--help":
                    DisplayUsage();
                    return;
                default:
                    Configuration config = LoadConfig();
                    if (config.Browsers == null || config.Browsers.Count < 1)
                    {
                        DisplayError("No browsers present in configuration:  " + ConfigPath);
                        return;
                    }

                    LaunchBrowser(url, config);
                    return;
            }


        }

        #region Actions
        private static void EditConfig(string ConfigPath)
        {
            Process.Start(ConfigPath);
        }

        private static void LaunchBrowser(string url, Configuration config)
        {
            BrowserConfigs browsers = config.Browsers;
            if (config.Settings.ShowGui)
                browsers = ShowGui(config);

            Launcher.Launch(url, browsers);
        }

        /// <summary>
        /// Shows the Gui and lets the user decide which browser they want to use
        /// </summary>
        /// <param name="config"></param>
        /// <returns>A list of browsers to try and use, or null when the user cancelled the opening of the URL</returns>
        private static BrowserConfigs ShowGui(Configuration config)
        {
            BrowserPicker bp = new BrowserPicker(config);
            bool? result = bp.ShowDialog();
            return bp.BrowsersToUse;
        }

        private static Configuration LoadConfig()
        {
            return Configuration.Load(ConfigPath);
  
        }

        private static void SaveDefaultConfig(string path)
        {
            Configuration c = new Configuration();
            c.Settings = Settings.Default;

            Boolean bFirefox = File.Exists(BrowserConfig.FireFox.FullPath);
            Boolean bFirefox64 = false;

            if (!bFirefox)
                bFirefox64 = File.Exists(BrowserConfig.FireFox64.FullPath);

            Boolean bChrome = File.Exists(BrowserConfig.Chrome.FullPath);
            Boolean bChrome64 = false;

            if (!bChrome)
              bChrome64 = File.Exists(BrowserConfig.Chrome64.FullPath);

            Boolean bOpera = File.Exists(BrowserConfig.Opera.FullPath);
            Boolean bOpera64 = false;

            if (!bOpera)
                bOpera64 = File.Exists(BrowserConfig.Opera64.FullPath);

            Boolean bSafari = File.Exists(BrowserConfig.Safari.FullPath);
            Boolean bSafari64 = false;

            if (!bSafari)
                bSafari64 = File.Exists(BrowserConfig.Safari64.FullPath);

            Boolean bIE = File.Exists(BrowserConfig.IE.FullPath);

            BrowserConfigs bc = new BrowserConfigs();

            if (bFirefox)
                bc.Add(BrowserConfig.FireFox);

            if (bFirefox64)
                bc.Add(BrowserConfig.FireFox64);

            if (bChrome)
                bc.Add(BrowserConfig.Chrome);

            if (bChrome64)
                bc.Add(BrowserConfig.Chrome64);

            if (bIE)
                bc.Add(BrowserConfig.IE);

            if (bOpera)
                bc.Add(BrowserConfig.Opera);

            if (bOpera64)
                bc.Add(BrowserConfig.Opera64);

            if (bSafari)
                bc.Add(BrowserConfig.Safari);

            if (bSafari64)
                bc.Add(BrowserConfig.Safari64);

            bc.Add(BrowserConfig.Edge);

            /*
            c.Browsers = new BrowserConfigs(){
			BrowserConfig.FireFox,
			BrowserConfig.Chrome,
			BrowserConfig.Opera,
			BrowserConfig.Safari,
			BrowserConfig.IE};
            */

            c.Browsers = bc;
            c.Save(path);
        }
        #endregion
    }
}
