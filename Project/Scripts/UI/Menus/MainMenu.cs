using Com.IsartDigital.OBG.Managers;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.UI.Menus
{
	public partial class MainMenu : Control
	{
        // ---------- VARIABLES ---------- \\

        #region // ----- Singleton ----- \\
        static private MainMenu instance;

        static public MainMenu GetInstance()
        {
            if (instance == null) instance = new MainMenu();
            return instance;

        }
        #endregion

        // ----- Paths ----- \\

        // ----- Nodes ----- \\
        [Export] private Button buttonPlay;
        [Export] private Button buttonSettings;
        [Export] private Button buttonCredits;
        [Export] private Button buttonQuit;

        // ----- Others ----- \\

        // ---------- FUNCTIONS ---------- \\

        // ----- Constructor & Ready & Process ----- \\

        private MainMenu() : base() { }

		public override void _Ready()
		{
            #region Singleton Ready
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(MainMenu) + " Instance already exist, destroying the last added.");
                return;
            }

            instance = this;
            #endregion


            base._Ready();

			SetButtons();
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		private void SetButtons()
		{
            buttonPlay.Pressed += PlayFunction;
			buttonSettings.Pressed += SettingsFunction;
			buttonCredits.Pressed += CreditsFunction;
			buttonQuit.Pressed += QuitFunction;
		}

		private void PlayFunction()
		{
			GD.Print("Play game");
		}

		private void SettingsFunction()
		{
			GD.Print("Go to settings");
		}

		private void CreditsFunction()
		{
			GD.Print("Go to credits");
		}

		private void QuitFunction()
		{
			GetTree().Quit();
		}

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
            #region Singleton Dispose
            if (pDisposing && instance == this) instance = null;
            #endregion

            base.Dispose(pDisposing);
		}
	}
}
