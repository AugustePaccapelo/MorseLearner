using System.Diagnostics.SymbolStore;
using Com.IsartDigital.OBG.Managers;
using Com.IsartDigital.OBG.Utils;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.UI
{
	public partial class HUD : Control
	{
		// ---------- VARIABLES ---------- \\

		#region // ----- Singleton ----- \\
		static private HUD instance;

		static public HUD GetInstance()
		{
			if (instance == null) instance = new HUD();
			return instance;

		}
		#endregion

		// ----- Paths ----- \\
		[Export] private PackedScene goodDotScene;
        [Export] private PackedScene goodDashScene;

        // ----- Nodes ----- \\
		[Export] private Label currentLetterLabel;
		[Export] private Label confirmationLabel;
        [Export] private HBoxContainer morseCodeConainer;

        // ----- Others ----- \\
		private string wrongMorseCodeConfirmation = "Sorry !";

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		private HUD() : base() { }

		public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(HUD) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

			base._Ready();

			LevelManager.GetInstance().hud = this;
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		public void UpdateLetter(string pLetter)
		{
			currentLetterLabel.Text = pLetter;
		}

		public void UpdateConfirmation(bool pIsWrong)
		{
			confirmationLabel.Text = pIsWrong ? wrongMorseCodeConfirmation : "";
		}

		public void UpdateMorse(string pCharac)
		{
			if (pCharac == MorseCode.DOT_CHARAC) CreateDot();
			else if (pCharac == MorseCode.DASH_CHARAC) CreateDash();
		}

		public void ClearMorseCode()
		{
			foreach (Node lNode in morseCodeConainer.GetChildren())
			{
				lNode.QueueFree();
			}
		}

		private void CreateDot()
		{
			TextureRect lDot = goodDotScene.Instantiate<TextureRect>();
			morseCodeConainer.AddChild(lDot);
		}

		private void CreateDash()
		{
			TextureRect lDash = goodDashScene.Instantiate<TextureRect>();
			morseCodeConainer.AddChild(lDash);
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
