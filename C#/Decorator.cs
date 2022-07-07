using System;


namespace Decorator
{

    interface ICoffee
    {
        //virtual ~ICoffee() = default;

        public double Cost();
        public string Ingredient();
    }


    class SimpleCoffee: ICoffee
    {
        private double m_cost;
        private string m_ingredient;

        public SimpleCoffee()
        {
            m_cost = 1;
            m_ingredient = "coffee";
        }

        //virtual ~SimpleCoffee(){}

        public double Cost()
        {
            return m_cost;
        }

        public string Ingredient()
        {
            return m_ingredient;
        }
    }


    class CoffeeDecorator : ICoffee
    {
        protected ICoffee m_coffee;

        public CoffeeDecorator(ICoffee coffee)
        {
            this.m_coffee = coffee;
        }

        public virtual double Cost()
        {
            return m_coffee.Cost();
        }

        public virtual string Ingredient()
        {
            return m_coffee.Ingredient();
        }
    }


    class Milk : CoffeeDecorator
    {
        private double m_cost;
        private string m_ingredient;

        public Milk(ICoffee coffee) : base(coffee)
        {
            m_cost = 9.5;
            m_ingredient = "Milk";
        }

        public override double Cost()
        {
            return m_coffee.Cost() + m_cost;
        }

        public override string Ingredient()
        {
            return m_coffee.Ingredient() + " " + m_ingredient;
        }
    }

    class Whip: CoffeeDecorator
    {
        private double m_cost;
        private string m_ingredient;

        public Whip(ICoffee coffee) : base(coffee)
        {

            m_cost = 0.7;
            m_ingredient = "Whip";
        }

        public override double Cost()
        {
            return m_coffee.Cost() + m_cost;
        }

        public override string Ingredient()
        {
            return m_coffee.Ingredient() + " " + m_ingredient;
        }
    }

    class Sprinkles : CoffeeDecorator
    {
        private double m_cost;
        private string m_ingredient;

        public Sprinkles(ICoffee coffee) : base(coffee)
        {
            m_cost = 0.2;
            m_ingredient = "Sprinkles";
        }

        public override double Cost()
        {
            return m_coffee.Cost() + m_cost;
        }

        public override string Ingredient()
        {
            return m_coffee.Ingredient() + " " + m_ingredient;
        }
    }



    class Program
    {
        public static void Main(string[] args)
        {
            var coffee = new Sprinkles(new Milk(new SimpleCoffee()));

            Console.WriteLine(coffee.Cost());
            Console.WriteLine(coffee.Ingredient());

            Console.ReadKey();
        }
    }

}