﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystem
{
    public class Monitor : GameEntity
    {
        private SpriteFont Font { get; set; }
        private List<string> Info { get; set; }
        private List<string> Help { get; set; }

        //TODO: leap year
        private readonly int[] NumDaysOfMonths = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

        private readonly string[] NamesOfMonths =
            { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public Monitor()
        {
            Info = new List<string>();
            Help = new List<string>();
        }

        public override void LoadContent()
        {
            Font = Game.Content.Load<SpriteFont>(@"Fonts\font1");
        }

        public override void Update(float dt)
        {
            Info.Clear();

            var minsFromMidday = Game.Earth.Revolution * 1440 / Earth.RevolutionAngularSpeed + 720;
            var dayFromVernalEquinox = (int)(minsFromMidday / 1440);

            var month = 2;
            var day = 21;

            for (; ; )
            {
                var numDaysInMonth = NumDaysOfMonths[month];

                if (day + dayFromVernalEquinox > numDaysInMonth)
                {
                    dayFromVernalEquinox += day - numDaysInMonth;
                    day = 0;
                    month = (month + 1)%12;
                }
                else
                {
                    day += dayFromVernalEquinox;
                    break;
                }
            }

            minsFromMidday %= 1440;
            var hour = (int)(minsFromMidday / 60);
            var minute = (int)(minsFromMidday % 60);

            var date = string.Format("Date: {0} {1}", NamesOfMonths[month], day);
            var time = string.Format("Time: {0:D2}:{1:D2}", hour, minute);

            const string earthRevSpeed = "Revolution Speed: 29.8 km/s";
            const string earthRotSpeed = "Rotation Speed: 15 degree/s";
            const string moonRevSpeed = "Revolution Speed: 1.02 km/s";
            const string moonRotSpeed = "Rotation Speed: 1.54 degree/s";

            Info.Add(date);
            Info.Add(time);
            Info.Add("Earth");
            Info.Add(earthRevSpeed);
            Info.Add(earthRotSpeed);
            Info.Add("Moon");
            Info.Add(moonRevSpeed);
            Info.Add(moonRotSpeed);

            Help.Clear();
            Help.Add("Help");
            Help.Add("Esc: quit");
            Help.Add("P: show Earth's revolution orbit");
            Help.Add("O: show Earth's rotation     orbit");
            Help.Add("L: show Moon's revolution orbit");
            Help.Add("K: show Moon's rotation     orbit");
            Help.Add("1: general view");
            Help.Add("2: the Earth's view");
            Help.Add("Up: speed up motion");
            Help.Add("Down: slowdown motion");
        }

        public override void Draw(float dt)
        {
            var pos = new Vector2(10, 10);
            foreach (var info in Info)
            {
                Game.SpriteBatch.DrawString(Font, info, pos, Color.White);
                pos += new Vector2(0, 20);
            }

            var runningSpeed = string.Format("Speed: x {0:N0}", Game.Setting.Speed);
            Game.SpriteBatch.DrawString(Font, runningSpeed, new Vector2(630, 10), Color.White);

            pos = new Vector2(1150, 10);
            foreach (var help in Help)
            {
                Game.SpriteBatch.DrawString(Font, help, pos, Color.White);
                pos += new Vector2(0, 20);
            }
        }
    }
}
