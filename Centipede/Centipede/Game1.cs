using System;
using System.Collections;
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

        int score;

        LinkedList<Centipede> centipedes;
        Texture2D img;//Use this texture if you want to test the visuals. We need to delete this before we submit project.
        Texture2D none;
        Player player;
        KeyboardState kb;
        KeyboardState kbO;
        Level level;

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

            player = new Player(null, null, new Rectangle(0, GraphicsDevice.Viewport.Height - 20, 20, 20),
                GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            level = new Level();

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

            Globals.mushroom0 = img;
            Globals.mushroom1 = Content.Load<Texture2D>("dmg1");
            Globals.mushroom2 = Content.Load<Texture2D>("dmg2");

            player.setTex(Content.Load<Texture2D>("graphicstest"));
            player.setProjTex(Content.Load<Texture2D>("graphicstest"));

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
            kb = Keyboard.GetState();

            // TODO: Add your update logic here

            if (kb.IsKeyDown(Keys.LeftAlt) && !kbO.IsKeyDown(Keys.LeftAlt))
                updateLevel();//This is to test what you are working on in multiple levels. (Secret skip button)

            //player movement logic
            if (kb.IsKeyDown(Keys.W))
            {
                player.moveUp();
            }
            if (kb.IsKeyDown(Keys.A))
            {
                player.moveLeft();
            }
            if (kb.IsKeyDown(Keys.D))
            {
                player.moveRight();
            }
            if (kb.IsKeyDown(Keys.S))
            {
                player.moveDown();
            }

            if (kb.IsKeyDown(Keys.Space) && !player.isFiring)
                player.fire();

            if (player.isFiring)
                player.updateProj(level.mushrooms);


            //Centipede cleaning
            /* We don't currently add centipedes
            foreach (Centipede c in centipedes) {
                if (c.size() == 0) {
                    centipedes.Remove(c);
                }
            }
            */

            kbO = kb;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(level.backgroundColor);
            spriteBatch.Begin();
            player.draw(spriteBatch, gameTime);
            for (int x = 0; x < level.mushrooms.GetLength(0); x++) {
                for (int y = 0; y < level.mushrooms.GetLength(0); y++) {
                    level.mushrooms[x, y].Draw(spriteBatch, gameTime);
                }
            }
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        // TODO: Once we introduce Centipede speed, also update that here as well
        public void updateLevel() {
            level = new Level(level.id);
        }

        public bool[] Collision(Player pc)
        {
            Rectangle one = pc.getRec();
            int indexU = (one.Y - pc.speedY) / 20;
            int indexD = (one.Y + pc.speedY) / 20;
            int indexL = (one.X - pc.speedX) / 20;
            int indexR = (one.Y + pc.speedY) / 20;
            bool[] check = new bool[4];//Boolean Order: Up, Down, Left, Right
            if (level.mushrooms[one.X / 20, indexU].visible == true)
                check[0] = true;
            if (level.mushrooms[one.X / 20, indexD].visible == true)
                check[1] = true;
            if (level.mushrooms[indexL, one.Y / 20].visible == true)
                check[2] = true;
            if (level.mushrooms[indexR + 1, one.Y / 20].visible == true)
                check[3] = true;
            return check;
        }
    }

}
