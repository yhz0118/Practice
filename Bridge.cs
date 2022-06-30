using System;


namespace Bridge
{

    interface DrawingAPI
    {
        public void DrawCircle(double x, double y, double radius);
    }

    class CocoaAPI : DrawingAPI
    {
        public void DrawCircle(double x, double y, double radius)
        {
            Console.WriteLine(string.Format("Cocoa.circle at ({0}, {1}, {2})", x, y, radius));
        }
    }

    class CarbonAPI : DrawingAPI
    {
        public void DrawCircle(double x, double y, double radius)
        {
            Console.WriteLine(string.Format("Carbon.circle at ({0}, {1}, {2})", x, y, radius));
        }
    }




    class Shape
    {
        protected DrawingAPI m_api;

        public Shape(DrawingAPI api)
        {
            m_api = api;
        }

        public virtual void Draw(){ }
        public virtual void ResizeBy(double percentage){ }
    }


    class Circle : Shape // inherit
    {
        private double m_x, m_y, m_radius;

        public Circle(double x, double y, double radius, DrawingAPI api) : base(api)
        {
            m_x = x;
            m_y = y;
            m_radius = radius;
        }

        public override void Draw()
        {
            m_api.DrawCircle(m_x, m_y, m_radius);
        }

        public override void ResizeBy(double percentage)
        {
            m_radius *= percentage;
        }
    }


    class Program
    {
        public static void Main(string[] args)
        {
            var cocoa = new CocoaAPI();
            var carbon = new CarbonAPI();

            var s1 = new Circle(1, 2, 3, cocoa);
            var s2 = new Circle(2, 3, 43, carbon);

            s1.Draw();
            s2.Draw();
        }
    }
}
    