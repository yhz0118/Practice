
using System;


namespace openClose
{

    public enum Color { Red, Green, Blue, Yellow }
    public enum Size { Small, Medium, Large }

    // product class
    public class Product
    {

        public string name;
        public Color color;
        public Size size;

        public Product(string name, Color color, Size size) // constructor
        {
            this.name = name;
            this.color = color;
            this.size = size;
        }

        public override string ToString()
        {
            return $"product: {name} / {color} / {size}";
        }
    }

    // spec class with generic
    public class Spec<T>
    {
        public static AndSpec<T> operator & (Spec<T> a1, Spec<T> a2)
        {
            return new AndSpec<T>(a1, a2);
        }
        
        public static OrSpec<T> operator | (Spec<T> o1, Spec<T> o2)
        {
            return new OrSpec<T>(o1, o2);
        }

        public virtual bool IsSatisfied(T item)
        {
            return false;
        }
    }

    public class AndSpec<T> : Spec<T>
    {
        private Spec<T> one;
        private Spec<T> two;

        public AndSpec(Spec<T> one, Spec<T> two)
        {
            this.one = one;
            this.two = two;
        }

        public override bool IsSatisfied(T item)
        {
            return one.IsSatisfied(item) && two.IsSatisfied(item);
        }
    }


    public class OrSpec<T> : Spec<T>
    {
        private Spec<T> one;
        private Spec<T> two;

        public OrSpec(Spec<T> one, Spec<T> two)
        {
            this.one = one;
            this.two = two;
        }

        public override bool IsSatisfied(T item)
        {
            return one.IsSatisfied(item) || two.IsSatisfied(item);
        }
    }


    // color spec
    public class ColorSpec : Spec<Product>
    {
        private Color color;

        public ColorSpec(Color color)
        {
            this.color = color;
        }

        public override bool IsSatisfied(Product item)
        {
            return item.color == color;
        }
    }

    // size spec
    public class SizeSpec : Spec<Product>
    {
        private Size size;

        public SizeSpec(Size size)
        {
            this.size = size;
        }

        public override bool IsSatisfied(Product item)
        {
            return item.size == size;
        }
    }


    // filter class

    public class Filter<T>
    {
        public virtual List<T> RealFilter(List<T> item, Spec<Product> spec)
        {
            return new List<T>();
        }
    }

    public class BetterFilter : Filter<Product>
    {
        public override List<Product> RealFilter(List<Product> item, Spec<Product> spec)
        {
            return item.FindAll(item => spec.IsSatisfied(item));
        }
    }


    // program class
    class Program
    {
        public static void Main(string[] args)
        {
            var all = new List<Product>
            {
                new Product("Blueberry", Color.Blue, Size.Small),
                new Product("Strawberry", Color.Red, Size.Small),
                new Product("Asparagus", Color.Green, Size.Medium),
                new Product("Banana", Color.Yellow, Size.Large)
            };

            var bf = new BetterFilter();

            var greenAndlarge = new ColorSpec(Color.Green) & new SizeSpec(Size.Large);
            var greenThings = bf.RealFilter(all, greenAndlarge);

            var greenOrlarge = new ColorSpec(Color.Green) | new SizeSpec(Size.Large);
            var greenOrLargeThings = bf.RealFilter(all, greenOrlarge);

            //test 1
            foreach (var x in greenThings)
            { Console.WriteLine(x.name, " ", "is green"); }

            //test 2
            foreach(var x in greenOrLargeThings)
            { Console.WriteLine(x.name, " ", "is green or large"); }
        }
    }
}