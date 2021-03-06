﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Centipede
{
    public class Player
    {

        //top measured in pixels the ship can travel above the bottum of the screen
        public const int top = 100;

        public int speedY = 3, speedX = 3, rightEdge = 600, bottomEdge = 640;

        Texture2D playerTex;
        Rectangle playerRec;
        public bool isFiring;
        public Rectangle proj;
        Texture2D projTex;
        Color pColor;

        public Player(Texture2D playerTex,Texture2D projTex, Rectangle playerRec, int rightEdge, int bottomEdge, Color levelColor)
        {
            this.playerTex = playerTex;
            this.playerRec = playerRec;
            this.rightEdge = rightEdge;
            this.bottomEdge = bottomEdge;
            isFiring = false;
            proj = new Rectangle();
            this.projTex = projTex;
            pColor = levelColor;
        }

        public Player(Texture2D playerTex, Rectangle playerRec)
        {
            this.playerTex = playerTex;
            this.playerRec = playerRec;
        }

        //getters setters

        public Rectangle getRec()
        {
            return this.playerRec;
        }

        public void setRec(Rectangle playerRec)
        {
            this.playerRec = playerRec;
        }

        public Texture2D getTex()
        {
            return this.playerTex;
        }

        public void setTex(Texture2D playerTex)
        {
            this.playerTex = playerTex;
            
        }

        public void setProjTex(Texture2D projTex)
        {
            this.projTex = projTex;
        }

        //movement methods
        public void moveUp()
        {
            playerRec.Y -= speedY;
            bounding();
        }

        public void moveDown()
        {
            playerRec.Y += speedY;
            bounding();
        }

        public void moveLeft()
        {
            playerRec.X -= speedX;
            bounding();
        }

        public void moveRight()
        {
            playerRec.X += speedX;
            bounding();
        }

        public void bounding()
        {
            if (playerRec.X < 0)
            {
                playerRec.X = 0;
            }
            if (playerRec.X > rightEdge - playerRec.Width)
            {
                playerRec.X = rightEdge - playerRec.Width;
            }
            if (playerRec.Y > bottomEdge - playerRec.Height)
            {
                playerRec.Y = bottomEdge - playerRec.Height;
            }
            if (playerRec.Y < bottomEdge - top)
            {
                playerRec.Y = bottomEdge - top;
            }
        }

        public void fire()
        {
            if (!isFiring)
            {
                proj = new Rectangle(playerRec.X+(playerRec.Width/2)-2, playerRec.Y, 4, 12);
                isFiring = true;
            }
        }

        public void changeColor(Color newColor)
        {
            pColor = newColor;
        }

        public int updateProj(Mushroom[,] mushrooms,Spider spider)
        {
            proj.Y-=10;
            if (proj.Y < 0)
                isFiring = false;
            foreach(Mushroom m in mushrooms)
            {
                if (proj.Intersects(m.loc) && m.visible)
                {
                    isFiring = false;
                    if (m.hit())
                        return 1;
                }
            }
            if(proj.Intersects(spider.getLoc())&&spider.visible())
            {
                isFiring = false;
                return spider.hit();
            }
            return 0;
        }



        //draw method

        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(playerTex, playerRec, pColor);
            if (isFiring)
                spriteBatch.Draw(projTex, proj, pColor);
            
        }


    }
}



