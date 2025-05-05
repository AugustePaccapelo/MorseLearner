using Godot;
using System;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Morse
{
	public abstract partial class MorseCharacter : Control
	{
		// ---------- VARIABLES ---------- \\

		// ----- Paths ----- \\

		// ----- Nodes ----- \\

		// ----- Others ----- \\

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
		public abstract void BrokenAnimation();

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
			base.Dispose(pDisposing);
		}
	}
}
