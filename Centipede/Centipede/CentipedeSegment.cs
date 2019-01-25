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
    public class CentipedeSegment
    {
        public Rectangle position;
        public int velocity, yDirection, frame;

        public CentipedeSegment(Rectangle position, int velocity)
        {
            this.position = position;
            this.velocity = velocity;
            this.yDirection = 1;
        }

        public void update()
        {
            position.X += velocity;
            frame++;
        }

        public void turn()
        {//used when hitting a horizontal limit
            position.Y += 20 * yDirection;
            velocity *= -1;
        }

        public void turnVeritcal()
        {//used when hitting a vertical limit
            yDirection *= -1;
            turn();
        }

        public bool pastSpawn()
        {//used to know when it's time to spawn a new segment - see update method in centipede
            return position.X > 310;
        }

        public Vector2 origin()
        {//if time for rotations
            return new Vector2(10, 10);
        }

        public int animationFrame()
        {
            if(frame % 20 < 10)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

    }
}
