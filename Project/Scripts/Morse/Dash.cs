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
            
        }

        public override void BrokenAnimation()
        {
            
        }

        // ----- Destructor ----- \\

        protected override void Dispose(bool pDisposing)
		{
			base.Dispose(pDisposing);
		}
	}
}
