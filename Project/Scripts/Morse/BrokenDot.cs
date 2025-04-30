using Godot;
using System;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Morse
{
	public partial class BrokenDot : Control
	{
		// ---------- VARIABLES ---------- \\

		// ----- Paths ----- \\

		// ----- Nodes ----- \\

		// ----- Others ----- \\
		private float speed;

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		protected BrokenDot () : base() { }

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

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
			base.Dispose(pDisposing);
		}
	}
}
