using System;

namespace Composite
{
    interface Graphic
    {
        public void Print();
    }

    class CompositeGraphic : Graphic
    {
        private List<Graphic> g = new List<Graphic>();

        public void Print()
        {
            foreach (var m_child in g) { };
        }

        public void Add(Graphic shape)
        {
            g.Add(shape);
        }

        public void Remove(Graphic shape)
        {
            g.Remove(shape);
        }
     
    }

    class Ellipse: Graphic
    {
        private int m_id;

        public Ellipse(int id)
        {
            this.m_id = id;
        }

        public void Print()
        {
            Console.WriteLine("ellipse: "+ m_id);
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            var e1 = new Ellipse(10);
            var e2 = new Ellipse(20);
            var e3 = new Ellipse(30);

            var cp1 = new CompositeGraphic();
            var cp2 = new CompositeGraphic();
            var all = new CompositeGraphic();

            cp1.Add(e1); cp1.Add(e2); cp1.Add(e3);
            cp2.Add(e1); cp2.Add(e2); cp2.Add(e3);

            all.Add(cp1);
            all.Add(cp2);
            all.Print();
        }
    }


}