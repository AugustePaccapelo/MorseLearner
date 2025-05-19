using System.Runtime.CompilerServices;
using Com.IsartDigital.OBG.Utils;
using Godot;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Managers
{
	public partial class GameManager : Manager
	{
		// ---------- VARIABLES ---------- \\
		
		// ----- Paths ----- \\
		[Export] private PackedScene lightScene;
		[Export] private PackedScene particuleScene;

		// ----- Nodes ----- \\
		[Export] public Node2D gameContainer { get; private set; }
		private InputManager inputManager;
		public PointLight2D currentLight;
		private Control nodeToFollow;

		// ----- Others ----- \\
		public Vector2 screenSize { get; private set; }
        private float lightRotatingSpeed = 100f;

        // ---------- FUNCTIONS ---------- \\

        // ----- Constructor & Ready & Process ----- \\

        protected override void Init()
        {
            screenSize = GetViewportRect().Size;

			allManagersFinishedInits += GetAllManagers;
			CustomSignals.GoToMainMenu += GoToMainMenu;
			CustomSignals.GoToInGame += (_) => ClearGameContainer();
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);

			if (currentLight != null && nodeToFollow != null)
			{
				currentLight.Position = nodeToFollow.Position + nodeToFollow.PivotOffset;
                currentLight.Rotation += Mathf.DegToRad(lightRotatingSpeed) * lDelta;
            }
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

		public void StartLight(Control pNodeToFollow)
		{
			if (currentLight != null) return;
			currentLight = lightScene.Instantiate<PointLight2D>();
			gameContainer.AddChild(currentLight);
			nodeToFollow = pNodeToFollow;
			currentLight.Position = nodeToFollow.Position + nodeToFollow.PivotOffset;
        }

		public void StopLight()
		{
			if (nodeToFollow != null)
			{
				CpuParticles2D lParticules = particuleScene.Instantiate<CpuParticles2D>();
				gameContainer.AddChild(lParticules);
				lParticules.Position = nodeToFollow.Position + nodeToFollow.PivotOffset;
				lParticules.Emitting = true;
				lParticules.Finished += () => lParticules.QueueFree();
			}
			currentLight?.QueueFree();
			currentLight = null;
			nodeToFollow = null;
		}

		private void GoToMainMenu()
		{
			GetManager<InputManager>().canPlay = false;
			ClearGameContainer();
		}

		private void ClearGameContainer()
		{
			for (int i = gameContainer.GetChildCount() - 1; i > -1; i--)
			{
				//if (gameContainer.GetChild(i) is CpuParticles2D lPart) lPart.Emitting = false;
				gameContainer.GetChild(i).QueueFree();
			}
		}

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
            base.Dispose(pDisposing);

            CustomSignals.GoToInGame -= (pDifficulty) => PlayPressed();
            CustomSignals.GoToMainMenu -= GoToMainMenu;
        }
	}
}
