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
		// Menus Buttons
		[Signal] public delegate void GoToMainMenuEventHandler();
		[Signal] public delegate void GoToLevelSelectorEventHandler();
		[Signal] public delegate void PlayButtonPressedEventHandler(int pDifficulty);

		// Inputs Signals
		[Signal] public delegate void InputPressedEventHandler();
		[Signal] public delegate void InputReleasedEventHandler();
		[Signal] public delegate void InputClickEventHandler();
        [Signal] public delegate void InputStartHoldEventHandler();
        [Signal] public delegate void InputStopHoldEventHandler();

        [Signal] public delegate void LetterFinishedEventHandler();
        [Signal] public delegate void WrongCodeEventHandler();
        [Signal] public delegate void WordFinishedEventHandler();

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
