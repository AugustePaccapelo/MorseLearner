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
		[Export] private Node2D gameContainer;
		private InputManager inputManager;
		public PointLight2D currentLight;
		private Control nodeToFollow;

		// ----- Others ----- \\
		public Vector2 screenSize { get; private set; }
        private float lightRotatingSpeed = 100f;

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
			GD.Print(nodeToFollow);
        }

		public void StopLight()
		{
			CpuParticles2D lParticules = particuleScene.Instantiate<CpuParticles2D>();
			gameContainer.AddChild(lParticules);
			lParticules.Position = nodeToFollow.Position + nodeToFollow.PivotOffset;
			lParticules.Emitting = true;
			lParticules.Finished += () => lParticules.QueueFree();
			currentLight.QueueFree();
			currentLight = null;
			nodeToFollow = null;
		}

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
            base.Dispose(pDisposing);

            CustomSignals.GoToInGame -= (pDifficulty) => PlayPressed();
        }
	}
}
