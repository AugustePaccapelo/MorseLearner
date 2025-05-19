using System.Collections.Generic;
using System.Linq;
using Com.IsartDigital.OBG.Morse;
using Com.IsartDigital.OBG.UI;
using Com.IsartDigital.OBG.Utils;
using Com.IsartDigital.OBG.UI.Menus;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Managers
{
	public partial class LevelManager : Manager
	{
		// ---------- VARIABLES ---------- \\

        // ----- Paths ----- \\
        [Export] private PackedScene DotScene;
        [Export] private PackedScene DashScene;

        // ----- Nodes ----- \\
		public HUD hud;
        [Export] private Node2D gameContainer;
        public Control startMorseCodePos;

        // ----- Others ----- \\

        private RandomNumberGenerator rand = new RandomNumberGenerator();

		private List<string> allLetters = new List<string>();

		public string currentLetter { get; private set; }
        public string secondLetter { get; private set; }
        public string thirdLetter { get; private set; }
        private string currentLetterMorseCode;
		private string currentMorseCode;
		private bool wasWrong = false;
		private bool isCurrentlyWrong = false;

        public List<MorseCharacter> allMorseCharacters = new List<MorseCharacter>();
        [Export] private float morseHeight = 40f;
        [Export] private float separation = 10f;

        public static int streak = 0;
        public bool letterFinished = false;

        // ---------- FUNCTIONS ---------- \\

        // ----- Init & Process ----- \\

        protected override void Init()
        {
            rand.Randomize();

            CustomSignals.InputClick += InputClick;
            CustomSignals.InputStartHold += InputStartHold;
            CustomSignals.InputStopHold += InputStopHold;
            CustomSignals.NewCharacter += NewCharacter;
            CustomSignals.ErrorInCode += WrongCharacter;

            allLetters = MorseCode.alphabet.Keys.ToList();
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		public void StartGame(int pDifficulty)
		{
			if (pDifficulty == 0)
			{
				allLetters = allLetters.Take(LevelSelector.numLettersKnown).ToList();
			}
            //GetRandomLetter();

            thirdLetter = GetRandomLetter();
            secondLetter = GetRandomLetter();

            NewLetter();
		}

		private string GetRandomLetter()
		{
			int lLength = allLetters.Count;
			int lIndex = rand.RandiRange(0, lLength - 1);
            
            return allLetters[lIndex];
        }

        public void NewLetter()
        {
            currentLetter = secondLetter;
            secondLetter = thirdLetter;
            if (streak >= 3 && LevelSelector.numLettersKnown < MorseCode.alphabet.Count)
            {
                allLetters.Add(MorseCode.alphabet.Keys.ToList()[LevelSelector.numLettersKnown]);
                thirdLetter = allLetters.Last();
            }
            else
            { 
                thirdLetter = GetRandomLetter();
            }

            string lCurrentMorseCode = MorseCode.alphabet[currentLetter];
            currentLetterMorseCode = lCurrentMorseCode;

            hud.UpdateLetter(currentLetter, secondLetter, thirdLetter);
        }

		public void NewCharacter(MorseCharacter pCharac)
		{
            if (!isCurrentlyWrong)
			{
                if (wasWrong)
                {
                    wasWrong = false;
                    hud.UpdateConfirmation(false);
                }
                pCharac.GoodAnimation();
				if (!letterFinished && IsCodeFinished())
				{
                    letterFinished = true;
                    if (currentLetter == allLetters.Last()) streak++;
                    HUD.GetInstance().LetterFinishedAnimation();
                    GetManager<InputManager>().canPlay = false;
				}
			}
			else
			{
                streak = 0;
				isCurrentlyWrong = true;
                foreach (MorseCharacter lCharac in allMorseCharacters)
                {
                    lCharac.SetBroken();
                }
                pCharac.BrokenAnimation();
            }
		}

		private void VerifyCurrentCode()
		{
			if (isCurrentlyWrong) return;
			isCurrentlyWrong = !IsLastCharacterGood();
		}

		public void WrongCharacter()
		{
            wasWrong = true;
            hud.UpdateConfirmation(true);
            ClearMorseCode();
        }

		private bool IsLastCharacterGood()
		{
			int lLastCharacIndex = currentMorseCode.Length - 1;
			if (lLastCharacIndex >= currentLetterMorseCode.Length) return false;
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
            isCurrentlyWrong = false;
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
			VerifyCurrentCode();
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
			VerifyCurrentCode();
        }

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
			base.Dispose(pDisposing);

            CustomSignals.InputClick -= InputClick;
            CustomSignals.InputStartHold -= InputStartHold;
            CustomSignals.InputStopHold -= InputStopHold;
            CustomSignals.NewCharacter -= NewCharacter;
            CustomSignals.ErrorInCode -= WrongCharacter;
        }
	}
}