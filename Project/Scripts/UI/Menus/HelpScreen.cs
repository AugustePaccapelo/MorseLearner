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
		[Export] private PackedScene particulesScene;
		private const string PATH_LABEL = "Letter";

		// ----- Nodes ----- \\
		[Export] private Button playButton;
		[Export] private VBoxContainer vBoxCont;
		[Export] private TextureRect handDot, handDash;
		private List<HBoxContainer> allHBoxContainers = new List<HBoxContainer>();
		private CpuParticles2D dotParticules, dashParticules;

		// ----- Others ----- \\
		public int numLettersKnown = 1;
		public int numLetterToShow;
		private List<string> lettersToShow;

		private float dotPressTime;
		private float dashPressTime;
		private float handAnimationDuration = 0.5f;
		private float startRotation;
		private float addedRotation = 60f;
		private float rotationSpeed;
		private float waitTimeToLoop = 1f;

		private bool isDoingDot;
		private bool isDoingDash;
		private float elapseTime;
		private bool isDotPressing;
		private bool isDashPressing;
		private bool isDotGoingBack;
        private bool isDashGoingBack;

        private enum HandState { Idle, Rotating, Emitting, Returning }

        private HandState dotState = HandState.Idle;
        private HandState dashState = HandState.Idle;

        private float dotAnimTimer = 0;
        private float dashAnimTimer = 0;


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

			dotPressTime = InputManager.unitTime * InputManager.DOT_UNIT + InputManager.timeErrorMargin;
			dashPressTime = InputManager.unitTime * InputManager.DASH_UNIT + InputManager.timeErrorMargin;
            dashPressTime *= 1.5f;
            startRotation = handDot.Rotation;
			rotationSpeed = Mathf.DegToRad(addedRotation) / handAnimationDuration;
		}

        public override void _Process(double pDelta)
        {
            float delta = (float)pDelta;
            base._Process(pDelta);

            // Start new loop only if both are Idle
            if (dotState == HandState.Idle && dashState == HandState.Idle)
            {
                elapseTime += delta;

                if (elapseTime >= waitTimeToLoop)
                {
                    elapseTime = 0;
                    StartDot();
                    StartDash();
                }
            }

            UpdateDot(delta);
            UpdateDash(delta);
        }


        // ----- My Functions ----- \\

        private void StartDot()
        {
            dotState = HandState.Rotating;
            dotAnimTimer = 0;

            if (dotParticules is null)
                dotParticules = particulesScene.Instantiate<CpuParticles2D>();

            dotParticules.OneShot = false;
            dotParticules.Emitting = false;
            dotParticules.Amount = 20;
            dotParticules.Explosiveness = 0;
        }

        private void StartDash()
        {
            dashState = HandState.Rotating;
            dashAnimTimer = 0;

            if (dashParticules is null)
                dashParticules = particulesScene.Instantiate<CpuParticles2D>();

            dashParticules.OneShot = false;
            dashParticules.Emitting = false;
            dashParticules.Amount = 20;
            dashParticules.Explosiveness = 0;
        }

        private void UpdateDot(float delta)
        {
            switch (dotState)
            {
                case HandState.Rotating:
                    handDot.Rotation += rotationSpeed * delta;
                    dotAnimTimer += delta;

                    if (dotAnimTimer >= handAnimationDuration)
                    {
                        handDot.Rotation = startRotation + Mathf.DegToRad(addedRotation);
                        dotParticules.Emitting = true;
                        dotState = HandState.Emitting;
                        Manager.GetManager<GameManager>().gameContainer.AddChild(dotParticules);
                        dotParticules.Position = handDot.GetChild<Control>(0).GlobalPosition;
                        dotAnimTimer = 0;
                    }
                    break;

                case HandState.Emitting:
                    dotAnimTimer += delta;

                    if (dotAnimTimer >= dotPressTime)
                    {
                        dotParticules.Emitting = false;
                        dotParticules.QueueFree();
                        dotParticules = null;

                        dotState = HandState.Returning;
                        dotAnimTimer = 0;
                    }
                    break;

                case HandState.Returning:
                    handDot.Rotation -= rotationSpeed * delta;
                    dotAnimTimer += delta;

                    if (dotAnimTimer >= handAnimationDuration)
                    {
                        handDot.Rotation = startRotation;
                        dotState = HandState.Idle;
                        dotAnimTimer = 0;
                    }
                    break;
            }
        }

        private void UpdateDash(float delta)
        {
            switch (dashState)
            {
                case HandState.Rotating:
                    handDash.Rotation += rotationSpeed * delta;
                    dashAnimTimer += delta;

                    if (dashAnimTimer >= handAnimationDuration)
                    {
                        handDash.Rotation = startRotation + Mathf.DegToRad(addedRotation);
                        dashParticules.Emitting = true;
                        dashState = HandState.Emitting;
                        Manager.GetManager<GameManager>().gameContainer.AddChild(dashParticules);
                        dashParticules.Position = handDash.GetChild<Control>(0).GlobalPosition;
                        dashAnimTimer = 0;
                    }
                    break;

                case HandState.Emitting:
                    dashAnimTimer += delta;

                    if (dashAnimTimer >= dashPressTime)
                    {
                        dashParticules.Emitting = false;
                        dashParticules.QueueFree();
                        dashParticules = null;

                        dashState = HandState.Returning;
                        dashAnimTimer = 0;
                    }
                    break;

                case HandState.Returning:
                    handDash.Rotation -= rotationSpeed * delta;
                    dashAnimTimer += delta;

                    if (dashAnimTimer >= handAnimationDuration)
                    {
                        handDash.Rotation = startRotation;
                        dashState = HandState.Idle;
                        dashAnimTimer = 0;
                    }
                    break;
            }
        }

        private void PlayButtonPressed()
		{
			CustomSignals.GoToInGame?.Invoke(0);
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
