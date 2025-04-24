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
        [Export] private Button PlayButton;
        [Export] private Button quitButton;
		[Export] private Button soundOnButton;
		[Export] private Button soundOffButton;

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
            PlayButton.Pressed += PlayFunction;
			quitButton.Pressed += QuitFunction;
            soundOnButton.Pressed += TurnSoundOn;
            soundOffButton.Pressed += TurnSoundOff;
		}

		private void PlayFunction()
		{
            SignalsManager.GetInstance().EmitSignal(SignalsManager.SignalName.PlayButtonPressed);
		}

		private void QuitFunction()
		{
			GetTree().Quit();
		}

		private void TurnSoundOn()
        {
            GD.Print("Sound On");
            soundOffButton.Show();
            soundOnButton.Hide();
        }

        private void TurnSoundOff()
        {
            GD.Print("Sound Off");
            soundOffButton.Hide();
            soundOnButton.Show();
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
