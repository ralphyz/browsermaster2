using System;
using Microsoft.Win32;

namespace BrowserMaster
{
    public class Registrar
    {
        #region Methods

        public static void Register(string protocol, string handlerPath, string iconPath)
        {
            //var root = Registry.ClassesRoot;
            //var root = Registry.CurrentUser.OpenSubKey(@"Software\Classes\", true);
            CreateClass();
            //Register(root, protocol, handlerPath, iconPath);
        }

        public static void CreateClass()
        {
            string masterName = "BrowserMaster";
            
            string baseFolder = @"SOFTWARE\Classes\" + masterName;
            string iconFolder = "DefaultIcon";
            string shellFolder = "shell";
            string openFolder =  "open";
            string command = "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" %1";
            string defaultIcon = System.Reflection.Assembly.GetExecutingAssembly().Location + ",0";
            
            RegistryKey root = Registry.LocalMachine;
            RegistryKey baseName = root.CreateSubKey(baseFolder);

            RegistryKey icon = baseName.CreateSubKey(iconFolder);
            icon.SetValue( "",defaultIcon);            
            
            RegistryKey shell = baseName.CreateSubKey(shellFolder);

            RegistryKey openkey = shell.CreateSubKey(openFolder);
            RegistryKey commandKey = openkey.CreateSubKey("command");
            commandKey.SetValue("", command);
            
            //clean up
            commandKey.Close();
            icon.Close();
            openkey.Close();
            shell.Close();
            baseName.Close();
            root.Close();


            //register the program with the OS (Win 7)
            //HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice
            root = Registry.CurrentUser;
            RegistryKey userChoiceHTTP = root.CreateSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice");
            userChoiceHTTP.SetValue("Progid", masterName);
            RegistryKey userChoiceHTTPS = root.CreateSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice");
            userChoiceHTTPS.SetValue("Progid", masterName);
            RegistryKey userChoiceFTP = root.CreateSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\ftp\UserChoice");
            userChoiceFTP.SetValue("Progid", masterName);
            
            //bkd - new

            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string appFullName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            RegistryKey classes_root = Registry.ClassesRoot;
            RegistryKey classesRootBrowserMaster = classes_root.CreateSubKey("BrowserMasterURL");
            classesRootBrowserMaster.SetValue("", "BrowserMaster URL Handler");
            classesRootBrowserMaster.SetValue("AppUserModelId", "BrowserMaster");
            
            RegistryKey classesRootBrowserMasterApplication = classesRootBrowserMaster.CreateSubKey("Application");
            classesRootBrowserMasterApplication.SetValue("ApplicationCompany", masterName);
            classesRootBrowserMasterApplication.SetValue("ApplicationDescription", masterName);
            classesRootBrowserMasterApplication.SetValue("ApplicationIcon", string.Format("{0},0", appFullName));
            classesRootBrowserMasterApplication.SetValue("ApplicationName", masterName);
            classesRootBrowserMasterApplication.SetValue("AppUserModelId", masterName);

            RegistryKey classesRootBrowserMasterDefaultIcon= classesRootBrowserMaster.CreateSubKey("DefaultIcon");
            classesRootBrowserMasterDefaultIcon.SetValue("", string.Format("{0},0", appFullName));

            RegistryKey classesRootBrowserMasterShell = classesRootBrowserMaster.CreateSubKey(@"shell\open\command");            
            classesRootBrowserMasterShell.SetValue("", string.Format("\"{0}\" \"%1\"", appFullName));

            classesRootBrowserMasterShell.Close();
            classesRootBrowserMasterDefaultIcon.Close();
            classesRootBrowserMasterApplication.Close();
            classesRootBrowserMaster.Close();
            classes_root.Close();

            RegistryKey localMachineRoot = Registry.LocalMachine;
            RegistryKey localMachineBrowserMaster = localMachineRoot.CreateSubKey(string.Format(@"SOFTWARE\Clients\StartMenuInternet\{0}", masterName));
            localMachineBrowserMaster.SetValue("", masterName);

            RegistryKey localMachineBrowserMasterCapabilities = localMachineBrowserMaster.CreateSubKey("Capabilities");
            localMachineBrowserMasterCapabilities.SetValue("ApplicationDescription", masterName );
            localMachineBrowserMasterCapabilities.SetValue("ApplicationIcon", string.Format(@"{0}, 0", appFullName));
            localMachineBrowserMasterCapabilities.SetValue("ApplicationName", masterName );
            localMachineBrowserMasterCapabilities.SetValue("AppUserModelId", masterName );

            RegistryKey localMachineStartMenu = localMachineBrowserMasterCapabilities.CreateSubKey("Startmenu");
            localMachineStartMenu.SetValue("StartMenuInternet", masterName );

            RegistryKey localMachineURLAssociations = localMachineBrowserMasterCapabilities.CreateSubKey("URLAssociations");
            localMachineURLAssociations.SetValue("http", string.Format("{0}URL", masterName));
            localMachineURLAssociations.SetValue("https", string.Format("{0}URL", masterName));

            RegistryKey localMachineDefaultIcon = localMachineBrowserMasterCapabilities.CreateSubKey("DefaultIcon");
            localMachineDefaultIcon.SetValue("", string.Format("{0},0", appFullName));

            RegistryKey localCommand = localMachineRoot.CreateSubKey(string.Format(@"SOFTWARE\Clients\StartMenuInternet\{0}\shell\open\command", masterName));
            localCommand.SetValue("", appFullName);
            
            RegistryKey localRegistered = localMachineRoot.CreateSubKey(@"SOFTWARE\RegisteredApplications");
            localRegistered.SetValue(masterName , string.Format(@"SOFTWARE\Clients\StartMenuInternet\{0}\Capabilities", masterName));


            RegistryKey bmWow64 = localMachineRoot.CreateSubKey(string.Format(@"SOFTWARE\Wow6432Node\Clients\StartMenuInternet\{0}", masterName));
            bmWow64.SetValue("", masterName );

            RegistryKey bmCapabilities = bmWow64.CreateSubKey("Capabilities");

            bmCapabilities.SetValue("ApplicationDescription", masterName );
            bmCapabilities.SetValue("ApplicationIcon", string.Format(@"{0}, 0", appFullName));
            bmCapabilities.SetValue("ApplicationName", masterName );
            bmCapabilities.SetValue("AppUserModelId", masterName );

            RegistryKey bmStart = bmCapabilities.CreateSubKey("Startmenu");
            bmStart.SetValue("StartMenuInternet", masterName );

            RegistryKey bmURL = bmCapabilities.CreateSubKey("URLAssociations");
            bmURL.SetValue("http", string.Format("{0}URL", masterName));
            bmURL.SetValue("https", string.Format("{0}URL", masterName));

            RegistryKey bmDefaultIcon = bmCapabilities.CreateSubKey("DefaultIcon");
            bmDefaultIcon.SetValue("", string.Format(@"{0},0", appFullName));

            RegistryKey bmCommand = bmWow64.CreateSubKey(string.Format(@"SOFTWARE\Clients\StartMenuInternet\{0}\shell\open\command", masterName));
            bmCommand.SetValue("", appFullName);

            RegistryKey bmRegistered = localMachineRoot.CreateSubKey(@"SOFTWARE\Wow6432Node\RegisteredApplications");
            bmRegistered.SetValue(masterName , string.Format(@"SOFTWARE\Clients\StartMenuInternet\{0}\Capabilities", masterName));

            bmRegistered.Close();
            bmCommand.Close();
            bmDefaultIcon.Close();
            bmURL.Close();
            bmStart.Close();
            bmCapabilities.Close();
            bmWow64.Close();
            localRegistered.Close();
            localCommand.Close();
            localMachineDefaultIcon.Close();
            localMachineURLAssociations.Close();
            localMachineStartMenu.Close();
            localMachineBrowserMasterCapabilities.Close();
            localMachineBrowserMaster.Close();
            localMachineRoot.Close();


            //clean up
            
            root.Close();
            userChoiceFTP.Close();
            userChoiceHTTP.Close();
            userChoiceHTTPS.Close();
            
        }

