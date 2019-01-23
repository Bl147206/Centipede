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
        int score;
        LinkedList<Centipede> centipedes;
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
            level = new Level();
            centipedes = new LinkedList<Centipede>();


            updateLevel();

            centipedes.AddFirst(new Centipede(10, 2, 0, GraphicsDevice.Viewport.Width, 100, GraphicsDevice.Viewport.Height - 40));

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
            level.initialize();
            level.mushroomInit();
            player = new Player(null, null, new Rectangle(0, GraphicsDevice.Viewport.Height - 20, 20, 20),
            GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, level.backgroundColor);
            player.setTex(Content.Load<Texture2D>("playerSprite"));
            player.setProjTex(Content.Load<Texture2D>("graphicstest"));
            Globals.mushroom0 = Content.Load<Texture2D>("mushroom0");
            Globals.mushroom1 = Content.Load<Texture2D>("mushroom1");
            Globals.mushroom2 = Content.Load<Texture2D>("mushroom2");

            //temp code testing centipede
            Texture2D[] centipedeTex = new Texture2D[8];
            for(int i = 0; i < centipedeTex.Length; i++)
            {
                centipedeTex[i] = Content.Load<Texture2D>("graphicsTest");
            }

            centipedes.ElementAt(0).setTextures(centipedeTex, centipedeTex);

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
                score += player.updateProj(level.mushrooms);

            


            //Centipede cleaning
            foreach(Centipede c in centipedes)
            {
                if(c.size() == 0)
                {
                    centipedes.Remove(c);
                }else
                {
                    c.Update();
                }
            }

            player.changeColor(level.backgroundColor);
            kbO = kb;
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
            player.draw(spriteBatch, gameTime);
            for (int x = 0; x < level.mushrooms.GetLength(0); x++)
            {
                for (int y = 0; y < level.mushrooms.GetLength(0); y++)
                {
                    level.mushrooms[x, y].Draw(spriteBatch, gameTime);
                }
            }

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
            level = new Level(level.id);
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
    }

}
