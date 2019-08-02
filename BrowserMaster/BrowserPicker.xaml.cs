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

namespace BrowserMaster
{
    /// <summary>
    /// Interaction logic for BrowserPicker.xaml
    /// </summary>
    public partial class BrowserPicker : Window
    {
        private int _selectedIndex = 0;
        private int _size = 150;
        private BrowserConfigs _browsers;
        private System.Timers.Timer _timer;
        private delegate void VoidDelagate();

        public BrowserConfigs BrowsersToUse { get; protected set; }

        public BrowserPicker() { InitializeComponent(); }

        public BrowserPicker(Configuration config)
            : this()
        {
#if DEBUG
            this.ShowInTaskbar = true;
#endif
            _browsers = config.Browsers.EnabledAndInstalled;
            if (config.Settings.DelayInSeconds > 0)
            {
                _timer = new System.Timers.Timer(config.Settings.DelayInSeconds * 1000);
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            }

            Height = _size * 1.2;
            Width = _browsers.Count * (_size + ((_size * .2) * 2));
            InitializeGrid(_size, new Thickness(_size * .2, 0, _size * .2, 0));
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new VoidDelagate(SetAllBrowsersAndClose));
        }

        private void InitializeGrid(double width, Thickness thickness)
        {
            bool isSelected = true;
            foreach (BrowserConfig bc in _browsers)
            {
                if (!(bc.Enabled && bc.Installed)) continue;
                BrowserImage bi = new BrowserImage(bc, width, thickness);

                //-- Attach to the selected event & entered event
                bi.BrowserSelected += new EventHandler(bi_BrowserSelected);
                bi.BrowserEntered += new EventHandler(bi_BrowserEntered);

                //-- Add the BrowserImage to our Stack Panel
                MainPanel.Children.Add(bi);

                //-- Set the Selected Bit
                bi.IsSelected = isSelected;

                //-- Initialize the Browser Image [Run the initialize Animation]
                bi.BeginInitAnimation();
                isSelected = false;
            }
        }

        private void bi_BrowserEntered(object sender, EventArgs e)
        {
            BrowserImage b = sender as BrowserImage;
            if (b != null)
            {
                for (int i = 0; i < MainPanel.Children.Count; i++)
                {
                    if ((BrowserImage)MainPanel.Children[i] == b && _selectedIndex != i)
                    {
                        _selectedIndex = i;
                        UpdateSelectedBrowser();
                    }
                }
            }
            SetSelectedAsBrowserToUseAndClose();
        }

        private void bi_BrowserSelected(object sender, EventArgs e)
        {
            BrowserImage b = sender as BrowserImage;

            if (b != null)
            {
                for (int i = 0; i < MainPanel.Children.Count; i++)
                {
                    if (MainPanel.Children[i] == b)
                    {
                        _selectedIndex = i;
                        UpdateSelectedBrowser();
                        return;
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.Left:
                    MovePrevious();
                    break;
                case Key.Down:
                case Key.Right:
                    MoveNext();
                    break;
                case Key.Escape:
                    SetNoBrowsersAndClose();
                    break;
                case Key.Enter:
                    SetSelectedAsBrowserToUseAndClose();
                    break;
                default:
                    break;
            }
        }


        private void MovePrevious()
        {
            --_selectedIndex;
            if (_selectedIndex < 0)
                _selectedIndex = MainPanel.Children.Count - 1;
            UpdateSelectedBrowser();
        }

        private void MoveNext()
        {
            ++_selectedIndex;
            if (_selectedIndex == MainPanel.Children.Count)
                _selectedIndex = 0;

            UpdateSelectedBrowser();
        }

        private void UpdateSelectedBrowser()
        {
            StopTimer();
            for (int i = 0; i < MainPanel.Children.Count; i++)
            {
                BrowserImage bi = MainPanel.Children[i] as BrowserImage;
                if (i == _selectedIndex)
                    bi.Select();
                else
                    bi.DeSelect();
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                MoveNext();
            else
                MovePrevious();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_timer != null) _timer.Start();
        }
        private void StopTimer()
        {
            if (_timer != null) _timer.Stop();
        }

        protected void SetAllBrowsersAndClose()
        {
            BrowsersToUse = this._browsers;
            Close();
        }
        protected void SetSelectedAsBrowserToUseAndClose()
        {
            BrowsersToUse = new BrowserConfigs() { _browsers[_selectedIndex] };
            Close();
        }
        private void SetNoBrowsersAndClose()
        {
            BrowsersToUse = null;
            Close();
        }

    }
}
