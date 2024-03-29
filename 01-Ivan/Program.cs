﻿using System;

internal abstract class Program
{ 
  private static double s_functionMethod(double x)
  {
    var numerator = Math.Pow(x, 2) - 3 * x + 2;

    return Math.Pow(numerator, 1.0 / 3.0);
  }
        
  /* Svenn method */
  private static (double, double) s_svennsMethod(double x)
  {
    var h = 0.5;
    var solution = s_functionMethod(x);

    Console.WriteLine("Решение уравнения: " + solution); 
      
    var a = x - h;
    var b = x + h;
    var fa = s_functionMethod(a);
    var fb = s_functionMethod(b);

    Console.WriteLine(fa + " " + solution + " " + fb);
    if (fa < solution && solution > fb) {
      Console.WriteLine("Отрезок, содержащий точку минимума: [" + a + ", " + b + "]");
      return (a, b);
    }

    if (fa > solution && solution > fb)
    {
      h = -h;
      (a, b) = (b, a);
    }

    while (fa > fb)
    {
      x += h;
      a = x - h;
      b = x + h;
      fa = s_functionMethod(a); fb = s_functionMethod(b);
    }

    if (h > 0) (a, b) = (b, a);

    Console.WriteLine("Отрезок, содержащий точку минимума: [" + a + ", " + b + "]");

    return (a, b);
  }

  /* Method of golden ratio */  
  private static double s_FindMinimum(double x)
  {
    const double e = 0.001;
    var result = s_svennsMethod(x);
    var a = result.Item1;
    var b = result.Item2;
    
    var phi = (1 + Math.Sqrt(5)) / 2; 
    var x1 = a + 0.382 * (b - a);
    var x2 = b - 0.382 * (b - a);
    
    var f1 = s_functionMethod(x1);
    var f2 = s_functionMethod(x2);

    while (Math.Abs(b - a) > e)
    {
      if (f1 > f2)
      {
        b = x2;
        x2 = x1;
        f2 = f1;
        x1 = a + (1 - phi) * (b - a);
        f1 = s_functionMethod(x1);
      }
      else
      {
        a = x1;
        x1 = x2;
        f1 = f2;
        x2 = b - (1 - phi) * (b - a);
        f2 = s_functionMethod(x2);
      }
    }

    return (a + b) / 2;
  }
  
  /* Main method */
  public static void Main()
  {
    const double x = -1;
    Console.WriteLine(s_FindMinimum(x));
  }
}