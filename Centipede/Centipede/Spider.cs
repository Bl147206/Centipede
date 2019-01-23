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
    class Spider
    {
        Texture2D[] texts;
        Rectangle loc;
        int topBound, botBound;
        Vector2 velocity;
        Color color;
        int dir;//1 is left, 2 is right

        public Spider(int topBound, int botBound,Color color,int direction)
        {
            this.topBound = topBound;
            this.botBound = botBound;
            dir = direction;
            this.color = color;

            if (dir == 1)
                velocity = new Vector2(-4, 4);
            else
                velocity = new Vector2(4, 4);
        }

        public void Update()
        {
            loc.X += (int)velocity.X;
            loc.Y -= (int)velocity.Y;
            if (loc.Y > botBound || loc.Y < topBound)
            {
                velocity.Y *= -1;
                if (Globals.rng.Next(1, 2) == 1)
                {
                    velocity.X = 0;
                }
                else
                {
                    if (dir == 1)
                        velocity.X = -4;
                    else
                        velocity.X = 4;

                }


            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texts[0], loc, color);
        }

        

    }
}
