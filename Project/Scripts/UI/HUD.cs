using Com.IsartDigital.OBG.Managers;
using Com.IsartDigital.OBG.UI.Menus;
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
        [Export] private PackedScene dotTextureScene, dashTextureScene;
        private const string PATH_MENU_BUTTON = "Button";

		// ----- Nodes ----- \\
		[Export] private Button mainMenuButton;
		[Export] private Label currentLetterLabel, secondLetterLabel, thirdLetterLabel;
		[Export] private Label confirmationLabel;
        [Export] public Control startMorseCodePos { get; private set; }
		[Export] private Control unkownPos;

		// ----- Others ----- \\
		private string wrongMorseCodeConfirmation = "Sorry !";

		// ----- Animations ----- \\
		// Finish
		private bool isLetterTurning = false;
		private float finishAnimDuration = 0.75f;
		private float letterFinishStartScale = 1f;
		private float letterFinishMaxScale = 1.75f;
		private float finishEndAnimationScaleMult = 1.25f;
		private float finishEndAnimationDuration = 0.4f;
		[Export] private Control letterFinishAnimPos;

		private float numLetterTurn = 2;
		private float turnTotalAngle;
		private float animFinishTurnDuration;
		private float animFinishCurrentXMult;
		private float animElapseTime;

		// Spawn
		private float transitionDuration = 1f;
		private float transitionDelay = 0.1f;
		private float thirdLetterSpawnDuration = 0.4f;
		private Vector2 currentLetterPos, secondLetterPos, thirdLetterPos;
		private Vector2 waitingLettersBaseScale;

		private bool isAnimationRunning = false;
		private HBoxContainer unkownCont;

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

			if (mainMenuButton is null) mainMenuButton = GetNode<Button>(PATH_MENU_BUTTON);
			
			mainMenuButton.Pressed += MainMenuPressed;
			
            currentLetterPos = currentLetterLabel.Position;
			secondLetterPos = secondLetterLabel.Position;
			thirdLetterPos = thirdLetterLabel.Position;
			waitingLettersBaseScale = secondLetterLabel.Scale;
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(pDelta);

			if (isLetterTurning)
			{
				animElapseTime += lDelta;
 
				float lProgress = animElapseTime / finishAnimDuration;

				float lScaleFac = Mathf.Lerp(letterFinishStartScale, letterFinishMaxScale, lProgress);

				float lXScaleMult = Mathf.Cos(turnTotalAngle * lProgress);

				Vector2 lScale = new Vector2(lScaleFac * lXScaleMult, lScaleFac);
				currentLetterLabel.Scale = lScale;
				if (animElapseTime >= finishAnimDuration)
				{
					isLetterTurning = false;
				}
			}
		}

		// ----- My Functions ----- \\

		private void MainMenuPressed()
		{
			if (isAnimationRunning) return;
            mainMenuButton.Pressed -= MainMenuPressed;
            CustomSignals.GoToMainMenu?.Invoke();
		}

		public void UnknownLetterAnim()
		{
			unkownCont = new HBoxContainer();
			AddChild(unkownCont);
            unkownCont.Position = unkownPos.Position;
			thirdLetterLabel.Reparent(unkownCont);
			AddMorseCode(thirdLetterLabel.Text, unkownCont);
			Tween lTween = CreateTween();
			lTween.TweenProperty(unkownCont, TweenProperties.POSITION, thirdLetterPos, 2f);
			lTween.Chain().TweenProperty(thirdLetterLabel, TweenProperties.SCALE, waitingLettersBaseScale, thirdLetterSpawnDuration); 
			lTween.Finished += () =>
			{
				thirdLetterLabel.Reparent(this);
				thirdLetterLabel.Position = thirdLetterPos;
				unkownCont.QueueFree();
				unkownCont = null;

				AnimationFinished();
				
			};
			lTween.Play();
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

        public void NewLetterAnimation()
		{
			isAnimationRunning = true;
            Manager.GetManager<GameManager>().StopLight();
            currentLetterLabel.Text = "";

			Tween lTween = CreateTween();

			lTween.TweenProperty(secondLetterLabel, TweenProperties.POSITION, currentLetterPos, transitionDuration)
				.SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut);
            lTween.Parallel().TweenProperty(secondLetterLabel, TweenProperties.SCALE, Vector2.One, transitionDuration)
                .SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut);

			Tween lSecondTween = CreateTween();
            lSecondTween.TweenInterval(transitionDelay);

            lSecondTween.TweenProperty(thirdLetterLabel, TweenProperties.POSITION, secondLetterPos, transitionDuration)
                .SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut);

            lSecondTween.Finished += UpdateLetters;

			lSecondTween.Play();
            lTween.Play();
		}

		public void LetterFinishedAnimation()
		{
			isAnimationRunning = true;

            Manager.GetManager<GameManager>().StartLight(currentLetterLabel);

			Tween lTween = CreateTween();

			lTween.TweenProperty(currentLetterLabel, TweenProperties.GLOBAL_POSITION, letterFinishAnimPos.GlobalPosition, finishAnimDuration);
            StartTurnAnimation();

			Vector2 lBaseScale = Vector2.One * letterFinishMaxScale;
			Vector2 lTargetScale = lBaseScale * finishEndAnimationScaleMult;
			float lTime = finishEndAnimationDuration * 0.5f;

            lTween.Chain().TweenProperty(currentLetterLabel, TweenProperties.SCALE, lTargetScale, lTime).From(lBaseScale);
			lTween.Chain().TweenProperty(currentLetterLabel, TweenProperties.SCALE, Vector2.Zero, lTime).From(lBaseScale);

            lTween.Finished += NewLetterAnimation;
            lTween.Play();
        }

		private void StartTurnAnimation()
		{
			isAnimationRunning = true;
            isLetterTurning = true;
			animFinishCurrentXMult = 0;
            animElapseTime = 0;
			animFinishTurnDuration = finishAnimDuration / numLetterTurn;
            turnTotalAngle = numLetterTurn * Mathf.Pi * 2;
        }
		
		private void UpdateLetters()
		{
			isAnimationRunning = true;

            Manager.GetManager<LevelManager>().NewLetter();

            currentLetterLabel.Position = currentLetterPos;
			secondLetterLabel.Position = secondLetterPos;
			thirdLetterLabel.Position = thirdLetterPos;
			thirdLetterLabel.Scale = Vector2.Zero;

			currentLetterLabel.Scale = Vector2.One;
			secondLetterLabel.Scale = waitingLettersBaseScale;

			if (LevelManager.streak >= 3)
			{
                LevelSelector.numLettersKnown++;
				LevelManager.streak = 0;
				UnknownLetterAnim();
            }
			else
			{
				Tween lTween = CreateTween();

				lTween.TweenProperty(thirdLetterLabel, TweenProperties.SCALE, waitingLettersBaseScale, thirdLetterSpawnDuration)
					.SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out);

				lTween.Finished += AnimationFinished;

				lTween.Play();
			}
		}

		private void AnimationFinished()
		{
			isAnimationRunning = false;
            Manager.GetManager<InputManager>().canPlay = true;
			Manager.GetManager<LevelManager>().ClearMorseCode();
			Manager.GetManager<LevelManager>().letterFinished = false;
        }

		public void UpdateLetter(string pLetter, string pSecondLetter, string pThirdLetter)
		{
			currentLetterLabel.Text = pLetter;
			secondLetterLabel.Text = pSecondLetter;
            thirdLetterLabel.Text = pThirdLetter;
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
