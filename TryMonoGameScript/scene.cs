using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Jurassic;
using Jurassic.Compiler;
using Jurassic.Library;

using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TryMonoGameScript
{    
    public class Scene
    {
        SpriteBatch batch { get; set; }
        List<Sprite> sprites = new List<Sprite>();

        public Scene()
        {

        }

        public void addSprite(Sprite sprite)
        {
            sprites.Add(sprite);
        }

        public void Draw()
        {
            batch.Begin();
            foreach (Sprite sprite in sprites)
            {
                batch.Draw(sprite.texture, sprite.position, null, Color.White, (float)sprite.rotation, new Vector2(sprite.texture.Width / 2.0f, sprite.texture.Height / 2.0f), (float)sprite.scale, SpriteEffects.None, 0.0f);
            }
            batch.End();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Update(gameTime);
            }
        }

    }
}
