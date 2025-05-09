using System;
using Com.IsartDigital.OBG.Morse;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Utils
{
	public static class CustomSignals
	{
		// UI Signals
        public static Action GoToTitleCard;
        public static Action GoToMainMenu;
		public static Action GoToLevelSelector;
		public static Action<int> GoToInGame;

		// Input signals
		public static Action InputPressed;
		public static Action InputReleased;
		public static Action InputClick;
		public static Action InputStartHold;
		public static Action InputStopHold;

		// In game signals
		public static Action NewLetter;
		public static Action ErrorInCode;
		public static Action<MorseCharacter> NewCharacter;
    }
}