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
using System.Windows.Media.Animation;

namespace BrowserMasterUI {
	/// <summary>
	/// Interaction logic for BrowserImage.xaml
	/// </summary>
	public partial class BrowserImage : UserControl {
		
		public event EventHandler BrowserSelected;
		public event EventHandler BrowserEntered;

		private double fullSize;
		private const int widthAnimation = 0;
		private const int heightAnimation = 1;

		public string NameOfBrowser { get { return TheBrowser.BrowserName; } }
		public Browser TheBrowser { get; set; }


		public BrowserImage(Browser b, double width, Thickness margin) {
			InitializeComponent();

			//-- Initialize the size of the control
			Height = Width = width;
			Margin = margin;

			//-- Initialize Full Size
			fullSize = width;
			
			//-- Grab the Image
			Icon.Source = b.Image;

			//-- Initialize Animations
			InitializeAnimations();

			//-- Keep a reference of the browser used to initialize this Browser Image
			TheBrowser = b;

			//-- Initialize the dynamic resource myDuration
			Resources["myDuration"] = new Duration(TimeSpan.FromMilliseconds(350));
		}

		private void InitializeAnimations() {
			Storyboard sb = Resources["SelectedSB"] as Storyboard;
			((DoubleAnimation)sb.Children[0]).To = fullSize;
			((DoubleAnimation)sb.Children[1]).To = fullSize;
			sb = Resources["DeSelectedSB"] as Storyboard;
			((DoubleAnimation)sb.Children[widthAnimation]).To = fullSize * .8;
			((DoubleAnimation)sb.Children[heightAnimation]).To = fullSize * .8;
			sb = Resources["InitSelectedSB"] as Storyboard;
			((DoubleAnimation)sb.Children[widthAnimation]).To = fullSize;
			((DoubleAnimation)sb.Children[heightAnimation]).To = fullSize;
			sb = Resources["InitDeSelectedSB"] as Storyboard;
			((DoubleAnimation)sb.Children[widthAnimation]).To = fullSize * .8;
			((DoubleAnimation)sb.Children[heightAnimation]).To = fullSize * .8;
		}

		public bool IsSelected { get; set; }
		
		/// <summary>
		/// Sets IsSelected to True and displays the selected animation
		/// </summary>
		public void Select() {
			((Storyboard)Resources["SelectedSB"]).Begin();
			IsSelected = true;
		}

		/// <summary>
		/// Sets IsSelected to False and displays the DeSelected animation
		/// </summary>
		public void DeSelect() {
			((Storyboard)Resources["DeSelectedSB"]).Begin();
			IsSelected = false;
		}

		/// <summary>
		/// Begins the Initialization animiation
		/// </summary>
		public void BeginInitAnimation() {
			if (IsSelected)
				((Storyboard)Resources["InitSelectedSB"]).Begin();
			else
				((Storyboard)Resources["InitDeSelectedSB"]).Begin();
		}

		/// <summary>
		/// Fires BrowserEntered or BrowserSelected if they have been attached 
		/// and if IsSelected is true.
		/// </summary>
		private void Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
			if (IsSelected && BrowserEntered != null)
				BrowserEntered(this, null);
			else if (BrowserSelected != null)
				BrowserSelected(this, null);
		}
	}
}
