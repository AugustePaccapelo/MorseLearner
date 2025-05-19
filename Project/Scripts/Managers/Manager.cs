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
		public static Action allManagersFinishedInits;
		public static bool isInitsFinished { get; private set; } = false;
		public Type thisType { get; private set; } = null;

		// ---------- FUNCTIONS ---------- \\

		// ----- Constructor & Ready & Init ----- \\

		protected Manager() : base()
		{
			// Count how many Managers instances have been created
			numManager++;
		}

		public override sealed void _Ready()
		{
            base._Ready();
            thisType = GetType();
			
			// Prevent duplicate manager of the same type
			if (allManagers.ContainsKey(thisType))
			{
				GD.Print("This manager : ", thisType, " already exist, destroying last added.");
                numManager--;
                QueueFree();
				return;
			}
			
			allManagers.Add(thisType, this);
			if (IsAllManagersReady()) InitAllManagers();
		}

		protected abstract void Init();

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
			if (!isInitsFinished)
			{
				foreach (Manager lManager in allManagers.Values) lManager.Init();
				isInitsFinished = true;
				allManagersFinishedInits?.Invoke();
				return;
			}
			Init();
		}

        // ----- Destructor ----- \\

        protected override void Dispose(bool pDisposing)
		{
            if (allManagers[thisType] == this)
			{
                numManager--;
				allManagers.Remove(thisType);
			}
            base.Dispose(pDisposing);
        }
	}
}