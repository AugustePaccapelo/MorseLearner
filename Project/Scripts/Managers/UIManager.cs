using Com.IsartDigital.OBG.Managers;
using Com.IsartDigital.OBG.UI.Menus;
using Com.IsartDigital.OBG.UI;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.managers
{
	public partial class UIManager : Control
	{
        // ---------- VARIABLES ---------- \\

        #region // ----- Singleton ----- \\
        static private UIManager instance;

        static public UIManager GetInstance()
        {
            if (instance == null) instance = new UIManager();
            return instance;

        }
        #endregion


        // ----- Paths ----- \\
        [Export] private PackedScene titleCardScene;
		[Export] private PackedScene mainMenuScene;
		[Export] private PackedScene levelSelectorScene;
		[Export] private PackedScene hudScene;

		// ----- Nodes ----- \\
		private SignalsManager signalsManager;

		// ----- Others ----- \\
		private Vector2 screenSize;

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		private UIManager() : base() { }

		public override void _Ready()
		{
            #region Singleton Ready
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(UIManager) + " Instance already exist, destroying the last added.");
                return;
            }

            instance = this;
            #endregion


            base._Ready();

			screenSize = GameManager.GetInstance().screenSize;
			CustomMinimumSize = screenSize;
			Position = screenSize * 0.5f;

			signalsManager = SignalsManager.GetInstance();
			signalsManager.GoToMainMenu += GoToMainMenu;
			signalsManager.GoToLevelSelector += GoToLevelSelector;
			signalsManager.PlayButtonPressed += (pDifficulty) => PlayPressed();

			AddChild(titleCardScene.Instantiate());
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		private void GoToMainMenu()
		{
			MainMenu lMainMenu = mainMenuScene.Instantiate<MainMenu>();
			AddChild(lMainMenu);
		}

		private void GoToLevelSelector()
		{
            MainMenu.GetInstance().QueueFree();
            LevelSelector lLevelSelector = levelSelectorScene.Instantiate<LevelSelector>();
			AddChild(lLevelSelector);
		}

		private void PlayPressed()
		{
			LevelSelector.GetInstance().QueueFree();
			AddChild(hudScene.Instantiate<HUD>());
		}

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
            #region Singleton Dispose
            if (pDisposing && instance == this) instance = null;
            #endregion

            base.Dispose(pDisposing);

            signalsManager.GoToMainMenu -= GoToMainMenu;
            signalsManager.GoToLevelSelector -= GoToLevelSelector;
            signalsManager.PlayButtonPressed -= (pDifficulty) => PlayPressed();
        }
	}
}