using Com.IsartDigital.OBG.Utils;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Managers
{
	public partial class GameManager : Manager
	{
		// ---------- VARIABLES ---------- \\

		// ----- Paths ----- \\

		// ----- Nodes ----- \\
		[Export] private Node2D gameContainer;
		private InputManager inputManager;

		// ----- Others ----- \\
		public Vector2 screenSize { get; private set; }

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Process ----- \\

        public override void Init()
        {
            screenSize = GetViewportRect().Size;

			allManagersFinished += GetAllManagers;
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		private void GetAllManagers()
		{
			CustomSignals.GoToInGame += (pDifficulty) => PlayPressed();

			inputManager = GetManager<InputManager>();

            CustomSignals.GoToTitleCard?.Invoke();
        }

		private void PlayPressed()
		{
			inputManager.canPlay = true;
		}

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
            base.Dispose(pDisposing);

            CustomSignals.GoToInGame -= (pDifficulty) => PlayPressed();
        }
	}
}
