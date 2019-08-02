using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BrowserMasterUI {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window {
		private int selectedIndex = 0;
		private int size = 225;

		public Window1() {
			InitializeComponent();
			//TODO: Remove Browsers
			List<Browser> browsers = new List<Browser>();

			browsers.Add(new Browser(GetImage("Image_Chrome"), "Chrome"));
			browsers.Add(new Browser(GetImage("Image_Firefox"), "Firefox"));
			browsers.Add(new Browser(GetImage("Image_IE"), "IE"));
			browsers.Add(new Browser(GetImage("Image_Opera"), "Opera"));
			browsers.Add(new Browser(GetImage("Image_Safari"), "Safari"));
            browsers.Add(new Browser(GetImage("Image_Edge"), "Edge"));

            Height = size * 1.2;
			Width = browsers.Count * (size + ((size * .2) * 2));
			InitializeGrid(browsers, size, new Thickness(size * .2, 0, size * .2, 0));
		}

		/// <summary>
		/// Simple helper for getting an image
		/// </summary>
		private BitmapImage GetImage(string key) {
			return (BitmapImage)App.Current.Resources[key];
		}

		private void InitializeGrid(List<Browser> browsers, double width, Thickness thickness) {
			for (int i = 0; i < browsers.Count; i++) {
				//-- Create the Image Control
				BrowserImage bi = new BrowserImage(browsers[i], width, thickness);

				//-- Attach to the selected event & entered event
				bi.BrowserSelected += new EventHandler(bi_BrowserSelected);
				bi.BrowserEntered += new EventHandler(bi_BrowserEntered);

				//-- Add the BrowserImage to our Stack Panel
				MainPanel.Children.Add(bi);
				
				//-- Set the Selected Bit
				bi.IsSelected = (i == selectedIndex);
				
				//-- Initialize the Browser Image [Run the initialize Animation]
				bi.BeginInitAnimation();
			}
		}

		private void bi_BrowserEntered(object sender, EventArgs e) {
			BrowserImage b = sender as BrowserImage;
			if (b != null) {
				for (int i = 0; i < MainPanel.Children.Count; i++) {
					if ((BrowserImage)MainPanel.Children[i] == b && selectedIndex != i) {
						selectedIndex = i;
						UpdateSelectedBrowser();
					}
				}
			}
			UseBrowser();
		}

		private void bi_BrowserSelected(object sender, EventArgs e) {
			BrowserImage b = sender as BrowserImage;

			if (b != null) {
				for (int i = 0; i < MainPanel.Children.Count; i++) {
					if (MainPanel.Children[i] == b) {
						selectedIndex = i;
						UpdateSelectedBrowser();
						return;
					}
				}
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e) {
			switch (e.Key) {
				case Key.Up:
				case Key.Left:
					MovePrevious();
					break;
				case Key.Down:
				case Key.Right:
					MoveNext();
					break;
				case Key.Escape:
					Close();
					break;
				case Key.Enter:
					UseBrowser();
					break;
				default:
					break;
			}
		}

		private void UseBrowser() {
			MessageBox.Show("Selected Browser: " + ((BrowserImage)MainPanel.Children[selectedIndex]).NameOfBrowser);
		}

		private void MovePrevious() {
			--selectedIndex;
			if (selectedIndex < 0)
				selectedIndex = MainPanel.Children.Count - 1;
			UpdateSelectedBrowser();
		}

		private void MoveNext() {
			++selectedIndex;
			if (selectedIndex == MainPanel.Children.Count)
				selectedIndex = 0;

			UpdateSelectedBrowser();
		}

		private void UpdateSelectedBrowser() {
			for (int i = 0; i < MainPanel.Children.Count; i++) {
				BrowserImage bi = MainPanel.Children[i] as BrowserImage;
				if (i == selectedIndex)
					bi.Select();
				else
					bi.DeSelect();
			}
		}

		private void Window_MouseWheel(object sender, MouseWheelEventArgs e) {
			if (e.Delta > 0)
				MoveNext();
			else
				MovePrevious();
		}
	}
}
