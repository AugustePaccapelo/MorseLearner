using System.Collections.Generic;
using System.Linq;
using Com.IsartDigital.OBG.managers;
using Com.IsartDigital.OBG.Morse;
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
        [Export] private PackedScene DotScene;
        [Export] private PackedScene DashScene;

        // ----- Nodes ----- \\
        private SignalsManager signalsManager;
		public HUD hud;
        private Node2D gameContainer;
        public Control startMorseCodePos;

        // ----- Others ----- \\

        private RandomNumberGenerator rand = new RandomNumberGenerator();

		private string[] allLetters;

		public bool letterSequence = true;
		private string currentLetter;
		private string currentLetterMorseCode;
		private string currentMorseCode;
		private bool wasWrong = false;

        public List<MorseCharacter> allMorseCharacters = new List<MorseCharacter>();
        [Export] private float morseHeight = 40f;
        [Export] private float separation = 10f;

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
			
			signalsManager.NewCharacter += NewCharacter;
			signalsManager.WrongCharacter += WrongCharacter;

			allLetters = MorseCode.alphabet.Keys.ToArray();
            gameContainer = GameManager.GetInstance().gameContainer;
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
            ClearMorseCode();
            hud.UpdateLetter(currentLetter);
		}

		public void NewCharacter()
		{
			if (IsLastCharacterGood())
			{
                if (wasWrong)
                {
                    wasWrong = false;
                    hud.UpdateConfirmation(false);
                }
                allMorseCharacters.Last().GoodAnimation();
				InputManager.GetInstance().canPlay = true;
				if (IsCodeFinished())
				{
					GetRandomLetter();
				}
			}
			else
			{
                foreach (MorseCharacter lCharac in allMorseCharacters)
                {
                    lCharac.SetBroken();
                }
                allMorseCharacters.Last().BrokenAnimation();
            }
		}

		public void WrongCharacter()
		{
            InputManager.GetInstance().canPlay = true;
            wasWrong = true;
            hud.UpdateConfirmation(true);
            ClearMorseCode();
        }

		private bool IsLastCharacterGood()
		{
			int lLastCharacIndex = currentMorseCode.Length - 1;
			return currentLetterMorseCode[lLastCharacIndex] == currentMorseCode[lLastCharacIndex];
		}

		private bool IsCodeFinished()
		{
			return currentLetterMorseCode.Length == currentMorseCode.Length;
		}

		private void InputClick()
		{
			NewDot();
        }

		private void InputStartHold()
		{
			NewDash();
        }

		private void InputStopHold()
		{
            
        }

        public void ClearMorseCode()
        {
			currentMorseCode = "";
            int lLength = allMorseCharacters.Count - 1;
            for (int i = lLength; i > -1; i--)
            {
                allMorseCharacters[i].QueueFree();
                allMorseCharacters.RemoveAt(i);
            }
        }

        private Vector2 GetLastPosition(MorseCharacter pMorse)
        {
            Vector2 lPos = startMorseCodePos.GlobalPosition;
            if (allMorseCharacters.Count > 0)
            {
                MorseCharacter lLast = allMorseCharacters.Last();
                lPos.X = lLast.GlobalPosition.X + lLast.TextureSize.X * 0.5f;
            }
            lPos.X += pMorse.TextureSize.X * 0.5f + separation;
            return lPos;
        }

        private void NewDot()
		{
			char lCharac = MorseCode.DOT_CHARAC;
			currentMorseCode += lCharac;
            Dot lDot = DotScene.Instantiate<Dot>();
            gameContainer.AddChild(lDot);
            lDot.GlobalPosition = GetLastPosition(lDot);
            allMorseCharacters.Add(lDot);
            lDot.SpawnAnimation();
        }

		private void NewDash()
		{
            char lCharac = MorseCode.DASH_CHARAC;
            currentMorseCode += lCharac;
            Dash lDash = DashScene.Instantiate<Dash>();
            gameContainer.AddChild(lDash);
            lDash.GlobalPosition = GetLastPosition(lDash);
            allMorseCharacters.Add(lDash);
            lDash.SpawnAnimation();
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

            signalsManager.NewCharacter -= NewCharacter;
            signalsManager.WrongCharacter -= WrongCharacter;
        }
	}
}
