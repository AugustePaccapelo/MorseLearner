using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Managers
{
	public partial class SignalsManager : Node
	{
		// ---------- VARIABLES ---------- \\

		#region // ----- Singleton ----- \\
		static private SignalsManager instance;

		static public SignalsManager GetInstance()
		{
			if (instance == null) instance = new SignalsManager();
			return instance;

		}
		#endregion

		// ----- Paths ----- \\

		// ----- Nodes ----- \\

		// ----- Others ----- \\
		[Signal] public delegate void GoToMainMenuEventHandler();

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		private SignalsManager() : base() { }

		public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(SignalsManager) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

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
			#region Singleton Dispose
			if (pDisposing && instance == this) instance = null;
			#endregion

			base.Dispose(pDisposing);
		}
	}
}
