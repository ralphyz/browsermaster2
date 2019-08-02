using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace BrowserMaster
{
    [XmlRoot("Browsers")]
    public class BrowserConfigs : List<BrowserConfig>
    {
        #region CTOR
        public BrowserConfigs() : base() { }
        public BrowserConfigs(int capacity) : base(capacity) { }
        public BrowserConfigs(IEnumerable<BrowserConfig> collection) : base(collection) { }
        #endregion

        public BrowserConfigs EnabledAndInstalled
        {
            get
            {
                return new BrowserConfigs(from config in this
                                          where config.EnabledAndInstalled
                                          select config);
            }
        }
    }
}
