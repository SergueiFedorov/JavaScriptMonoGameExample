using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TryMonoGameScript
{
    public class RectangleConstructor : Jurassic.Library.ClrFunction
    {
        public RectangleConstructor(Jurassic.ScriptEngine engine)
            : base(engine.Object.InstancePrototype, "Rectangle", new Rectangle(engine))
        {

        }
    }

    //Unfortunately has to be done due to the fact that jurrasic cannot
    //automatically create an object instance out of existing structs/classes. I'll try to set a binding
    //system in the future
    class Rectangle : Jurassic.Library.ObjectInstance
    {
        Microsoft.Xna.Framework.Rectangle rectangle;

        public Rectangle(Jurassic.ScriptEngine engine)
            : base(engine)
        {

        }

        public void construct(int x, int y, int width, int height)
        {
            rectangle = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
        }

        public int Width
        {
            get
            {
                return rectangle.Width;
            }
        }

        public int Height
        {
            get
            {
                return rectangle.Height;
            }
        }

        public int X
        {
            get
            {
                return rectangle.X;
            }
        }

        public int Y
        {
            get
            {
                return rectangle.Y;
            }
        }

    }
}
