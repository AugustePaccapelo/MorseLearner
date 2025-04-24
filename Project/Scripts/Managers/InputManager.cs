using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.managers
{
	public partial class InputManager : Node2D
	{
		// ---------- VARIABLES ---------- \\

		#region // ----- Singleton ----- \\
		static private InputManager instance;

		static public InputManager GetInstance()
		{
			if (instance == null) instance = new InputManager();
			return instance;

		}
		#endregion

		// ----- Paths ----- \\

		// ----- Nodes ----- \\

		// ----- Others ----- \\
		public bool canPlay = false;

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

		private InputManager() : base() { }

		public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(InputManager) + " Instance already exist, destroying the last added.");
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

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);

			// Touch managment
			if (!canPlay) return;
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
