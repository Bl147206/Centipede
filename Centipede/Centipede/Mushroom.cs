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
    public class Mushroom
    {
        public Rectangle loc;
        int damage;
        public bool visible;

        public Mushroom(Rectangle l)
        {
            loc = l;
            damage = 0;
            visible = false;
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
            // FIXME: When we introduce projectiles and mushroom hits update this to reflect new mushroom textures
            if (!visible) return;

            spriteBatch.Draw(Globals.mushroom0, loc, Color.White);
        }
    }
}
