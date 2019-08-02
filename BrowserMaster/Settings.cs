
namespace BrowserMaster {
	public class Settings {
		#region CTOR
		public Settings() {		}
		public Settings(bool showGui) {
			ShowGui = showGui;
		}
		#endregion

		public bool ShowGui { get; set; }
		public int DelayInSeconds { get; set; }

        #region Factory
        public static Settings Default
        {
            get {
                return new Settings()
                {
                    ShowGui = true,
                    DelayInSeconds = 2
                };
            }
        }
        #endregion
    }
}
