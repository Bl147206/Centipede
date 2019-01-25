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
    public class Centipede
    {
        
        public CentipedeSegment[] body;
        Texture2D[] head;
        Texture2D[] bodyTex;
        int topBound, rightBound, bottomBound, leftBound, velocity, turningTime, bounceTimer;
        LinkedList<Vector2> turnPoints;
        LinkedList<long> removeTimers;
        public bool recentBounce;

        //turning time is the number of updates the snake will be moving along the wall for

        //used for making new centipedes
        public Centipede(int length, int velocity, int left, int right, int top, int bottom)
        {
            body = new CentipedeSegment[length];
            this.velocity = velocity;
            topBound = top;
            rightBound = right;
            bottomBound = bottom;
            leftBound = left;
            turnPoints = new LinkedList<Vector2>();
            removeTimers = new LinkedList<long>();
            buildSnake();
            turningTime = body[0].position.Width / Math.Abs(velocity);
        }

        //used when splitting centipedes
        public Centipede(CentipedeSegment[] body, int velocity, int left, int right, int top, int bottom) {
            this.body = body;
            this.velocity = velocity;
            topBound = top;
            rightBound = right;
            bottomBound = bottom;
            turnPoints = new LinkedList<Vector2>();
            removeTimers = new LinkedList<long>();
            leftBound = left;
            turningTime = body[0].position.Width / Math.Abs(velocity);
        }

        public void setTextures(Texture2D[] h, Texture2D[] b)
        {
            head = h;
            bodyTex = b;
        }

        public void buildSnake()
        {
            body[0] = new CentipedeSegment(new Rectangle(290, 40, 20, 20), velocity);
        }

        public void Update(GameTime gameTime)
        {//controlls most actions
            for (int segment = 0; segment < body.Length; segment++)
            {
                if(body[segment] == null && segment != 0)
                {//Creating the centipede at the begging of the game
                    if (body[segment - 1].pastSpawn())
                    {
                        body[segment] = new CentipedeSegment(new Rectangle(290, 40, 20, 20), velocity);
                    }
                    break;
                }
                else
                {//Standard movement
                    body[segment].update();
                    if(body[segment].position.X < 0 || body[segment].position.X > rightBound - 20)
                    {//turn based on bounding/ game walls
                        if (body[segment].position.Y + 20 > bottomBound || body[segment].position.Y < topBound)
                        {
                            body[segment].turnVeritcal();
                        }
                        else
                        {
                            body[segment].turn();
                        }
                        if(segment == 0)
                        {
                            recentBounce = true;
                        }
                    }
                }
            }
            foreach (Vector2 turn in turnPoints)
            {//turning as a result of a mushroom - self clears
                for(int segment = 0; segment < body.Length; segment++)
                {
                    if(body[segment] != null)
                    {
                        if (body[segment].position.X == turn.X && body[segment].position.Y == turn.Y)
                        {
                            body[segment].turn();
                            body[segment].position.X -= body[segment].velocity;
                        }
                    }
                }
            }
            if(removeTimers.Last != null && removeTimers.ElementAt(removeTimers.Count-1) == gameTime.ElapsedGameTime.Ticks - 100)
            {
                removeTimers.RemoveLast();
                turnPoints.RemoveLast();
            }

            if (recentBounce)
            {//stops a bug from happening when the head of the centipede shifts down a wall into a mushroom
                bounceTimer++;
                if(bounceTimer == 8)
                {
                    recentBounce = false;
                    bounceTimer = 0;
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {//basic draw methods, add directions if time allows
            spriteBatch.Draw(head[gameTime.ElapsedGameTime.Ticks%2], body[0].position, Color.Red);
            for (int i = 1; i < body.Length; i++)
            {
                if(body[i] != null)
                spriteBatch.Draw(bodyTex[gameTime.ElapsedGameTime.Ticks % 2], body[i].position, Color.White);
            }
        }

        public CentipedeSegment[] hit(int segment)
        {//Splitting method for a hit. Not tested
            CentipedeSegment[] ret = new CentipedeSegment[body.Length - segment];
            CentipedeSegment[] newBody = new CentipedeSegment[segment];
            for (int index = 0; index < body.Length; index++)
            {
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

        public void addTurn(Vector2 turn, GameTime gameTime)
        {//adds a turn typically as a result of a mushroom collision
            turnPoints.AddFirst(turn);
            removeTimers.AddFirst((int)gameTime.ElapsedGameTime.Ticks);
        }

        public int size()
        {
            return body.Length;
        }

    }
}
