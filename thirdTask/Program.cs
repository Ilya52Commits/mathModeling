﻿using System;
using System.Collections.Generic;

namespace thirdTask;
internal abstract class Program
{
  /* Constant's */
  // d is the number of characters in the input alphabet
  private const int D = 256;

  #region Methods
  /* Rubin Karp algorithm method */
  private static void s_rubinKarpAlgorithmMethod(string pat, string txt, int q)
  {
    /* Variables */
    /* pat -> pattern
      txt -> text
      q -> A prime number
    */
    var m = pat.Length;
    var n = txt.Length;
    var p = 0; // hash value for pattern
    var t = 0; // hash value for txt
    var h = 1;
    int i;

    // The value of h would be "pow(d, M-1)%q"
    for (i = 0; i < m - 1; i++)
      h = (h * D) % q;

    // Calculate the hash value of pattern and first
    // window of text
    for (i = 0; i < m; i++)
    {
      p = (D * p + pat[i]) % q;
      t = (D * t + txt[i]) % q;
    }

    // Slide the pattern over text one by one
    for (i = 0; i <= n - m; i++)
    {
      // Check the hash values of current window of
      // text and pattern. If the hash values match
      // then only check for characters one by one
      if (p == t)
      {
        /* Check for characters one by one */
        int j;
        for (j = 0; j < m; j++)
          if (txt[i + j] != pat[j]) break;

        // if p == t and pat[0...M-1] = txt[i, i+1,
        // ...i+M-1]
        if (j == m)
          Console.WriteLine("Шаблон найден по индексу: " + i);
      }

      // Calculate hash value for next window of text:
      // Remove leading digit, add trailing digit
      if (i >= n - m) continue;
      t = (D * (t - txt[i] * h) + txt[i + m]) % q;

      // We might get negative value of t,
      // converting it to positive
      if (t < 0) t = (t + q);
    }
  }

  /* Boyer and Moore algorithm method */
  private static void s_boyerMooreAlgorithmMethod(IReadOnlyList<char> text, char[] pat)
  {
    // s is shift of the pattern 
    // with respect to text
    int s = 0;
    var m = pat.Length;
    var n = text.Count;

    var bpos = new int[m + 1];
    var shift = new int[m + 1];

    // initialize all occurrence of shift to 0
    for (var i = 0; i < m + 1; i++)
      shift[i] = 0;

    // do preprocessing
    s_preprocessStrongSuffix(shift, bpos, pat, m);
    s_preprocessSecondCase(shift, bpos, m);

    while (s <= n - m)
    {
      var j = m - 1;

      /* Keep reducing index j of pattern while
      characters of pattern and text are matching
      at this shift s*/
      while (j >= 0 && pat[j] == text[s + j])
        j--;

      /* If the pattern is present at the current shift,
      then index j will become -1 after the above loop */
      if (j < 0)
      {
        Console.Write($"Шаблонн содержится при сдвиге = {s}\n");
        s += shift[0];
      }
      else
        /*pat[i] != pat[s+j] so shift the pattern
        shift[j+1] times */
        s += shift[j + 1];
    }
  }
  #endregion

  #region SupportMethods
  // preprocessing for case 2
  private static void s_preprocessSecondCase(IList<int> shift, IReadOnlyList<int> bpos, int m)
  {
    int i;
    var j = bpos[0];
    for (i = 0; i <= m; i++)
    {
      /* set the border position of the first character
      of the pattern to all indices in array shift
      having shift[i] = 0 */
      if (shift[i] == 0)
        shift[i] = j;

      /* suffix becomes shorter than bpos[0],
      use the position of next widest border
      as value of j */
      if (i == j)
        j = bpos[j];
    }
  }

  // preprocess suffix method
  private static void s_preprocessStrongSuffix(IList<int> shift, IList<int> bpos, IReadOnlyList<char> pat, int m)
  {
    // m is the length of pattern 
    int i = m, j = m + 1;
    bpos[i] = j;

    while (i > 0)
    {
      /*if character at position i-1 is not
      equivalent to character at j-1, then
      continue searching to right of the
      pattern for border */
      while (j <= m && pat[i - 1] != pat[j - 1])
      {
        /* the character preceding the occurrence of t
        in pattern P is different than the mismatching
        character in P, we stop skipping the occurrences
        and shift the pattern from i to j */
        if (shift[j] == 0)
          shift[j] = j - i;

        //Update the position of next border 
        j = bpos[j];
      }
      /* p[i-1] matched with p[j-1], border is found.
      store the beginning position of border */
      i--; j--;
      bpos[i] = j;
    }
  }
  #endregion
  
  /* Main method */
  public static void Main()
  {
    const string text = "Какой-то текст в строке";
    const string pat = "то";

    // A prime number
    const int q = 101;

    // Function Call
    Console.WriteLine("Поиск алгоритмом Рабина-Карпа:");
    Console.WriteLine($"Текст для поиска: {text}");
    s_rubinKarpAlgorithmMethod(pat, text, q);
    Console.WriteLine();

    Console.WriteLine("Поиск алгоритмом Бойера и Мура:");  
    var text2 = "ABAAAABAACD".ToCharArray();
    var pat2 = "ABA".ToCharArray();
    s_boyerMooreAlgorithmMethod(text2, pat2);
  }
}