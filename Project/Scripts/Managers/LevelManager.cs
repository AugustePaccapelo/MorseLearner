using System.Collections.Generic;
using System.Linq;
using Com.IsartDigital.OBG.UI;
using Com.IsartDigital.OBG.Utils;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Managers
{
	public partial class LevelManager : Node2D
	{
		// ---------- VARIABLES ---------- \\

		#region // ----- Singleton ----- \\
		static private LevelManager instance;

		static public LevelManager GetInstance()
		{
			if (instance == null) instance = new LevelManager();
			return instance;

		}
		#endregion

		// ----- Paths ----- \\

		// ----- Nodes ----- \\
		private SignalsManager signalsManager;
		public HUD hud;

		// ----- Others ----- \\

		private RandomNumberGenerator rand = new RandomNumberGenerator();

		private string[] allLetters;

		public bool letterSequence = true;
		private string currentLetter;
		private string currentLetterMorseCode;
		private string currentMorseCode;
		private bool wasWrong = false;

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		private LevelManager() : base() { }

		public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(LevelManager) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

			base._Ready();

			rand.Randomize();

			signalsManager = SignalsManager.GetInstance();
			signalsManager.InputClick += InputClick;
			signalsManager.InputStartHold += InputStartHold;
			signalsManager.InputStopHold += InputStopHold;
			signalsManager.PlayButtonPressed += StartGame;

			allLetters = MorseCode.alphabet.Keys.ToArray();
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		private void StartGame(int pDifficulty)
		{
			GetRandomLetter();
		}

		private void GetRandomLetter()
		{
			int lLength = allLetters.Length;
			int lIndex = rand.RandiRange(0, lLength - 1);
			string lCurrentLetter = allLetters[lIndex];
			string lCurrentMorseCode = MorseCode.alphabet[lCurrentLetter];
			currentLetter = lCurrentLetter;
			currentLetterMorseCode = lCurrentMorseCode;
			currentMorseCode = "";
            hud.ClearMorseCode();
            hud.UpdateLetter(currentLetter);
		}

		private bool IsCurrentCodeCorrect()
		{
            return currentMorseCode == currentLetterMorseCode.Substring(0, currentMorseCode.Length);
		}

		private bool VerifyCurrentMorse()
		{
			if (!IsCurrentCodeCorrect())
			{
                wasWrong = true;
                hud.UpdateConfirmation(true);
                currentMorseCode = "";
				hud.ClearMorseCode();
            }
			else if (wasWrong)
			{
                wasWrong = false;
                hud.UpdateConfirmation(false);
                return true;
            }

			return false;
        }

		private bool IsCodeFinished()
		{
			return currentMorseCode.Length == currentLetterMorseCode.Length && IsCurrentCodeCorrect();
		}

		private void InputClick()
		{
			NewDot();
			NewCharacter();
        }

		private void InputStartHold()
		{
			NewDash();
        }

		private void InputStopHold()
		{
            NewCharacter();
        }

		private void NewCharacter()
		{
            if (IsCodeFinished())
            {
                GD.Print("GG !");
                GetRandomLetter();
            }
			else
			{
				VerifyCurrentMorse();
			}
        }

		private void NewDot()
		{
			string lCharac = MorseCode.DOT_CHARAC;
			currentMorseCode += lCharac;
            hud.UpdateMorse(lCharac);
        }

		private void NewDash()
		{
            currentMorseCode += MorseCode.DASH_CHARAC;
            hud.UpdateMorse(MorseCode.DASH_CHARAC);
        }

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
			#region Singleton Dispose
			if (pDisposing && instance == this) instance = null;
			#endregion

			base.Dispose(pDisposing);

            signalsManager.InputClick -= InputClick;
            signalsManager.InputStartHold -= InputStartHold;
            signalsManager.InputStopHold -= InputStopHold;
            signalsManager.PlayButtonPressed -= StartGame;
        }
	}
}
