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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Mushroom[,] mushrooms;
        LinkedList<Centipede> centipedes;
        Texture2D[] mushTexts;
        Texture2D img;//Use this texture if you want to test the visuals. We need to delete this before we submit project.
        Texture2D none;
        Random rng;
        public int level;
        KeyboardState kb;
        KeyboardState kbO;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 640;
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
            mushrooms = new Mushroom[30,30];
            mushTexts = new Texture2D[2];
            rng = new Random();
            level = 0;
            kbO = Keyboard.GetState();
  
            for(int x=0; x< mushrooms.GetLength(0); x++)
            {
                for(int y=0; y< mushrooms.GetLength(0); y++)
                {
                    mushrooms[x, y] = new Mushroom(new Rectangle(x * 20, y * 20 + 40, 20, 20));
                }
            }
            restart();

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
            img = Content.Load<Texture2D>("graphicsTest");
            none = Content.Load<Texture2D>("blank");
            mushTexts[0] = img;
            mushTexts[1] = none;
            for (int x = 0; x < mushrooms.GetLength(0); x++)
            {
                for (int y = 0; y < mushrooms.GetLength(0); y++)
                {
                    mushrooms[x, y].setTex(mushTexts);
                }
            }

            // TODO: use this.Content to load your game content here

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
            kb=Keyboard.GetState();
            // TODO: Add your update logic here
            if (kb.IsKeyDown(Keys.W) && !kbO.IsKeyDown(Keys.W))
                restart();
            kbO = kb;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int x = 0; x < mushrooms.GetLength(0); x++)
            {
                for (int y = 0; y < mushrooms.GetLength(0); y++)
                {
                    mushrooms[x, y].Draw(spriteBatch, gameTime);
                }
            }
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        public int checkArray(Mushroom[,] z)
        {
            int total = 0;
            for(int x=0; x<z.GetLength(0);x++)
            {
                for (int y = 0; y < z.GetLength(0); y++)
                {
                    if (z[x, y] == null)
                        continue;
                    if (z[x,y].visible == true)
                        total++;
                }

            }
            return total;
        }

        public void restart()
        {
            if(level<=15)
                level++;
            for (int a = 0; a < mushrooms.GetLength(0); a++)
            {
                for (int b = 0; b < mushrooms.GetLength(0); b++)
                {
                    mushrooms[a, b].visible = false;
                }
            }
                while (checkArray(mushrooms) < level * 10)
                {
                bool check = false;
                int x = rng.Next(30);
                int y = rng.Next(30);
                if(x-1>=0)
                {
                    if (y - 1 >= 0)
                        if (mushrooms[x - 1, y - 1].visible == true)
                            check = true;
                    if (y + 1 < mushrooms.GetLength(0))
                        if (mushrooms[x - 1, y + 1].visible == true)
                            check = true;
                }

                if (x + 1 < mushrooms.GetLength(0))
                {
                    if (y - 1 >= 0)
                        if (mushrooms[x + 1, y - 1].visible == true)
                            check = true;
                    if (y + 1 < mushrooms.GetLength(0))
                        if (mushrooms[x + 1, y + 1].visible == true)
                            check = true;
                }
                if (check==false)
                mushrooms[x, y].generate();
            }
        }
    }

}
