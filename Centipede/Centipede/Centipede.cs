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
        Vector4 bounds;
        int velocity;

        public Centipede(int length,int velocity,int left, int right,int top,int bottom)
        {
            body = new Rectangle[length];
            this.velocity = velocity;
            bounds = new Vector4(left, right, top, bottom);
        }

        public Centipede(Rectangle[] body, int velocity, int left, int right, int top, int bottom)
        {
            this.body = body;
            this.velocity = velocity;
            bounds = new Vector4(left, right, top, bottom);
        }

        public void setTextures(Texture2D[] h, Texture2D[] b)
        {
            head = h;
            bodyTex = b;
        }

        public void Update()
        {
            for (int i = 0; i < body.Length; i++)
            {
                body[i].X += velocity;
            }
            if (body[0].X < bounds.X || body[0].X > bounds.Y)
                velocity *= -1;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(head[gameTime.ElapsedGameTime.Ticks%8], body[0], Color.White);
            for (int i = 1; i < body.Length; i++)
            {
                spriteBatch.Draw(bodyTex[gameTime.ElapsedGameTime.Ticks % 8], body[i], Color.White);
            }
        }

        public Rectangle[] hit(int segment)
        {
            Rectangle[] ret = new Rectangle[body.Length-segment];
            Rectangle[] newBody = new Rectangle[segment];
            for(int index = 0; index < body.Length; index++)
            {
                if (index < segment)
                {
                    newBody[index] = body[index];
                }
                if(index > segment)
                {
                    ret[index] = body[index];
                }
            }
            body = newBody;
            if(newBody.Length == 0)
            {
                body = null;
            }
            return ret;
        }

        public int size()
        {
            return body.Length;
        }


    }
}
