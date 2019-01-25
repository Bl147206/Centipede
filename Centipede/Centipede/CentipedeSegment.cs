using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace Centipede
{
    class CentipedeSegment
    {
        public Rectangle position;
        public int velocity, yDirection;
        public bool isTurning, entered;

        public CentipedeSegment(Rectangle position, int velocity)
        {
            this.position = position;
            this.velocity = velocity;
            this.yDirection = 1;
            this.isTurning = false;
        }

        public void update()
        {
            if (isTurning)
            {
                position.Y += velocity;
            }
            else
            {
                position.X += velocity;
            }
        }

        public void turn()
        {
            position.Y += 20 * yDirection;
            velocity *= -1;
        }

        public void turnVeritcal()
        {
            yDirection *= -1;
            turn();
        }

        public bool pastSpawn()
        {
            return position.X > 310;
        }

        public Vector2 origin()
        {
            return new Vector2(10, 10);
        }

    }
}
