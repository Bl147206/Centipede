using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Centipede
{
    class Centipede
    {
        
        Rectangle[] body;
        Texture2D[] head;
        Texture2D[] bodyTex;
        int topBound, rightBound, bottomBound, leftBound, velocity, turningTime;
        //turning time is the number of updates the snake will be moving along the wall for
        bool turning;

        //used for making new centipedes
        public Centipede(int length, int velocity, int left, int right, int top, int bottom)
        {
            body = new Rectangle[length];
            this.velocity = velocity;
            topBound = top;
            rightBound = right;
            bottomBound = bottom;
            leftBound = left;
            buildSnake();
            turningTime = body[0].Width / Math.Abs(velocity);
        }

        //used when splitting centipedes
        public Centipede(Rectangle[] body, int velocity, int left, int right, int top, int bottom) {
            this.body = body;
            this.velocity = velocity;
            topBound = top;
            rightBound = right;
            bottomBound = bottom;
            leftBound = left;
            turningTime = body[0].Width / Math.Abs(velocity);
        }

        public void setTextures(Texture2D[] h, Texture2D[] b)
        {
            head = h;
            bodyTex = b;
        }

        public void buildSnake()
        {
            body[0] = new Rectangle(290,40,20,20);
        }

        public void Update()
        {
            if (!turning)
            {
                body[0].X += velocity;
            }

            //snakes follow one another
            for (int segment = body.Length-1; segment > 0; segment--)
            {
                if(body[segment].Width == 0)
                {
                    if(Math.Abs(body[segment-1].X) - 290 >= 20)
                    {
                        body[segment] = new Rectangle(290, 40, 20, 20);
                    }
                }
                else
                {
                    if (body[segment].X < leftBound || body[segment].X + body[segment].Width > rightBound)
                    {
                        velocity *= -1;
                        body[segment].Y += Math.Abs(velocity);
                    }else
                    {
                        body[segment].X += velocity;
                    }
                }
            }

            if (turning)
            {
                turningTime--;
                turn();
                if (turningTime == 0)
                {
                    turningTime = body[0].Width / Math.Abs(velocity);
                    turning = false;
                }
            }

        }

        public void turn()
        {
            body[0].Y += Math.Abs(velocity);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(head[gameTime.ElapsedGameTime.Ticks%8], body[0], Color.Red);
            for (int i = 1; i < body.Length; i++)
            {
                spriteBatch.Draw(bodyTex[gameTime.ElapsedGameTime.Ticks % 8], body[i], Color.White);
            }
        }

        public Rectangle[] hit(int segment) {
            Rectangle[] ret = new Rectangle[body.Length - segment];
            Rectangle[] newBody = new Rectangle[segment];
            for (int index = 0; index < body.Length; index++) {
                if (index < segment) {
                    newBody[index] = body[index];
                }
                if (index > segment) {
                    ret[index] = body[index];
                }
            }
            body = newBody;
            if (newBody.Length == 0) {
                body = null;
            }
            return ret;
        }

        public int size() {
            return body.Length;
        }

    }
}
