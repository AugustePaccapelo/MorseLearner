using Com.IsartDigital.OBG.Managers;
using Com.IsartDigital.OBG.Utils;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.UI.Menus
{
	public partial class LevelSelector : Control
	{
		// ---------- VARIABLES ---------- \\

		#region // ----- Singleton ----- \\
		static private LevelSelector instance;

		static public LevelSelector GetInstance()
		{
			if (instance == null) instance = new LevelSelector();
			return instance;

		}
        #endregion

        // ----- Paths ----- \\
        [Export] private PackedScene helpScreenScene;

        // ----- Nodes ----- \\
        [Export] private Button learningButton, easyButton, normalButton, hardButton, hardcoreButton;

		// ----- Others ----- \\
		public static int numLettersKnown = 3;

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		private LevelSelector() : base() { }

		public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(LevelSelector) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

			base._Ready();

			learningButton.Pressed += () => PlayGame(0);
			easyButton.Pressed += () => PlayGame(1);
			normalButton.Pressed += () => PlayGame(2);
			hardButton.Pressed += () => PlayGame(3);
			hardcoreButton.Pressed += () => PlayGame(4);
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		private void PlayGame(int pDifficulty)
		{
			if (pDifficulty == 0)
			{
				HelpScreen lHelp = helpScreenScene.Instantiate<HelpScreen>();
				lHelp.numLettersKnown = numLettersKnown;
				AddChild(lHelp);
			}
			else
			{
				CustomSignals.GoToInGame?.Invoke(pDifficulty);
			}
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
