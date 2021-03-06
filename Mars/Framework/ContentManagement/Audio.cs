﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Mars
{
    public static class Audio
    {
        private static SoundEffect _MISSING_AUDIO;

        private static Dictionary<string, SoundEffect> _sounds;
        private static Dictionary<string, Song> _music;

        static Audio()
        {
            _sounds = new Dictionary<string, SoundEffect>();
            _music = new Dictionary<string, Song>();
        }

        public static void LoadSound(string[] data, ContentManager content)
        {
            string id = data[1];
            string filepath = data[2];

            SoundEffect newSound = content.Load<SoundEffect>(filepath);
            _sounds.Add(id, newSound);
        }

        public static void LoadMusic(string[] data, ContentManager content)
        {
            string id = data[1];
            string filepath = data[2];

            Song newSong = content.Load<Song>(filepath);
            _music.Add(id, newSong);
        }

        /// <summary>
        /// Plays a sound effect.
        /// </summary>
        /// <param name="key">The key of the sound effect</param>
        /// <returns>True if sound was found and played</returns>
        public static bool PlaySoundEffect(string key)
        {
            string keyL = key.ToLower();

            if (Settings.EffectsOn == false) return false;

            if (string.IsNullOrEmpty(keyL) == false)
            {
                if (_sounds.ContainsKey(keyL))
                {
                    SoundEffectInstance sei = _sounds[keyL].CreateInstance();
                    sei.Volume = ApplyMasterVolume(Settings.EffectVolume);
                    sei.Play();
                    return true;
                }
            }
            _MISSING_AUDIO.Play();
            return false;
        }

        public static bool PlayMusicTrack(string key)
        {
            string keyL = key.ToLower();

            if (Settings.MusicOn == false) return false;

            if (string.IsNullOrEmpty(keyL) == false)
            {
                if (_music.ContainsKey(keyL))
                {
                    MediaPlayer.Volume = ApplyMasterVolume(Settings.MusicVolume);
                    MediaPlayer.Play(_music[keyL]);
                    return true;
                }
            }
            _MISSING_AUDIO.Play();
            return false;
        }

        public static SoundEffect GetSoundEffect(string key)
        {
            string keyL = key.ToLower();

            if (string.IsNullOrEmpty(keyL) == false)
            {
                if (_sounds.ContainsKey(keyL))
                {
                    return _sounds[keyL];
                }
            }
            return _MISSING_AUDIO;
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }

        private static float ApplyMasterVolume(float otherVolume)
        {
            return MathHelper.Clamp(otherVolume * Settings.MasterVolume, 0.0f, 1.0f);
        }

        public static bool Repeat
        {
            get
            {
                return MediaPlayer.IsRepeating;
            }
            set
            {
                MediaPlayer.IsRepeating = value;
            }
        }

        public static SoundEffect MISSING_AUDIO
        {
            get { return _MISSING_AUDIO; }
            set { _MISSING_AUDIO = value; }
        }

        public static string SongPosition
        {
            get
            {
                TimeSpan time = MediaPlayer.PlayPosition;
                return time.Minutes.ToString() + ":" + time.Seconds.ToString();
            }
        }
    }
}
