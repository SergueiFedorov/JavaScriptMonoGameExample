﻿using System;
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
    /*
    class SpriteConstructor : ClrFunction
    {
        ContentManager content;

        public SpriteConstructor(ScriptEngine engine, ContentManager content)
            : base(engine.Function.InstancePrototype, "sprite", 
                    new Sprite(engine.Object.InstancePrototype, content))
        {
            this.content = content;
        }

        [JSConstructorFunction]
        public Sprite construct(double x, double y, double rotation)
        {
            return new Sprite(this.InstancePrototype, this.content, new Vector2((float)x, (float)y), (double)rotation);
        }
    }*/

    public class Sprite
    {

        public ContentManager Content { get; set; }

        public double someFloat = 10.0;

        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }
        public double rotation { get; set; }
        public double scale { get; set; }

        public double x
        {
            get
            {
                return position.X;
            }
            set
            {
                position = new Vector2((float)value, position.Y);
            }
        }

        [JSProperty(Name = "y")]
        public double y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position = new Vector2(position.X, (float)value);
            }
        }

        public Sprite()
        {

        }

        /*
        public Sprite(ObjectInstance instance, ContentManager content)
            : base(instance)
        {
            this.Content = content;
            this.PopulateFunctions();

            this.scale = 1.0;
        }

        public Sprite(ObjectInstance instance, ContentManager content, Vector2 position, double rotation)
            : base(instance)
        {
            this.Content = content;
            this.position = position;
            this.rotation = rotation;

            this.scale = 1.0;
        }*/

        void Initialize()
        {

        }

        public int SetTexture(string name)
        {
            this.texture = Content.Load<Texture2D>(name);
            this.scale = 1.0f;

            return 0;
        }

        public void Update(GameTime gameTime)
        {
            //base.CallMemberFunction("update", gameTime.ElapsedGameTime.Milliseconds);
        }
    }
}
