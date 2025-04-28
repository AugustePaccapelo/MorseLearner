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

		private void NewLittleSpace()
		{
			GD.Print("Little space");
		}

		private void NewLetter()
		{
			GD.Print("New Letter");
		}

		private void NewWord()
		{
			GD.Print("New Word");
		}

		private void DetectInput()
		{
			// If click to chort, ignore
			if (currentHoldingTime < DOT_UNIT * unitTime - timeErrorMargin) return;
			
            // Click
            if (currentHoldingTime <= DOT_UNIT * unitTime + timeErrorMargin)
			{
				GD.Print(".");
				return;
			}
			if (currentHoldingTime <= DASH_UNIT * unitTime + timeErrorMargin)
			{
				GD.Print("-");
				return;
			}
		}

		private void DetectSpace()
		{
			if (currentSpacingTime < LITTLE_SPACE_UNIT * unitTime - timeErrorMargin)
			{
				inputPressed = false;
				GD.Print("To short");
			}

			else if (currentSpacingTime <= LETTERS_SPACE_UNIT * unitTime - timeErrorMargin)
			{
				GD.Print("Little Space");
			}

			else if (currentSpacingTime <= WORDS_SPACE_UNIT * unitTime - timeErrorMargin)
			{
				GD.Print("Letter Space");
			}
			else if (currentSpacingTime <= WORDS_SPACE_UNIT * unitTime + timeErrorMargin)
			{
				GD.Print("Word Space");
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
