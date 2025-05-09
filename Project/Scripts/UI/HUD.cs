using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Com.IsartDigital.OBG.Managers;
using Com.IsartDigital.OBG.Morse;
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

        // ----- Nodes ----- \\
		[Export] private Label currentLetterLabel;
		[Export] private Label confirmationLabel;
        [Export] public Control startMorseCodePos { get; private set; }

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

			LevelManager lLevelManager = Manager.GetManager<LevelManager>();
            lLevelManager.hud = this;
            lLevelManager.startMorseCodePos = startMorseCodePos;
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
