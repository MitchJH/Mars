﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Mars
{
    /// <summary>
    /// Languages available in the game
    /// </summary>
    public enum Languages
    {
        English
    }

    /// <summary>
    /// Usage example: Localization.Get("text_key");
    /// </summary>
    public static class Localization
    {
        private const string MISSING_STRING = "**MISSING STRING**";

        private static Languages _language = Languages.English;
        private static Dictionary<string, string> _textValues;

        static Localization()
        {
            _textValues = new Dictionary<string, string>();
        }

        /// <summary>
        /// Load the localization file of the selected language
        /// </summary>
        public static void Load()
        {
            using (var reader = new StreamReader(TitleContainer.OpenStream("Content/Localization/" + _language.ToString() + ".txt"), Encoding.Default, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#") == false && string.IsNullOrEmpty(line) == false)
                    {
                        string[] split = line.Split(',');
                        string key = split[0].ToLower();
                        string text = split[1];
                        _textValues.Add(key, text);
                    }
                }
            }
        }

        /// <summary>
        /// Get a localized text value from a key
        /// </summary>
        /// <param name="key">The key of the text value</param>
        /// <returns>A localized string</returns>
        public static string Get(string key)
        {
            string keyL = key.ToLower();
            if (_textValues.ContainsKey(keyL))
            {
                return _textValues[keyL];
            }
            else
            {
                return MISSING_STRING;
            }
        }

        /// <summary>
        /// The currently loaded language the game is using
        /// </summary>
        public static Languages Language
        {
            get
            {
                return _language;
            }
            set
            {
                if (value != _language)
                {
                    // Only load the loc if it's a different language being requested compared to the current
                    _language = value;
                    Load();
                }
            }
        }
    }
}
