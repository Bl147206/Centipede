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
    class Mushroom
    {
        Texture2D[] tex;
        Rectangle loc;
        int damage;
        
        
        public Mushroom(int x,int y)
        {
            tex = null;
            loc = new Rectangle(x,y,20,20);
            damage = 0;
        }

        public void setTex(Texture2D[] t)
        {
            tex = t;
        }

        public void hit()
        {
            damage++;
        }


    }
}
