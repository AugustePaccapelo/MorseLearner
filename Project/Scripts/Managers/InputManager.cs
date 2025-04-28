using Com.IsartDigital.OBG.Managers;
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
		private SignalsManager signalsManager;

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

			signalsManager = SignalsManager.GetInstance();
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

        public override void _Input(InputEvent pEvent)
        {
            base._Input(pEvent);

			// Touch managment
			if (!canPlay) return;

			// Input for screen
			if (pEvent is InputEventScreenTouch lEventTouch)
			{

			}

			// Input for mouse
			if (pEvent is InputEventMouseButton lEventMouse)
			{
				if (lEventMouse.IsReleased())
				{
					signalsManager.EmitSignal(SignalsManager.SignalName.InputReleased);
                }
				else
				{
                    signalsManager.EmitSignal(SignalsManager.SignalName.InputPressed);
                }
			}
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
