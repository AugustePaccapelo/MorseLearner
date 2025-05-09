using Godot;
using System;
using System.Collections.Generic;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.OBG.Managers
{
	public abstract partial class Manager : Node2D
	{
		// ---------- VARIABLES ---------- \\

		// ----- Managers ----- \\
		public static int numManager { get; private set; } = 0;
		private static Dictionary<Type, Manager> allManagers = new Dictionary<Type, Manager>();
		protected static Action allManagersFinished;

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Init ----- \\

		protected Manager() : base()
		{
			// Count how many Managers instances have been created
			numManager++;
		}

		public override void _Ready()
		{
            base._Ready();
            Type lType = GetType();

			// Prevent duplicate manager of the same type
			if (allManagers.ContainsKey(lType))
			{
				GD.Print("This manager : ", lType, " already exist, destroying last added.");
				QueueFree();
				return;
			}

			allManagers.Add(lType, this);
			if (IsAllManagersReady()) InitAllManagers();
		}

		public abstract void Init();

		// ----- My Functions ----- \\

		/// <summary>
		/// Retrieves the instance of the specified manager type, if it exist.
		/// </summary>
		/// <typeparam name="T">The manager type to retrieve</typeparam>
		/// <returns>The manager instance, or null if not found</returns>
		public static T GetManager<T>() where T : Manager
		{
			Type lManagerType = typeof(T);
			if (allManagers.ContainsKey(lManagerType))
				return (T)allManagers[lManagerType];
			
			GD.Print("Manager of type : ", lManagerType, " not found.");
			return null;
		}

		private bool IsAllManagersReady() => numManager == allManagers.Count;
		private void InitAllManagers()
		{
			foreach (Manager lManager in allManagers.Values) lManager.Init();
			allManagersFinished?.Invoke();
		}

		// ----- Destructor ----- \\

		protected override void Dispose(bool pDisposing)
		{
			numManager--;
			allManagers.Remove(GetType());
			base.Dispose(pDisposing);
		}
	}
}
