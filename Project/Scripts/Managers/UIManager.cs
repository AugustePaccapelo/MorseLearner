using Com.IsartDigital.OBG.UI.Menus;
using Com.IsartDigital.OBG.UI;
using Godot;
using Com.IsartDigital.OBG.Utils;
using System;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Managers
{
	public partial class UIManager : Manager
	{
        // ---------- VARIABLES ---------- \\

        // ----- Paths ----- \\
        [Export] private PackedScene titleCardScene;
		[Export] private PackedScene mainMenuScene;
		[Export] private PackedScene levelSelectorScene;
		[Export] private PackedScene hudScene;

		// ----- Nodes ----- \\
		[Export] private Control uiContainer;
		[Export] private Control MenusContainer;

		// ----- Others ----- \\
		private Vector2 screenSize;

		// ---------- FUNCTIONS ---------- \\

		// ----- Init & Process ----- \\

        protected override void Init()
        {
            screenSize = GetViewportRect().Size;
			uiContainer.Size = screenSize;
            //Position = screenSize * 0.5f;

			CustomSignals.GoToTitleCard += GoToTitleCard;
			CustomSignals.GoToMainMenu += GoToMainMenu;
			CustomSignals.GoToLevelSelector += GoToLevelSelector;
			CustomSignals.GoToInGame += LaunchGame;   
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);
		}

		// ----- My Functions ----- \\

		private void ClearClhilds(Node pNode)
		{
			foreach (Node lChild in pNode.GetChildren()) lChild.QueueFree();
		}

		private void GoToTitleCard()
		{
            ClearClhilds(MenusContainer);
            MenusContainer.AddChild(titleCardScene.Instantiate());
        }

		private void GoToMainMenu()
		{
            ClearClhilds(MenusContainer);
            MainMenu lMainMenu = mainMenuScene.Instantiate<MainMenu>();
            MenusContainer.AddChild(lMainMenu);
		}

		private void GoToLevelSelector()
		{
			ClearClhilds(MenusContainer);
            LevelSelector lLevelSelector = levelSelectorScene.Instantiate<LevelSelector>();
            MenusContainer.AddChild(lLevelSelector);
		}

		private void LaunchGame(int pDifficulty)
		{
            ClearClhilds(MenusContainer);
            MenusContainer.AddChild(hudScene.Instantiate<HUD>());
			GetManager<LevelManager>().StartGame(pDifficulty);
		}

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
            base.Dispose(pDisposing);

            CustomSignals.GoToTitleCard -= GoToTitleCard;
            CustomSignals.GoToMainMenu -= GoToMainMenu;
            CustomSignals.GoToLevelSelector -= GoToLevelSelector;
            CustomSignals.GoToInGame -= LaunchGame;
        }
	}
}