using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Centipede
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font1;
        int score;
        LinkedList<Centipede> centipedes;
        Player player;
        KeyboardState kb;
        KeyboardState kbO;
        Level level;
        int visualLevel;
        SpriteFont font;
        SpriteFont titleFont;
        bool hasGameStarted = false;
        public Spider spider;
        int highScore;
        bool gameOver;
        Texture2D[] centTextBody;
        Texture2D[] centTextHead;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 600;
            graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            kbO = Keyboard.GetState();
            score = 0;
            highScore = getHighScore();
            visualLevel = 1;
            centipedes = new LinkedList<Centipede>();

            level = new Level(Color.White);
            

            centipedes.AddFirst(new Centipede(10, 5, 0, GraphicsDevice.Viewport.Width, 20, GraphicsDevice.Viewport.Height - 40));
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(null, null, new Rectangle(0, GraphicsDevice.Viewport.Height - 20, 20, 20),
            GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, level.backgroundColor);
            player.setTex(Content.Load<Texture2D>("playerSprite"));
            player.setProjTex(Content.Load<Texture2D>("graphicstest"));
            Globals.mushroom0 = Content.Load<Texture2D>("mushroom0");
            Globals.mushroom1 = Content.Load<Texture2D>("mushroom1");
            Globals.mushroom2 = Content.Load<Texture2D>("mushroom2");
            font = Content.Load<SpriteFont>("SpriteFont1");
            titleFont = Content.Load<SpriteFont>("SpriteFont2");

            spider = new Spider(new Texture2D[] {Content.Load<Texture2D>("spider0"),Content.Load<Texture2D>("spider1") },GraphicsDevice.Viewport.Height-(Player.top*2),GraphicsDevice.Viewport.Height-20,level.backgroundColor,Globals.rng.Next(1,3));

            centTextBody = new Texture2D[2];
            centTextBody[0] = Content.Load<Texture2D>("centbody0");
            centTextBody[1] = Content.Load<Texture2D>("centbody1");

            centTextHead = new Texture2D[2];
            centTextHead[0] = Content.Load<Texture2D>("centHead0");
            centTextHead[1] = Content.Load<Texture2D>("centHead1");

            centipedes.ElementAt(0).setTextures(centTextBody, centTextHead);

            // TODO: use this.Content to load your game content here
            font1 = this.Content.Load<SpriteFont>("SpriteFont1");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            kb = Keyboard.GetState();

            if (!hasGameStarted) {
                if (kb.IsKeyDown(Keys.Enter) && !kbO.IsKeyDown(Keys.Enter)) {
                    hasGameStarted = true;
                }

                return;
            }
            //if (gameOver)
            //{
            //    if (kb.IsKeyDown(Keys.Enter) && !kbO.IsKeyDown(Keys.Enter))
            //    {
            //        gameOver = false;
            //        reset();
            //    }
            //}

            if (kb.IsKeyDown(Keys.LeftAlt) && !kbO.IsKeyDown(Keys.LeftAlt))
                updateLevel();//This is to test what you are working on in multiple levels. (Secret skip button)

            //player movement logic
            if (kb.IsKeyDown(Keys.W))
            {
                if (!Collision(player, 0))
                    player.moveUp();
            }
            if (kb.IsKeyDown(Keys.A))
            {
                if (!Collision(player, 2))
                    player.moveLeft();
            }
            if (kb.IsKeyDown(Keys.D))
            {
                if (!Collision(player, 3))
                    player.moveRight();
            }
            if (kb.IsKeyDown(Keys.S))
            {
                if (!Collision(player, 1))
                    player.moveDown();
            }

            if (kb.IsKeyDown(Keys.Space) && !player.isFiring)
                player.fire();

            if (player.isFiring)
            {
                score += player.updateProj(level.mushrooms, spider);
            }
            if (player.isFiring)
            {//having 2 is neccesary for the way centipedeproj works, do not combine it breaks shooting
                for(int i = 0; i < centipedes.Count; i++)
                {
                    player.isFiring = centipedeProj(centipedes.ElementAt(i));
                    if (!player.isFiring)
                    {
                        break;
                    }
                }
            }
            //if (kb.IsKeyDown(Keys.I) && kbO.IsKeyDown(Keys.I))
            //    gameOver = true;

            


            //Centipede logic
            for(int c = 0; c < centipedes.Count; c++)
            {
                if(centipedes.ElementAt(c).size() == 0)
                {
                    centipedes.Remove(centipedes.ElementAt(c--));
                }
                else
                {
                    if (centipedeCollision(centipedes.ElementAt(c), gameTime))
                    {//if a collision does not occur the update happens
                        centipedes.ElementAt(c).Update(gameTime);
                    }
                }
            }

            if(centipedes.Count == 0)
            {
                newCentipede();
            }

            if (kb.IsKeyDown(Keys.P) && !kbO.IsKeyDown(Keys.P))
            {
                centipedes.ElementAt(0).hit(5);
            }

            player.changeColor(level.backgroundColor);
            kbO = kb;
            if(spider.visible())
                spider.Update();

            if(!spider.visible()&&Globals.rng.Next(1,250)==1)//respawns spider after a random time
            {
                spider = new Spider(new Texture2D[] { Content.Load<Texture2D>("spider0"), Content.Load<Texture2D>("spider1") },
                    GraphicsDevice.Viewport.Height - (Player.top * 2), GraphicsDevice.Viewport.Height - 20, level.backgroundColor, Globals.rng.Next(1, 3));
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (true)
            {
                // If the game hasn't started yet, lets show them the title screen.
                if (!hasGameStarted)
                {

                    spriteBatch.DrawString(titleFont, "Centipede", new Vector2(125, 100), Color.White);
                    spriteBatch.DrawString(font, "Move around with W, A, S, and D", new Vector2(125, 300), Color.White);
                    spriteBatch.DrawString(font, "Press Spacebar to shoot mushrooms and the Centipede!", new Vector2(25, 400), Color.White);
                    spriteBatch.DrawString(font, "Press Enter to start!", new Vector2(175, 500), Color.White);

                    // Ensure that we end the sprite thing as well.
                    spriteBatch.End();
                    return;
                }

            player.draw(spriteBatch, gameTime);
            for (int x = 0; x < level.mushrooms.GetLength(0); x++)
            {
                for (int y = 0; y < level.mushrooms.GetLength(0); y++)
                {
                    level.mushrooms[x, y].Draw(spriteBatch, gameTime); 
                }
            }
            if (spider.visible())
                spider.Draw(spriteBatch, gameTime);

                spriteBatch.DrawString(font1, "Score: " + score, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(font1, "Level: " + visualLevel, new Vector2(450, 0), Color.White);
            }
            else
                spriteBatch.DrawString(font1, "Game Over", new Vector2(250, 300), Color.White);
            spriteBatch.DrawString(font1, "High Score: " + (score > highScore ? score : highScore), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font1, "Score: " + score, new Vector2(200, 0), Color.White);
            spriteBatch.DrawString(font1, "Level: " + visualLevel, new Vector2(450, 0), Color.White);

            foreach (Centipede c in centipedes)
            {
                c.Draw(spriteBatch,gameTime);
            }

            spriteBatch.End();


            // TODO: Add your drawing code here
            


            base.Draw(gameTime);
        }

        // TODO: Once we introduce Centipede speed, also update that here as well
        public void updateLevel() {
            level = new Level(level.backgroundColor, visualLevel);
            visualLevel += 1;
        }

        public int getHighScore() {
            if (File.Exists("high-score.txt")) {
                string content = File.ReadAllText("high-score.txt");
                return int.Parse(content);
            }

            // We don't have a high score
            return 0;
        }

        public void setHighScore(int newScore) {
            // Make the file and set the score
            File.WriteAllText("high-score.txt", score.ToString());
        }

        public void newCentipede(CentipedeSegment[] cent)
        {//creates and adds a new centipede when a split occurs
            centipedes.AddFirst(new Centipede(cent, 5, 0, GraphicsDevice.Viewport.Width, 20, GraphicsDevice.Viewport.Height - 40));
            centipedes.ElementAt(0).setTextures(centTextHead, centTextBody);
        }

        public void newCentipede()
        {//used for spawning an entirely new centipede
            centipedes.AddFirst(new Centipede(10, 5, 0, GraphicsDevice.Viewport.Width, 20, GraphicsDevice.Viewport.Height - 40));
            centipedes.ElementAt(0).setTextures(centTextBody, centTextHead);
        }

        public bool centipedeProj(Centipede c)
        {//calculates relationship between a speffic centipede and the ships projectile
            for(int segment = 0; segment < c.body.Length; segment++)
            {
                if (c.body[segment].position.Intersects(player.proj))
                {
                    score += 1;
                    level.mushrooms[c.body[segment].position.X / 20, (c.body[segment].position.Y - 40) / 20].visible = true;
                    newCentipede(c.hit(segment));
                    return false;
                }
            }
            return true;
        }

        public bool centipedeCollision(Centipede centipede, GameTime gameTime)
        {//This works I think but is kinda weird and finicky on relative positioning
            CentipedeSegment head = centipede.body[0];
            foreach(Mushroom m in level.mushrooms)
            {
                if (!centipede.recentBounce && head.position.Intersects(m.loc) && m.visible)
                {
                    if(head.velocity > 0)
                    {
                        centipede.addTurn(new Vector2(m.loc.X - 20, m.loc.Y), gameTime);
                        centipede.body[0].position.X -= head.velocity;
                    }
                    else if(head.velocity < 0)
                    {
                        centipede.addTurn(new Vector2(m.loc.X + 20, m.loc.Y), gameTime);
                        centipede.body[0].position.X -= head.velocity;
                    }
                    centipede.body[0].turn();
                    return false;
                }
            }
            return true;
        }

        public bool Collision(Player pc, int direction) {
            Rectangle one = pc.getRec();
            int oneCx = one.X + (one.Width / 2);
            int oneCy = one.Y + (one.Height / 2);

            bool[] check = new bool[4];//Boolean Order: Top, Bottom, Left, Right
            check[0] = false;
            check[1] = false;
            check[2] = false;
            check[3] = false;

            foreach (Mushroom m in level.mushrooms) {
                int centerX = m.loc.X + (m.loc.Width / 2);
                int centerY = m.loc.Y + (m.loc.Height / 2);
                if (one.Intersects(m.loc) && centerY < oneCy && m.visible)//Top
                    check[0] = true;
                if (one.Intersects(m.loc) && centerY > oneCy && m.visible)//Bottom
                    check[1] = true;
                if (one.Intersects(m.loc) && centerX < oneCx && m.visible)//Left
                    check[2] = true;
                if (one.Intersects(m.loc) && centerX > oneCx && m.visible)//Right
                    check[3] = true;
            }
            bool final = check[direction];
            return final;
        }
        public void reset()
        {
            visualLevel = 1;
            level = new Level(Color.White);
            hasGameStarted = false;

        }

    }

}
