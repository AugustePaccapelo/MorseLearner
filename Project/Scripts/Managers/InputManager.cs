using System.Transactions;
using Com.IsartDigital.OBG.Managers;
using Com.IsartDigital.OBG.UI;
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
		private bool isTouching = false;

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

			if (IsPressedEvent(pEvent) && !isTouching)
			{
                isTouching = true;
                signalsManager.EmitSignal(SignalsManager.SignalName.InputPressed);
            }
			else if (IsReleasedEvent(pEvent) && isTouching)
			{
                isTouching = false;
                signalsManager.EmitSignal(SignalsManager.SignalName.InputReleased);
            }
        }

        // ----- My Functions ----- \\

		private bool IsPressedEvent(InputEvent pEvent)
		{
			if (pEvent is InputEventMouseButton lMouse) return lMouse.Pressed;
			if (pEvent is InputEventScreenTouch lTouch) return lTouch.Pressed;
			return false;
		}

		private bool IsReleasedEvent(InputEvent pEvent)
		{
            if (pEvent is InputEventMouseButton lMouse) return !lMouse.Pressed;
            if (pEvent is InputEventScreenTouch lTouch) return !lTouch.Pressed;
            return false;
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
