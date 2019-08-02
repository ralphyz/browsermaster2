using System.Xml.Serialization;
using System.IO;
using System;

namespace BrowserMaster {
	public class Configuration {
		public Settings Settings {get ; set;}
		public BrowserConfigs Browsers { get; set; }

		public static Configuration Load(string path) {
			XmlSerializer xs = new XmlSerializer(typeof(Configuration));
			using (TextReader tr = new StreamReader(path)) {
				Configuration config = xs.Deserialize(tr) as Configuration;
				return config;
			}
		}

		public void Save(string path) {
			XmlSerializer xs = new XmlSerializer(typeof(Configuration));
			using (TextWriter tw = new StreamWriter(path)) {
				xs.Serialize(tw, this);
			}
		}
	}
}
