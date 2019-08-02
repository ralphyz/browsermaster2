using System.Xml.Serialization;
using System;
using System.IO;


namespace BrowserMaster
{
    [XmlRoot("Browser")]
    public class BrowserConfig
    {
        private string _argumentFormat;
        #region CTOR

        public BrowserConfig() { }
        public BrowserConfig(string name, bool enabled, string fullPath, string processName, string iconPath)
        {
            Name = name;
            Enabled = enabled;
            FullPath = fullPath;
            ProcessName = processName;
            IconPath = iconPath;
        }

        #endregion

        #region Factory

        public static BrowserConfig FireFox
        {
            get
            {
                return new BrowserConfig()
                {
                    FullPath = @"C:\Program Files\Mozilla Firefox\firefox.exe",
                    Enabled = true,
                    Name = "FireFox",
                    ProcessName = "firefox",
                    IconPath = Path.Combine(IconDirectory, @"Firefox.png"),
                    ArgumentsFormat = @"-requestPending -osint -url ""{0}"""
                };
            }
        }

        public static BrowserConfig FireFox64
        {
            get
            {
                return new BrowserConfig()
                {
                    FullPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe",
                    Enabled = true,
                    Name = "FireFox",
                    ProcessName = "firefox",
                    IconPath = Path.Combine(IconDirectory, @"Firefox.png"),
                    ArgumentsFormat = @"-requestPending -osint -url ""{0}"""
                };
            }
        }

        public static BrowserConfig Chrome
        {
            get
            {
                return new BrowserConfig()
                {
                    FullPath = @"c:\Program Files\Google\Chrome\Application\chrome.exe",
                    Enabled = true,
                    Name = "Chrome",
                    ProcessName = "chrome",
                    IconPath = Path.Combine(IconDirectory, "Chrome.png"),
                };
            }
        }

        public static BrowserConfig Chrome64
        {
            get
            {
                return new BrowserConfig()
                {
                    FullPath = @"c:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                    Enabled = true,
                    Name = "Chrome",
                    ProcessName = "chrome",
                    IconPath = Path.Combine(IconDirectory, "Chrome.png"),
                };
            }
        }
        public static BrowserConfig IE
        {
            get
            {
                return new BrowserConfig()
                {
                    FullPath = @"C:\Program Files\Internet Explorer\iexplore.exe",
                    Comment = "Currently, pages will not open as a new tab in an existing instance of the browser.  This is a limitation of the browser, not BrowserMaster.",
                    Enabled = true,
                    Name = "IE",
                    ProcessName = "iexplore",
                    IconPath = Path.Combine(IconDirectory,"IE.png"),
                };
            }
        }

        public static BrowserConfig Edge
        {
          
            get
            {
                return new BrowserConfig()
                {                    
                    FullPath = Environment.GetEnvironmentVariable("windir") +  @"\System32\cmd.exe",
                    Comment = "Edge requires http[s]:// in front of a URL",
                    Enabled = true,
                    Name = "Edge",
                    ProcessName = "microsoft-edge",
                    IconPath = Path.Combine(IconDirectory, "Edge.png"),
                    ArgumentsFormat = @"/c start shell:AppsFolder\Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge"
                };
            }
        }
        public static BrowserConfig Opera
        {
            get
            {
                return new BrowserConfig()
                {
                    FullPath = @"C:\Program Files\Opera\opera.exe",
                    Enabled = true,
                    Name = "Opera",
                    ProcessName = "opera",
                    IconPath = Path.Combine(IconDirectory, "Opera.png"),
                };
            }
        }

        public static BrowserConfig Opera64
        {
            get
            {
                return new BrowserConfig()
                {
                    FullPath = @"C:\Program Files (x86)\Opera\opera.exe",
                    Enabled = true,
                    Name = "Opera",
                    ProcessName = "opera",
                    IconPath = Path.Combine(IconDirectory, "Opera.png"),
                };
            }
        }
        public static BrowserConfig Safari
        {
            get
            {
                return new BrowserConfig()
                {
                    FullPath = @"c:\Program Files\Safari\Safari.exe",
                    Comment = "Currently, pages will not open as a new tab in an existing instance of the browser.  This is a limitation of the browser, not BrowserMaster.",
                    Enabled = true,
                    Name = "Safari",
                    ProcessName = "safari",
                    IconPath = Path.Combine(IconDirectory, "Safari.png"),
                };
            }
        }

        public static BrowserConfig Safari64
        {
            get
            {
                return new BrowserConfig()
                {
                    FullPath = @"c:\Program Files (x86)\Safari\Safari.exe",
                    Comment = "Currently, pages will not open as a new tab in an existing instance of the browser.  This is a limitation of the browser, not BrowserMaster.",
                    Enabled = true,
                    Name = "Safari",
                    ProcessName = "safari",
                    IconPath = Path.Combine(IconDirectory, "Safari.png"),
                };
            }
        }
        #endregion

        #region Serialized Properties
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public bool Enabled { get; set; }
        public string FullPath { get; set; }
        public string ArgumentsFormat
        {
            get { return _argumentFormat; }
            set
            {
                if (value == null) value = string.Empty;
                if (!value.Contains("{0}")) value += " {0}";
                _argumentFormat = value;
            }
        }
        public string ProcessName { get; set; }
        public string Comment { get; set; }
        public string IconPath { get; set; }
        #endregion

        #region Not Serialized Properties
        [XmlIgnore]
        public bool Installed {
            get {
                if (this.Name == "Edge")
                    return true;

                return File.Exists(this.FullPath);
            }
        }
        public bool EnabledAndInstalled { get { return Enabled && Installed; } }
        private static string IconDirectory { get { return Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "icons"); } }
        #endregion

        public string GetArgument(string url)
        {
            return string.IsNullOrEmpty(ArgumentsFormat) ?
                url :
                string.Format(ArgumentsFormat, url);
        }

        public override string ToString()
        {
            return string.Format("{0} ({2})-> {1} ", Name, FullPath, Enabled ? "Enabled" : "Disabled");
        }
    }
}