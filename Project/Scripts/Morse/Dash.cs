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
            lTween.Finished += () => SignalsManager.GetInstance().EmitSignal(SignalsManager.SignalName.NewCharacter, this);
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
            GetTree().CreateTimer(0.2).Timeout += () => SignalsManager.GetInstance().EmitSignal(SignalsManager.SignalName.WrongCharacter);
        }

        // ----- Destructor ----- \\

        protected override void Dispose(bool pDisposing)
		{
			base.Dispose(pDisposing);
		}
	}
}
