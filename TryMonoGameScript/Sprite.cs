using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.IO;

using Jurassic;
using Jurassic.Compiler;
using Jurassic.Library;

namespace TryMonoGameScript
{
    class Sprite
    {
        private Jurassic.ScriptEngine engine { get; set; }
        private string scriptFile { get; set; }
        private ContentManager Content { get; set; }

        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }
        public float rotation { get; set; }

        public Sprite(string script, ContentManager content, ScriptEngine engine, Vector2 position, float rotation)
        {
            this.Content = content;
            this.engine = engine;
            this.scriptFile = script;
            this.position = position;
            this.rotation = rotation;

            this.Initialize();
        }



        void Initialize()
        {

        }

        public void Update(GameTime gameTime)
        {
            this.engine.CallGlobalFunction("Update", gameTime.ElapsedGameTime.Milliseconds);

            this.position = new Vector2( (float)engine.Evaluate<double>("position.x"),
                                         (float)engine.Evaluate<double>("position.y") );

            this.rotation = (float)engine.Evaluate<double>("rotation");
        }
    }
}
