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
        int topBound, rightBound, bottomBound, leftBound;
        int velocity;

        public Centipede(int length, int velocity, int left, int right, int top, int bottom)
        {
            body = new Rectangle[length];
            this.velocity = velocity;
            topBound = top;
            rightBound = right;
            bottomBound = bottom;
            leftBound = left;
            buildSnake();
        }

        public Centipede(Rectangle[] body, int velocity, int left, int right, int top, int bottom) {
            this.body = body;
            this.velocity = velocity;
            topBound = top;
            rightBound = right;
            bottomBound = bottom;
            leftBound = left;
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
            //snakes follow one another
            for(int segment = body.Length-1; segment > 0; segment--)
            {
                body[segment] = body[segment - 1];
            }
            body[0].X += velocity;
            if(body[0].X < leftBound || body[0].X > rightBound)
            {
                velocity *= -1;
                body[0].Y += 20;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(head[gameTime.ElapsedGameTime.Ticks%8], body[0], Color.White);
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
