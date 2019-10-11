using System;

namespace Decompress
{
  class Program
  {
    /*
     * 3[abc]4[ab]c
       
       Would be output as
       
       abcabcabcababababc
       
       Other rules
       Number can have more than one digit. For example, 10[a] is allowed, and just means aaaaaaaaaa
       
       One repetition can occur inside another. For example, 2[3[a]b] decompresses into aaabaaab
       
       Characters allowed as input include digits, small English letters and brackets [ ].
     */
    static void Main(string[] args)
    {
      var testStrings = new[] { "2[3[a]b]", "10[a]", "3[abc]4[ab]c"," ","3[e" };
      foreach (var s in testStrings)
      {
        var test = s.ToCharArray();

        CompressedString(test);
      }
      Console.ReadLine();
    }

    private static void CompressedString(char[] arr)
    {
      if (arr.Length == 0 || arr.Length < 4)
      {
        return;
      }
      var rotNumber = getRootNumber(arr);
      if (rotNumber == 0)
      {
        return;
      }

      var index = GetStartIndex(arr);
      var tempRes = CompressedString(arr, index, "", rotNumber, 0); ;
      var lastChars = AttachLastCharacters(arr);
      var result = tempRes + lastChars;
      Console.WriteLine("Result from Decompress :" + result);
    }

    private static string AttachLastCharacters(char[] arr)
    {
      var endChars = "";
      for (int i = arr.Length - 1; i > 0; i--)
      {
        if (char.IsLetter(arr[i]))
        {
          endChars += arr[i];
        }

        if (arr[i] == ']')
        {
          break;
        }
      }
      return endChars;
    }

    private static int GetStartIndex(char[] arr)
    {
      int index = 0;
      for (int i = 0; i < arr.Length; i++)
      {
        index = i;
        if (arr[i] == '[')
        {
          break;
        }

      }
      return index + 1;
    }

    private static int getRootNumber(char[] arr)
    {
      string rootNumber = "";
      int rotNum = 0;
      int j = 0;
      while (arr[j] != '[' && Char.IsNumber(arr[j]))
      {
        rootNumber += arr[j];
        j++;
      }
      rotNum = getNumber(rootNumber);
      return rotNum;
    }

    private static string CompressedString(char[] arr, int index, string result, int rootNumber, int prevRootNum)
    {
      Console.WriteLine($"current index: {index} currentresult :{result}  rootNumber:{rootNumber}  PrevNumber:{prevRootNum}");
      var currentPeel = "";

      if (index == arr.Length)
      {
        return result;
      }
      var curr = index;
      var temp = "";
      // Collect
      if (char.IsLetter(arr[curr]))
      {
        while (arr[curr] != ']')
        {
          temp += arr[curr];
          if (curr == arr.Length)
          {
            break;
          }
          curr++;
        }
        curr++;
        for (int k = 0; k < rootNumber; k++)
        {
          currentPeel += temp;

        }
        result = currentPeel;

        var restPeel = "";
        while (curr < arr.Length - 1)
        {
          if (char.IsLetter(arr[curr]))
          {
            restPeel += arr[curr];
          }
          if (char.IsNumber(arr[curr]))
          {
            break;
          }
          curr++;
        }

        result += restPeel;

        if (prevRootNum > 0)
        {
          for (int k = 0; k < prevRootNum / 2; k++)
          {
            result += result;
          }
        }



        if (curr > arr.Length - 1) return result;

        // Next--------------------------------------------------
        if (char.IsDigit(arr[curr]))
        {
          var nextChar = curr + 2;
          var currRootNumber = getNumber(arr[curr].ToString());
          result += CompressedString(arr, nextChar, result, currRootNumber, 0);
        }
      }
      else if (char.IsDigit(arr[curr]))
      {
        var currentPrevNumber = rootNumber;
        var nextChar = curr + 2;
        var currRootNumber = getNumber(arr[curr].ToString());
        result += CompressedString(arr, nextChar, result, currRootNumber, currentPrevNumber);
      }

      return result;
    }

    private static int getNumber(string c)
    {
      if (int.TryParse(c, out var number))
        return number;
      return 0;
    }
  }

}

