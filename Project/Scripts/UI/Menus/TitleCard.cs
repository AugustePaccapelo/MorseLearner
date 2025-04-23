using Godot;
using Com.IsartDigital.OBG.Utils;
using Com.IsartDigital.OBG.Managers;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.UI.Menus
{
	public partial class TitleCard : Control
	{
        // ---------- VARIABLES ---------- \\

        #region // ----- Singleton ----- \\
        static private TitleCard instance;

        static public TitleCard GetInstance()
        {
            if (instance == null) instance = new TitleCard();
            return instance;

        }
        #endregion


        // ----- Paths ----- \\

        // ----- Nodes ----- \\
        [Export] private TextureRect isartLogo;

		// ----- Others ----- \\
		private float isartLogoVisibleDuration = 2f;
		private Color isartLogoStartColor = Colors.Transparent;
		private Color isartLogoEndColor = Colors.White;

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		private TitleCard() : base() { }

		public override void _Ready()
		{
            #region Singleton Ready
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(TitleCard) + " Instance already exist, destroying the last added.");
                return;
            }

            instance = this;
            #endregion


            base._Ready();

			float lIsartTweenDuration = isartLogoVisibleDuration * 0.5f;
			Tween lTween = CreateTween();
			lTween.TweenProperty(isartLogo, TweenProperties.MODULATE, isartLogoEndColor, lIsartTweenDuration).From(isartLogoStartColor);
			lTween.Chain().TweenProperty(isartLogo, TweenProperties.MODULATE, isartLogoStartColor, lIsartTweenDuration);
			lTween.Finished += AnimationFinished;
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		private void AnimationFinished()
		{
			SignalsManager.GetInstance().EmitSignal(SignalsManager.SignalName.GoToMainMenu);
			QueueFree();
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