        public static void Register(RegistryKey root, string protocol, string handlerPath, string iconPath)
        {
            protocol = protocol.ToLower();

            // If this key already exists delete it so we start fresh
            try
            {
                root.DeleteSubKeyTree(protocol);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }  
         
            RegistryKey protocolKey = root.CreateSubKey(protocol);
            protocolKey.SetValue(@"URL Protocol", string.Empty, RegistryValueKind.String);
            protocolKey.SetValue(string.Empty, @"URL:HyperText Transfer Protocol");

            RegistryKey cmdKey = OpenOrCreateSubKey(protocolKey, @"shell\open\command");
            cmdKey.SetValue(string.Empty, handlerPath);

            if (string.IsNullOrEmpty(iconPath)) return;
            RegistryKey iconKey = OpenOrCreateSubKey(protocolKey, @"DefaultIcon");
            iconKey.SetValue(string.Empty, iconPath);
        }

        /// <summary>
        /// Registers the application at the path specified as the default browser.
        /// </summary>
        /// <param name="path">The path to the browser with the arguments specified</param>
        public static void RegisterBrowser(string path, string iconPath)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (!path.Contains(@"%1")) throw new ArgumentException("Path must specify the full argument list and contain %1 to represent passed in arguments");
            Register("http", path, iconPath);
            Register("https", path, iconPath);
        }

        #endregion

        #region Helpers
        private static RegistryKey OpenOrCreateSubKey(RegistryKey baseKey, string subKeyPath)
        {
            return baseKey.OpenSubKey(subKeyPath, true) ?? baseKey.CreateSubKey(subKeyPath);
        }
        #endregion
    }
}
