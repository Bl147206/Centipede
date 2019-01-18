using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
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

            Debug.Assert(damage <= 2, "We should not have damage values greater than 2");

            Texture2D mushroomTexture;

            switch (damage) {
                case 0:
                    mushroomTexture = Globals.mushroom0;
                    break;
                case 1:
                    mushroomTexture = Globals.mushroom1;
                    break;
                case 2:
                    mushroomTexture = Globals.mushroom2;
                    break;
                default:
                    return;
            }

            spriteBatch.Draw(mushroomTexture, loc, Color.White);
        }
    }
}
