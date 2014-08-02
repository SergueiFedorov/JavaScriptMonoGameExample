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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            engine.ExecuteFile("Math.js");
            engine.ExecuteFile("CommonClasses.js");
            engine.ExecuteFile("VariableBridge.js");
            engine.ExecuteFile("scene.js");

            engine.SetGlobalFunction("print", new Func<string, int>((string message) =>
            {
                Console.WriteLine(message);

                return 0;
            }));

            engine.SetGlobalFunction("MonoGame_AddSprite", new Func<string, double, double, double, int>((string script, double x, double y, double rotation) =>
            {
                iterator++;
                Sprite sprite =  new Sprite(script, this.Content, this.engine, new Vector2((float)x, (float)y), (float)rotation);
                sprites.Add(iterator, sprite);
                return iterator;
            }));

            engine.SetGlobalFunction("setTextureFor", new Func<int, string, int>((ID, texture) =>
            {
                Sprite sprite = sprites[ID];
                sprite.texture = Content.Load<Texture2D>(texture);
                return 0;
            }));

            engine.SetGlobalFunction("updateSprite", new Func<int, double, double, double, int>((ID, x, y, rotation) =>
            {
                Sprite sprite = sprites[ID];
                sprite.position = new Vector2((float)x, (float)y);
                sprite.rotation = (float)rotation;

                return 0;
            }));

            using (FileStream stream = File.Open("scene.js", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string code = reader.ReadToEnd();
                    engine.Execute(code);

                    engine.Execute("currentScene.delegate.initialize()");
                }
            }
            
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

        void MonoGame_AddSprite(string script, double x, double y, double rotation)
        {
            //Sprite sprite = new Sprite(script, this.Content, this.engine, new Vector2((float)x, (float)y), (float)rotation);

           // sprites.Add(sprite);
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

            engine.Execute("currentScene.updateInternal(" + gameTime.ElapsedGameTime.Milliseconds + ");");

            //gameObject.Update(gameTime);

            // TODO: Add your update logic here

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

            foreach (KeyValuePair<int, Sprite> keyValue in sprites)
            {
                Sprite sprite = keyValue.Value;
                spriteBatch.Draw(sprite.texture, sprite.position, null, Color.White, sprite.rotation, new Vector2(sprite.texture.Width / 2.0f, sprite.texture.Height / 2.0f), 1.0f, SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
