using Godot;
using System;
using System.Runtime.CompilerServices;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Morse
{
	public abstract partial class MorseCharacter : Node2D
	{
		// ---------- VARIABLES ---------- \\

		// ----- Paths ----- \\
		[Export] protected PackedScene particuleScene;
		[Export] protected PackedScene brokenParticuleScene;

        // ----- Nodes ----- \\
        [Export] public Sprite2D defaultSprite { get; private set; }
        [Export] public Node2D renderer { get; private set; }
		[Export] protected Node2D goodVisual;
		[Export] protected Node2D brokenVisual;
        [Export] protected Sprite2D leftBroken, rightBroken;

        // ----- Others ----- \\
        public Vector2 TextureSize => defaultSprite.Texture.GetSize() * renderer.Scale;

        // ---------- FUNCTIONS ---------- \\

        // ----- Constructor & Ready & Process ----- \\

        protected MorseCharacter () : base() { }

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

		public abstract void SpawnAnimation();
        public abstract void GoodAnimation();
        public abstract void BrokenAnimation();

		public void SetBroken()
		{
			goodVisual.Hide();
			brokenVisual.Show();
		}

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
			base.Dispose(pDisposing);
		}
	}
}
