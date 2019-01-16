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
        int velocity;

        public Centipede(int length)
        {
            body = new Rectangle[length];            
        }

        public void setTextures(Texture2D[] h, Texture2D[] b)
        {
            head = h;
            bodyTex = b;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(head[gameTime.ElapsedGameTime.Ticks%8], body[0], Color.White);
            for (int i = 1; i < body.Length; i++)
            {
                spriteBatch.Draw(bodyTex[gameTime.ElapsedGameTime.Ticks % 8], body[i], Color.White);
            }
        }

        public int hit(int segment)
        {
            int ret = body.Length - segment;

            return 0;
        }


    }
}
