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
			{"a", ".-" }, {"b", "-..." }, {"c", "-.-." }, {"d", "-.." }, {"e", "."},
            {"f", "..-." }, {"g", "--." }, {"h", "...." }, {"i", ".." }, {"j", ".---"},
            {"k", "-.-" }, {"l", ".-.." }, {"m", "--" }, {"n", "-." }, {"o", "---"},
            {"p", ".--." }, {"q", "--.-" }, {"r", ".-." }, {"s", "..." }, {"t", "-"},
            {"u", "..-" }, {"v", "...-" }, {"w", ".--" }, {"x", "-..-" }, {"y", "-.--"},
            {"z", "--.." }, {"0", "-----" }, {"1", ".----" }, {"2", "..---" }, {"3", "...--" },
			{"4", "....-"}, {"5", "....." }, {"6", "-...." }, {"7", "--..." }, {"8", "---.." },
			{"9", "----."}
        };
	}
}
