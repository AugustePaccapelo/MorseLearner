using Godot;
using System;
using System.Collections.Generic;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Utils
{
	public static class MorseCode
	{
		public const string DOT_CHARAC = ".";
		public const string DASH_CHARAC = "-";
		public const string LITTLE_SPACE_CHARAC = "";
		public const string LETTER_SPACE_CHARAC = " ";
		public const string WORD_SPACE_CHARAC = "   ";

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
