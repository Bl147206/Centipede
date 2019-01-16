﻿using System;
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
    class Mushroom
    {
        Texture2D[] texts;//First image in array is the texture, second one is blank at the moment
        public Rectangle loc;
        int damage;
        bool visible;

        public Mushroom(Rectangle l)
        {
            texts = null;
            loc = l;
            damage = 0;
            visible = false;
        }

        public void setTex(Texture2D[] t)
        {
            texts = t;
        }

        public void generate()
        {
            damage = 0;
            visible = true;
        }

        public void hit()
        {
            damage++;
            if(damage>2)
            {
                visible = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texts[damage], loc, Color.White);
        }
    }
}
