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

		private float unitTime = 0.15f;
		private const int DOT_UNIT = 1;
		private const int DASH_UNIT = 3;
		private const int LITTLE_SPACE_UNIT = 1;
		private const int LETTERS_SPACE_UNIT = 3;
		private const int WORDS_SPACE_UNIT = 5;
		private float timeErrorMargin = 0.1f;
		private bool inputPressed = false;
		private bool inputReleased = false;
		private float currentHoldingTime;
		private float currentSpacingTime;
		private bool isFirstCharacter = true;

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
			signalsManager.InputPressed += InputSignalPressed;
			signalsManager.InputReleased += InputSignalReleased;
			signalsManager.PlayButtonPressed += StartGame;

			allLetters = MorseCode.alphabet.Keys.ToArray();
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

		private void StartGame()
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
			hud.UpdateLetter(currentLetter);
			hud.UpdateMorse(currentMorseCode);
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
                hud.UpdateConfirmation(false);
                currentMorseCode = "";
                hud.UpdateMorse(currentMorseCode);
            }
			else if (wasWrong)
			{
                wasWrong = false;
                hud.UpdateConfirmation(true);
                return true;
            }

			return false;
        }

		private bool IsCodeFinished()
		{
			return currentMorseCode.Length == currentLetterMorseCode.Length && IsCurrentCodeCorrect();
		}

		private void InputSignalPressed()
		{
			inputPressed = true;
			inputReleased = false;
			currentHoldingTime = 0f;
			
			if (!isFirstCharacter)
			{
                DetectSpace();
            }
        }

		private void InputSignalReleased()
		{
            inputPressed = false;
			inputReleased = true;
			DetectInput();
            hud.UpdateMorse(currentMorseCode);
			if (!VerifyCurrentMorse())
			{
				isFirstCharacter = true;
			}
			if (IsCodeFinished())
			{
				GD.Print("GG !");
				GetRandomLetter();
			}
        }

		private void NewDot()
		{
			currentMorseCode += MorseCode.DOT_CHARAC;
		}

		private void NewDash()
		{
            currentMorseCode += MorseCode.DASH_CHARAC;
        }

		private void NewLittleSpace()
		{
            currentMorseCode += MorseCode.LITTLE_SPACE_CHARAC;
        }

		private void NewLetter()
		{
            currentMorseCode += MorseCode.LETTER_SPACE_CHARAC;
        }

		private void NewWord()
		{
            currentMorseCode += MorseCode.WORD_SPACE_CHARAC;
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
				if (VerifyCurrentMorse())
				{
                    NewLetter();
                }
			}
			else if (currentSpacingTime <= WORDS_SPACE_UNIT * unitTime + timeErrorMargin)
			{
				if (VerifyCurrentMorse())
				{
                    NewWord();
                }
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
            signalsManager.PlayButtonPressed -= StartGame;
        }
	}
}
