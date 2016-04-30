﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    // MARTIAN YEAR = 669 Sols
    // MARTIAN DAY = 24h 39m 35s

    // Martians Season Length (Sols)
    // Spring = 194
    // Summer = 178
    // Autumn = 142
    // Winter = 154
    public class Clock
    {
        private const int DAYS_IN_MARTIAN_YEAR = 669;
        private const int SEASON_SPRING = 0;
        private const int SEASON_SUMMER = 194;
        private const int SEASON_AUTUMN = 194 + 178;
        private const int SEASON_WINTER = 194 + 178 + 142;

        private int _sols;
        private Season _season;

        private double _years;
        private double _days;
        private double _hours;
        private double _minutes;
        private double _seconds;
        private double _milisecs;

        private ClockSpeed _clockSpeed;
        private float _clockSpeedMultiplier;

        public Clock()
        {
            _sols = 0;
            _season = Season.Spring;
            _years = 0;
            _days = 0;
            _hours = 0;
            _minutes = 0;
            _seconds = 0;
        }

        public void SetClock(int years, int days, int hours, int minutes, int seconds)
        {
            _years = years;
            _days = days;
            _hours = hours;
            _minutes = minutes;
            _seconds = seconds;

            _clockSpeed = ClockSpeed.RealTime;
            _clockSpeedMultiplier = 0;
        }

        public void SetSpeed(ClockSpeed clockSpeed = ClockSpeed.RealTime, float clockSpeedMultiplier = 1)
        {
            _clockSpeed = clockSpeed;
            _clockSpeedMultiplier = clockSpeedMultiplier;
        }

        public void Update(GameTime gameTime)
        {
            _milisecs += (float)gameTime.ElapsedGameTime.TotalSeconds * _clockSpeedMultiplier;
            float elapsedTime = 0;

            if (_milisecs >= 1.0f)
            {
                _milisecs--;
                elapsedTime++;
            }

            if (_clockSpeed == ClockSpeed.RealTime)
            {
                _seconds += elapsedTime;
            }
            else if (_clockSpeed == ClockSpeed.MinutesPerSecond)
            {
                _minutes += elapsedTime;
            }
            else if (_clockSpeed == ClockSpeed.HoursPerSecond)
            {
                _hours += elapsedTime;
            }
            else if (_clockSpeed == ClockSpeed.DaysPerSecond)
            {
                _days += elapsedTime;
            }
            else if (_clockSpeed == ClockSpeed.YearsPerSecond)
            {
                _years += elapsedTime;
            }

            if (_seconds >= 60)
            {
                // A MINUTE HAS PASSED
                _seconds = _seconds - 60;
                _minutes++;
            }

            if (_minutes >= 60)
            {
                // AN HOUR HAS PASSED
                _minutes = _minutes - 60;
                _hours++;
            }

            // A DAY HAS PASSED
            if (_hours >= 24)
            {
                if (_minutes >= 37)
                {
                    _hours = _hours - 24;
                    _minutes = _minutes - 37;
                    _seconds = _seconds - 35;
                    _days++;
                    _sols++;
                }
            }

            if (_days > DAYS_IN_MARTIAN_YEAR)
            {
                // A YEAR HAS PASSED
                _days = _days - DAYS_IN_MARTIAN_YEAR;
                _years++;
            }

            // Seasons
            if (_days >= SEASON_WINTER)
            {
                _season = Season.Winter;
            }
            else if (_days >= SEASON_AUTUMN)
            {
                _season = Season.Autumn;
            }
            else if (_days >= SEASON_SUMMER)
            {
                _season = Season.Summer;
            }
            else
            {
                _season = Season.Spring;
            }
        }

        public string Time
        {
            get
            {
                string HH = _hours.ToString("N0");
                string MM = _minutes.ToString("N0");

                if (_hours < 10)
                {
                    HH = "0" + HH;
                }
                if (_minutes < 10)
                {
                    MM = "0" + MM;
                }

                return HH + ":" + MM;
            }
        }

        public int Sols
        {
            get { return _sols; }
            set { _sols = value; }
        }

        public Season Season
        {
            get { return _season; }
            set { _season = value; }
        }

        public string DebugText
        {
            get
            {
                return "Year: " + _years.ToString("N0") +
                    " / Day: " + _days.ToString("N0") +
                    //" / Hour: " + _hours.ToString("N0") +
                    //" / Minute: " + _minutes.ToString("N0") +
                    //" / Second: " + _seconds.ToString("N0") +
                    //" / Season: " + _season.ToString() +
                    " / Light: " + _ambience.ToString("N2") + " - " + _ambiencePercentage.ToString("N2") + "%";
            }
        }

        private float _ambience = 1.0f;
        private float _ambiencePercentage = 100.0f;
        private Color ambientColour = Color.White;
        private const float DARK = 0.3f;
        private const float LIGHT = 1.0f;
        
        // 88775 seconds in a day
        // LIGHT   - 06:00 - 6AM
        // SUNRISE - 08:00 - 8AM
        // SUNSET  - 16:00 - 4PM
        // DARK    - 18:00 - 6PM

        public Color AmbientLight
        {
            get
            {
                double midday = 12;
                double m = _minutes / 60;
                double h = _hours + m;

                double z = Math.Cos((h - midday) * Math.PI / 12);
                float b = (float)(0.2f + 0.8f * (z + 1.0) / 2.0);
                _ambience = b;
                return new Color(b, b, b, 1.0f);
            }
        }

        private int ClockTimeToSeconds()
        {
            double hourSeconds = (_hours * 60) * 60;
            double minuteSeconds = _minutes * 60;

            return (int)(hourSeconds + minuteSeconds + _seconds);
        }

        private int HoursToSeconds(int hours)
        {
            return (hours * 60) * 60;
        }
    }

    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }

    public enum ClockSpeed
    {
        RealTime,
        MinutesPerSecond,
        HoursPerSecond,
        DaysPerSecond,
        YearsPerSecond
    }
}