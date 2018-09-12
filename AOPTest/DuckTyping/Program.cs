using System;
using System.Drawing;

namespace AOPTest.DuckTyping
{
	/// <summary>
	/// This version of duck-typing will try to create a dynamic type of the given object
	/// that conforms to the given interface.
	/// </summary>
	public static class Program
    {
        public static void Main()
        {
			// The first 3 types get converted into IHas2DPosition objects.
			// The last type, SizeF, is set to null, as it doesn't conform to the interface.
            var points = new IHas2DPosition[]
            {
                ProxyFactory.Create<IHas2DPosition>(new Vector3(10.0f, 20.0f, 30.0f)),
				ProxyFactory.Create<IHas2DPosition>(new Vector2(3.141f, 6.282f)),
				ProxyFactory.Create<IHas2DPosition>(new Vector2(6.282f, 3.141f)),
				ProxyFactory.Create<IHas2DPosition>(new PointF(14.98f, 392.12f)),
                ProxyFactory.Create<IHas2DPosition>(new SizeF(50, 100))
            };
            
            foreach (var point in points)
            {
                if (point == null)
                {
                    Console.WriteLine("NULL!");
                }
                else
                {
                    Console.WriteLine("X: {0}, Y: {1}", point.X, point.Y);
                }
            }
        }
    }
}