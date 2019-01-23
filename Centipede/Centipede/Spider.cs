using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Centipede
{
    public class Spider
    {
        Texture2D[] texts;
        Rectangle loc;
        int topBound, botBound;
        Vector2 velocity;
        Color color;
        int dir;//1 is left, 2 is right
        bool isVisible;

        public Spider(Texture2D[] texts, int topBound, int botBound, Color color, int direction)
        {
            this.texts = texts;
            this.topBound = topBound;
            this.botBound = botBound;
            dir = direction;
            this.color = color;
            isVisible = true;

            if (dir == 1)
            {
                velocity = new Vector2(-2, 3);
                loc = new Rectangle(600, 600, 40, 20);
            }
            else
            {
                velocity = new Vector2(2, 3);
                loc = new Rectangle(-40, 600, 40, 20);
            }
        }

        public void Update()
        {

            loc.X += (int)velocity.X;
            loc.Y -= (int)velocity.Y;
            Console.WriteLine(loc.X);
            if (loc.Y > botBound || loc.Y < topBound)
            {
                velocity.Y *= -1;
                if (Globals.rng.Next(1, 3) == 1)
                {
                    velocity.X = 0;
                }
                else
                {
                    if (dir == 1)
                        velocity.X = -2;
                    else
                        velocity.X = 2;

                }


            }
            if (loc.X < -40 || loc.X > 600)
                despawn();
        }

        public void despawn()
        {
            isVisible = false;
        }

        public bool visible()
        {
            return isVisible;
        }

        public int hit()
        {

            despawn();
            return 300;
        }

        public Rectangle getLoc()
        {
            return loc;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
                spriteBatch.Draw(texts[0], loc, color);
        }

        

    }
}
