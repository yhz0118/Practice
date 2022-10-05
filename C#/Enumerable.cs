using System;

public class Test
{
  public static void Main(string []args)
  {
    IEnumerable<int> squares = Enumerable.Range(1, 10).Select(x => x * x);

    foreach (int num in squares)
    {
      Console.WriteLine(num);
    }
  }
}
