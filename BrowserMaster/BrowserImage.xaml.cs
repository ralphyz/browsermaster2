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

namespace BrowserMaster
{
    /// <summary>
    /// Interaction logic for BrowserImage.xaml
    /// </summary>
    public partial class BrowserImage : UserControl
    {

        public event EventHandler BrowserSelected;
        public event EventHandler BrowserEntered;

        private double fullSize;
        private const int widthAnimation = 0;
        private const int heightAnimation = 1;

        public string NameOfBrowser { get { return Browser.Name; } }
        public BrowserConfig Browser { get; set; }


        public BrowserImage(BrowserConfig b, double width, Thickness margin)
        {
            InitializeComponent();

            //-- Initialize the size of the control
            Height = Width = width;
            Margin = margin;

            //-- Initialize Full Size
            fullSize = width;

            //-- Grab the Image
            Icon.Source = GetImage(b.IconPath);

            //-- Initialize Animations
            InitializeAnimations();

            //-- Keep a reference of the browser used to initialize this Browser Image
            Browser = b;
        }

        private void InitializeAnimations()
        {
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

        /// <summary>
        /// Returns a new ImageSource from an absolute path
        /// </summary>
        /// <param name="path">Absolute path to image file [PNG, JPEG, BMP]</param>
        private ImageSource GetImage(string path)
        {
            //-- Create Source
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path);
            bi.DecodePixelHeight = (int)fullSize;
            bi.DecodePixelWidth = (int)fullSize;
            bi.EndInit();
            return bi;
        }

        public bool IsSelected { get; set; }

        /// <summary>
        /// Sets IsSelected to True and displays the selected animation
        /// </summary>
        public void Select()
        {
            ((Storyboard)Resources["SelectedSB"]).Begin();
            IsSelected = true;
        }

        /// <summary>
        /// Sets IsSelected to False and displays the DeSelected animation
        /// </summary>
        public void DeSelect()
        {
            ((Storyboard)Resources["DeSelectedSB"]).Begin();
            IsSelected = false;
        }

        /// <summary>
        /// Begins the Initialization animiation
        /// </summary>
        public void BeginInitAnimation()
        {
            if (IsSelected)
                ((Storyboard)Resources["InitSelectedSB"]).Begin();
            else
                ((Storyboard)Resources["InitDeSelectedSB"]).Begin();
        }

        /// <summary>
        /// Fires BrowserEntered or BrowserSelected if they have been attached 
        /// and if IsSelected is true.
        /// </summary>
        private void Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsSelected && BrowserEntered != null)
                BrowserEntered(this, null);
            else if (BrowserSelected != null)
                BrowserSelected(this, null);
        }

        public override string ToString()
        {
            if (this.Browser == null) return "<NULL>";
            return string.Format("{0}{1}", IsSelected ? "Selected " : string.Empty, this.Browser);
        }
    }
}
