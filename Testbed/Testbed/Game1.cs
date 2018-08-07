using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Jmx.Monogame.ScreenTools;

namespace Testbed
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GifMaker gif;
        Texture2D mgLogo;
        Rectangle mgLogoBox;
        ScreenShotHelper ssh;
        float rot = 0;
        Vector2 center = new Vector2(200, 200);
        bool gifSaveRequested = false;
        bool doSingleScreenshot = false;
        double lastUpdate = 250;

        public Game1()
        {            
            
            graphics = new GraphicsDeviceManager(this);
            // using a small size until we implement some compression in the gifmaker
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 640;
            Content.RootDirectory = "Content";
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

            base.Initialize();
            ssh = new ScreenShotHelper(GraphicsDevice);
            gif = new GifMaker(GraphicsDevice);
            
            mgLogoBox = new Rectangle((graphics.PreferredBackBufferWidth/3) + 200, (graphics.PreferredBackBufferHeight/3), 400, 400);

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mgLogo = Content.Load<Texture2D>("mglogo");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                //only send a frame every 250ms
                lastUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (lastUpdate > 250)
                {
                    
                    gifSaveRequested = true;
                    gif.AddFrame(250);
                    lastUpdate = 0;                    
                }                
            }
            else if (gifSaveRequested) {
                gif.WriteAllFrames();                
                gifSaveRequested = false;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.F9))
            {
                if (!doSingleScreenshot)
                {
                    ssh.SaveScreenshot();
                    doSingleScreenshot = true;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.F9))
            {
                doSingleScreenshot = false;
            }


            // TODO: Add your update logic here
            rot += .05f;
            if (rot > 180)
                rot = 0;
            

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
            spriteBatch.Draw(mgLogo, mgLogoBox, null, Color.White, rot, center, SpriteEffects.None, 0);            
            spriteBatch.End();

            base.Draw(gameTime);
            
        }
    }
}
