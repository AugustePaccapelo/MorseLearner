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
		private bool isPressing = false;
		private bool wasPressing = false;
		private float currentPressedTime;
		private bool isHolding = false;

        private float unitTime = 0.15f;
        private const int DOT_UNIT = 1;
        private const int DASH_UNIT = 3;
        private float timeErrorMargin = 0.1f;

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

			if (isPressing)
			{
				currentPressedTime += lDelta;
				if (!isHolding && currentPressedTime > unitTime * DOT_UNIT + timeErrorMargin)
				{
                    signalsManager.EmitSignal(SignalsManager.SignalName.InputStartHold);
					isHolding = true;
                }
				if (!wasPressing) wasPressing = true;
			}
			else if (wasPressing)
			{
				if (currentPressedTime <= unitTime * DOT_UNIT + timeErrorMargin)
				{
					signalsManager.EmitSignal(SignalsManager.SignalName.InputClick);
				}
				else if (isHolding)
				{
                    signalsManager.EmitSignal(SignalsManager.SignalName.InputStopHold);
                    isHolding = false;
                }
				currentPressedTime = 0f;
                wasPressing = false;
			}
		}

        public override void _Input(InputEvent pEvent)
        {
            base._Input(pEvent);

			// Touch managment
			if (!canPlay) return;

			if (IsPressedEvent(pEvent) && !isPressing)
			{
                isPressing = true;
                signalsManager.EmitSignal(SignalsManager.SignalName.InputPressed);
            }
			else if (IsReleasedEvent(pEvent) && isPressing)
			{
                isPressing = false;
                signalsManager.EmitSignal(SignalsManager.SignalName.InputReleased);
            }
        }

        // ----- My Functions ----- \\

		private bool IsPressedEvent(InputEvent pEvent)
		{
			if (pEvent is InputEventScreenTouch lTouch) return lTouch.Pressed;
            if (pEvent is InputEventMouseButton lMouse) return lMouse.Pressed;
            return false;
		}

		private bool IsReleasedEvent(InputEvent pEvent)
		{
            if (pEvent is InputEventScreenTouch lTouch) return !lTouch.Pressed;
            if (pEvent is InputEventMouseButton lMouse) return !lMouse.Pressed;
            return false;
        }

		private bool InputPressed()
		{
			return false;
		}

		private bool InputReleased()
		{
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
