using Com.IsartDigital.OBG.managers;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Managers
{
	public partial class GameManager : Node2D
	{
        // ---------- VARIABLES ---------- \\

        #region // ----- Singleton ----- \\
        static private GameManager instance;

        static public GameManager GetInstance()
        {
            if (instance == null) instance = new GameManager();
            return instance;

        }
		#endregion


		// ----- Paths ----- \\
		[Export] private PackedScene inputManagerScene;
        [Export] private PackedScene uiManagerScene;

		// ----- Nodes ----- \\
		private SignalsManager signalsManager;
		private InputManager inputManager;
		private UIManager uiManager;

		// ----- Others ----- \\
		public Vector2 screenSize { get; private set; }

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		private GameManager() : base() { }

		public override void _Ready()
		{
            #region Singleton Ready
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(GameManager) + " Instance already exist, destroying the last added.");
                return;
            }

            instance = this;
            #endregion


            base._Ready();

			screenSize = GetViewportRect().Size;

			CreateAllManagers();
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		private void CreateAllManagers()
		{
			signalsManager = SignalsManager.GetInstance();
			signalsManager.PlayButtonPressed += PlayPressed;
			inputManager = inputManagerScene.Instantiate<InputManager>();
			AddChild(inputManager);
			uiManager = uiManagerScene.Instantiate<UIManager>();
			AddChild(uiManager);
		}

		private void PlayPressed()
		{
			inputManager.canPlay = true;
		}

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
            #region Singleton Dispose
            if (pDisposing && instance == this) instance = null;
            #endregion

            base.Dispose(pDisposing);

			signalsManager.PlayButtonPressed -= PlayPressed;
		}
	}
}
