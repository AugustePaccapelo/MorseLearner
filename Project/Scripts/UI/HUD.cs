using System.Threading;
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

        // ----- Nodes ----- \\
		[Export] private Label currentLetterLabel, secondLetterLabel, thirdLetterLabel;
		[Export] private Label confirmationLabel;
        [Export] public Control startMorseCodePos { get; private set; }

		// ----- Others ----- \\
		private string wrongMorseCodeConfirmation = "Sorry !";

		// ----- Animations ----- \\
		// Finish
		private bool isLetterTurning = false;
		private float finishAnimDuration = 2f;
		private float letterFinishStartScale = 1;
		private float letterFinishMaxScale = 1.75f;
		[Export] private Control letterFinishAnimPos;

		private int numLetterTurn = 4;
		private float animFinishTurnDuration;
		private float animFinishCurrentXMult;
		private float animElapseTime;

        // Spawn

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

			base._Process(pDelta);

			if (isLetterTurning)
			{
				animElapseTime += lDelta;

				float lProgress = animElapseTime / animFinishTurnDuration;

				float lScaleFac = Mathf.Lerp(letterFinishStartScale, letterFinishMaxScale, lProgress / finishAnimDuration);

				float lXScaleMult = Mathf.Cos(lProgress * Mathf.Pi);
				Vector2 lScale = new Vector2(lScaleFac * lXScaleMult, lScaleFac);
				currentLetterLabel.Scale = lScale;
				GD.Print($"Time : {animElapseTime}, Progress : {lProgress}, lWeight : {lProgress % finishAnimDuration}, Scale : {lScaleFac}");
				if (animElapseTime >= finishAnimDuration)
				{
					GD.Print("Finished");
					isLetterTurning = false;
				}
			}
		}

		// ----- My Functions ----- \\

		public void NewLetterAnimation()
		{

		}

		public void LetterFinishedAnimation()
		{
			Tween lTween = CreateTween();

			lTween.TweenProperty(currentLetterLabel, TweenProperties.GLOBAL_POSITION, letterFinishAnimPos.GlobalPosition, finishAnimDuration);

			StartTurnAnimation();

			//lTween.Finished += UpdateLetters;
		}

		private void StartTurnAnimation()
		{
            isLetterTurning = true;
			animFinishCurrentXMult = 0;
            animElapseTime = 0;
			animFinishTurnDuration = finishAnimDuration / numLetterTurn;
			GD.Print(animFinishTurnDuration);
        }
		
		private void UpdateLetters()
		{
			currentLetterLabel.Text = secondLetterLabel.Text;
			secondLetterLabel.Text = thirdLetterLabel.Text;
		}

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
