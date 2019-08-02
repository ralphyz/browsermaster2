using System;
using System.Windows.Media.Imaging;

namespace BrowserMasterUI {
	public struct Browser {
		public BitmapImage Image;
		public string BrowserName;

		public Browser(BitmapImage image, string browserName) {
			Image = image;
			BrowserName = browserName;
		}
	}
}
