using Com.IsartDigital.OBG.Managers;
using Com.IsartDigital.OBG.Utils;
using Godot;
using System;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Morse
{
	public partial class Dash : MorseCharacter
	{
        // ---------- VARIABLES ---------- \\

        // ----- Paths ----- \\

        // ----- Nodes ----- \\

        // ----- Others ----- \\
        // Spawning tween parameter
        private Vector2 startScale = Vector2.Zero;
        private Vector2 endScale = new Vector2(1.5f, 1.5f);
        private float spawnDuration = 0.3f;

        private float goodAnimDuration = 0.15f;
        private float brokenStartAnimDuration = 0.25f;

        private float waitTimeForBrokenAnim = 0.25f;
        private float borkenAnimDuration = 0.5f;
        private float fadeDuration = 0.75f;
        private float borkenWaitTime = 0.15f;

        private float brokenStartRotation = 45f;

        // ---------- FUNCTIONS ---------- \\

        // ----- Constructor & Ready & Process ----- \\

        protected Dash () : base() { }

		public override void _Ready()
		{
			base._Ready();
        }

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

        // ----- My Functions ----- \\

        public override void SpawnAnimation()
        {
            Tween lTween = CreateTween();
            lTween.TweenProperty(this, TweenProperties.SCALE, endScale, spawnDuration * 0.5f).From(startScale);
            lTween.Finished += () => CustomSignals.NewCharacter?.Invoke(this);
            lTween.Play();
        }

        public override void GoodAnimation()
        {
            Tween lTween = CreateTween();
            lTween.Chain().TweenProperty(this, TweenProperties.SCALE, Vector2.One, spawnDuration * 0.5f).From(endScale);
            lTween.Play();
        }

        public override void BrokenAnimation()
        {
            Tween lTween = CreateTween();
            float lAngle = Mathf.DegToRad(brokenStartRotation);
            lTween.TweenProperty(leftBroken, TweenProperties.ROTATION, -lAngle, brokenStartAnimDuration);
            lTween.Parallel().TweenProperty(rightBroken, TweenProperties.ROTATION, lAngle, brokenStartAnimDuration);
            lTween.Chain().TweenInterval(waitTimeForBrokenAnim);
            CpuParticles2D lParticule = brokenParticuleScene.Instantiate<CpuParticles2D>();
            AddChild(lParticule);
            lParticule.Scale *= 0.5f;
            lParticule.Emitting = true;
            lTween.Finished += () => CustomSignals.ErrorInCode?.Invoke();
        }

        // ----- Destructor ----- \\

        protected override void Dispose(bool pDisposing)
		{
			base.Dispose(pDisposing);
		}
	}
}
