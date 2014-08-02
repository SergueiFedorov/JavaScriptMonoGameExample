#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

using System.IO;

using Jurassic;

namespace TryMonoGameScript
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameBody : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D sprite;
        Jurassic.ScriptEngine engine;

        Sprite gameObject;

        Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>();

        public GameBody()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            engine = new ScriptEngine();
        }

        protected int setSprite(string spriteName)
        {
            sprite = Content.Load<Texture2D>(spriteName);
            return 0;
        }

        int iterator = 0;

        Scene currentScene;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            engine.ExecuteFile("Math.js");
            engine.ExecuteFile("CommonClasses.js");

            engine.SetGlobalValue("sprite", new SpriteConstructor(engine, Content));
            engine.SetGlobalValue("scene", new SceneConstructor(engine, this.spriteBatch));

            engine.Execute("currentScene = new scene();");

            currentScene = engine.GetGlobalValue<Scene>("currentScene");

            engine.ExecuteFile("scene.js");

            engine.SetGlobalFunction("print", new Func<string, int>((string message) =>
            {
                Console.WriteLine(message);

                return 0;
            }));
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentScene.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            currentScene.Draw();


            base.Draw(gameTime);
        }
    }
}
