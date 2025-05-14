using Com.IsartDigital.OBG.UI;
using Com.IsartDigital.OBG.Utils;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Managers
{
	public partial class InputManager : Manager
	{
		// ---------- VARIABLES ---------- \\

		// ----- Paths ----- \\

		// ----- Nodes ----- \\

		// ----- Others ----- \\
		public bool canPlay = false;
		private bool isPressing = false;
		private bool wasPressing = false;
		private float currentPressedTime;
		private bool isHolding = false;

        private float unitTime = 0.1f;
        private const int DOT_UNIT = 1;
        private const int DASH_UNIT = 3;
        private float timeErrorMargin = 0.1f;

        // ---------- FUNCTIONS ---------- \\

        // ----- Init & Process ----- \\

        public override void Init()
        {
            
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
					CustomSignals.InputStartHold?.Invoke();
					isHolding = true;
                }
				if (!wasPressing) wasPressing = true;
			}
			else if (wasPressing)
			{
				if (currentPressedTime <= unitTime * DOT_UNIT + timeErrorMargin)
				{
                    CustomSignals.InputClick?.Invoke();
				}
				else if (isHolding)
				{
                    CustomSignals.InputStopHold?.Invoke();
                    isHolding = false;
                }
				currentPressedTime = 0f;
                wasPressing = false;
			}
		}
        
        public override void _UnhandledInput(InputEvent pEvent)
        {
            base._UnhandledInput(pEvent);

			// Touch managment
			if (!canPlay) return;
			if (GetTree().Root.GuiGetFocusOwner() != null) return;

			if (IsPressedEvent(pEvent) && !isPressing)
			{
                isPressing = true;
                CustomSignals.InputPressed?.Invoke();
            }
			else if (IsReleasedEvent(pEvent) && isPressing)
			{
                isPressing = false;
                CustomSignals.InputReleased?.Invoke();
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
			base.Dispose(pDisposing);
		}
	}
}