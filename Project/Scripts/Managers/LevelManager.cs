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
		private float unitTime = 0.2f;
		private const int DOT_UNIT = 1;
        private const int DASH_UNIT = 3;
        private const int LITTLE_SPACE_UNIT = 1;
		private const int LETTERS_SPACE_UNIT = 3;
		private const int WORDS_SPACE_UNIT = 5;
		private float timeErrorMargin = 0.05f;
		private bool inputPressed = false;
		private bool inputReleased = false;
		private float currentHoldingTime;
		private float currentSpacingTime;

		public bool letterSequence = true;

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

			signalsManager = SignalsManager.GetInstance();
			signalsManager.InputPressed += InputSignalPressed;
			signalsManager.InputReleased += InputSignalReleased;
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);

			if (inputPressed)
			{
				currentHoldingTime += lDelta;
			}
			if (inputReleased)
			{
				currentSpacingTime += lDelta;
			}
		}

		// ----- My Functions ----- \\

		private void InputSignalPressed()
		{
			inputPressed = true;
			inputReleased = false;
			currentHoldingTime = 0f;
			
            DetectSpace();
        }

		private void InputSignalReleased()
		{
            inputPressed = false;
			inputReleased = true;
			DetectInput();
        }

		private void NewDot()
		{
			hud.UpdateMorse(MorseCode.DOT_CHARAC);
		}

		private void NewDash()
		{
            hud.UpdateMorse(MorseCode.DASH_CHARAC);
        }

		private void NewLittleSpace()
		{
            hud.UpdateMorse(MorseCode.LITTLE_SPACE_CHARAC);
        }

		private void NewLetter()
		{
            hud.UpdateMorse(MorseCode.LETTER_SPACE_CHARAC);
        }

		private void NewWord()
		{
            hud.UpdateMorse(MorseCode.WORD_SPACE_CHARAC);
        }

		private void DetectInput()
		{	
            // Click
            if (currentHoldingTime <= DOT_UNIT * unitTime + timeErrorMargin)
			{
				NewDot();

                return;
			}
			if (currentHoldingTime <= DASH_UNIT * unitTime + timeErrorMargin)
			{
				NewDash();
				return;
			}
		}

		private void DetectSpace()
		{
			if (currentSpacingTime <= LETTERS_SPACE_UNIT * unitTime - timeErrorMargin)
			{
				NewLittleSpace();
			}
			else if (letterSequence || currentSpacingTime <= WORDS_SPACE_UNIT * unitTime - timeErrorMargin)
			{
				NewLetter();
			}
			else if (currentSpacingTime <= WORDS_SPACE_UNIT * unitTime + timeErrorMargin)
			{
				NewWord();
			}
			
			currentSpacingTime = 0f;
        }

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
			#region Singleton Dispose
			if (pDisposing && instance == this) instance = null;
			#endregion

			base.Dispose(pDisposing);

            signalsManager.InputPressed -= InputSignalPressed;
            signalsManager.InputReleased -= InputSignalReleased;
        }
	}
}
