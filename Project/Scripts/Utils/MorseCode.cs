using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.OBG.Utils
{
	public static class MorseCode
	{
		public const char DOT_CHARAC = '.';
		public const char DASH_CHARAC = '-';
		public const string SPACE_LETTER = " ";
		public const string SPACE_WORD = "  ";

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

		public static List<String> militaryAlphabet = new List<string>()
		{
			"ALPHA", "BRAVO", "CHARLIE", "DELTA", "ECHO", "FOXTROT", "GOLF", "HOTEL",
			"INDIA", "JULIETT", "KILO", "LIMA", "MIKE", "NOVEMBER", "OSCAR", "PAPA",
			"QUEBEC", "ROMEO", "SIERRA", "TANGO", "UNIFORM", "VICTOR", "WHISKEY", "XRAY",
			"YANKEE", "ZULU"
		};

		public static string TextToMorse(string pMessage)
		{
			List<string> lAllLetters = alphabet.Keys.ToList();
			string lMorse = "";

			foreach (char lCharacChar in pMessage)
			{
				string lCharacString = lCharacChar.ToString();
				if (lAllLetters.Contains(lCharacString))
				{
					lMorse += alphabet[lCharacString];
				}
				else
				{
					lMorse += SPACE_WORD;
				}
				lMorse += SPACE_LETTER;
			}
			return lMorse;
		}
	}
}
