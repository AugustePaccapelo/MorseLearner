using System.Collections.Generic;
using System.Linq;
using Com.IsartDigital.OBG.Managers;
using Com.IsartDigital.OBG.Utils;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.UI.Menus
{
	public partial class HelpScreen : Control
	{
		// ---------- VARIABLES ---------- \\

		#region // ----- Singleton ----- \\
		static private HelpScreen instance;

		static public HelpScreen GetInstance()
		{
			if (instance == null) instance = new HelpScreen();
			return instance;

		}
		#endregion

		// ----- Paths ----- \\
		[Export] private PackedScene dotTextureScene, dashTextureScene;
		private const string PATH_LABEL = "Letter";

		// ----- Nodes ----- \\
		[Export] private Button playButton;
		[Export] private VBoxContainer vBoxCont;
		private List<HBoxContainer> allHBoxContainers = new List<HBoxContainer>();

		// ----- Others ----- \\
		public int numLettersKnown = 3;
		public int numLetterToShow;
		private List<string> lettersToShow;

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		private HelpScreen() : base() { }

		public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(HelpScreen) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

			base._Ready();

			playButton.Pressed += PlayButtonPressed;

			foreach (HBoxContainer lCont in vBoxCont.GetChildren())
			{
				allHBoxContainers.Add(lCont);
			}
			numLetterToShow = allHBoxContainers.Count;

			lettersToShow = MorseCode.alphabet.Keys.ToList().GetRange(numLettersKnown - numLetterToShow, numLetterToShow);

			for (int i = 0; i < numLetterToShow; i++)
			{
				HBoxContainer lHCont = allHBoxContainers[i];
				Label lLab = lHCont.GetNode<Label>(PATH_LABEL);
				lLab.Text = lettersToShow[i];
				AddMorseCode(lettersToShow[i], lHCont);
			}
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		private void PlayButtonPressed()
		{
			SignalsManager.GetInstance().EmitSignal(SignalsManager.SignalName.LaunchGame, 0);
			QueueFree();
		}

		private void AddMorseCode(string pLetter, HBoxContainer pCont)
		{
			string lCode = MorseCode.alphabet[pLetter];
			foreach (char lCarac in lCode)
			{
				switch (lCarac)
				{
					case MorseCode.DOT_CHARAC:
                        AddDot(pCont);
						break;
					case MorseCode.DASH_CHARAC:
						AddDash(pCont);
						break;
				}
			}
		}

		private void AddDot(Control lParent)
		{
			TextureRect lDot = dotTextureScene.Instantiate<TextureRect>();
			lParent.AddChild(lDot);
		}

		private void AddDash(Control lParent)
		{
			TextureRect lDash = dashTextureScene.Instantiate<TextureRect>();
			lParent.AddChild(lDash);
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
