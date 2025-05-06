using Godot;
using System;
using System.Collections.Generic;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Utils
{
	public static class MorseCode
	{
		public const char DOT_CHARAC = '.';
		public const char DASH_CHARAC = '-';

		public static Dictionary<String, String> alphabet = new Dictionary<string, string>()
		{
			{"A", ".-" }, {"B", "-..." }, {"C", "-.-." }, {"D", "-.." }, {"E", "."},
            {"F", "..-." }, {"G", "--." }, {"H", "...." }, {"I", ".." }, {"J", ".---"},
            {"K", "-.-" }, {"L", ".-.." }, {"M", "--" }, {"N", "-." }, {"O", "---"},
            {"P", ".--." }, {"Q", "--.-" }, {"R", ".-." }, {"S", "..." }, {"T", "-"},
            {"U", "..-" }, {"V", "...-" }, {"W", ".--" }, {"X", "-..-" }, {"Y", "-.--"},
            {"Z", "--.." }, {"0", "-----" }, {"1", ".----" }, {"2", "..---" }, {"3", "...--" },
			{"4", "....-"}, {"5", "....." }, {"6", "-...." }, {"7", "--..." }, {"8", "---.." },
			{"9", "----."}
        };
	}
}
