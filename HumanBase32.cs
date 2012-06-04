using System;
using System.Collections.Generic;

/// <summary>
/// Human targeted Base32 encoding, inspired by Douglas Crockford (http://www.crockford.com/wrmg/base32.html)
/// The intent is to achieve decent compresssion (a 6 digit base32 numeral ~ a 10 digit base10 numeral)
/// while remaining easily legible and communicable.  The latter is achieved by only using lowercase digits that 
/// can't be easily misread: o, 0, and O are all interpreted as 0, and I, l, 1 and i are all interpreted as 1, etc
/// Note that the checksum provided by Crockford is not implemented
/// </summary>
public class HumanBase32
{
  #region Private Members
  public const int Base = 32;
  private static Dictionary<char, int> _decodingDictionary = new Dictionary<char, int>(Base);
  private static Dictionary<int, char> _encodingDictionary = new Dictionary<int, char>(Base);

  /// <summary>
  /// Static constructor to fill the dictionaries
  /// </summary>
  static HumanBase32()
  {
    FillDecodingDictionary();
    FillEncodingDictionary();
  }

  /// <summary>
  /// Fills the encoding dictionary
  /// </summary>
  private static void FillEncodingDictionary()
  {
    _encodingDictionary.Add(0, '0');
    _encodingDictionary.Add(1, '1');
    _encodingDictionary.Add(2, '2');
    _encodingDictionary.Add(3, '3');
    _encodingDictionary.Add(4, '4');
    _encodingDictionary.Add(5, '5');
    _encodingDictionary.Add(6, '6');
    _encodingDictionary.Add(7, '7');
    _encodingDictionary.Add(8, '8');
    _encodingDictionary.Add(9, '9');
    _encodingDictionary.Add(10, 'a');
    _encodingDictionary.Add(11, 'b');
    _encodingDictionary.Add(12, 'c');
    _encodingDictionary.Add(13, 'd');
    _encodingDictionary.Add(14, 'e');
    _encodingDictionary.Add(15, 'f');
    _encodingDictionary.Add(16, 'g');
    _encodingDictionary.Add(17, 'h');
    _encodingDictionary.Add(18, 'j');
    _encodingDictionary.Add(19, 'k');
    _encodingDictionary.Add(20, 'm');
    _encodingDictionary.Add(21, 'n');
    _encodingDictionary.Add(22, 'p');
    _encodingDictionary.Add(23, 'q');
    _encodingDictionary.Add(24, 'r');
    _encodingDictionary.Add(25, 's');
    _encodingDictionary.Add(26, 't');
    _encodingDictionary.Add(27, 'v');
    _encodingDictionary.Add(28, 'w');
    _encodingDictionary.Add(29, 'x');
    _encodingDictionary.Add(30, 'y');
    _encodingDictionary.Add(31, 'z');
  }

  /// <summary>
  /// Fills the decoding dictionary
  /// </summary>
  private static void FillDecodingDictionary()
  {
    // o, O, and 0 should all = 0
    _decodingDictionary.Add('0', 0);
    _decodingDictionary.Add('o', 0);
    //i, I, l, L, and 1 should all = 1
    _decodingDictionary.Add('1', 1);
    _decodingDictionary.Add('i', 1);
    _decodingDictionary.Add('l', 1);
    _decodingDictionary.Add('2', 2);
    _decodingDictionary.Add('3', 3);
    _decodingDictionary.Add('4', 4);
    _decodingDictionary.Add('5', 5);
    _decodingDictionary.Add('6', 6);
    _decodingDictionary.Add('7', 7);
    _decodingDictionary.Add('8', 8);
    _decodingDictionary.Add('9', 9);
    _decodingDictionary.Add('a', 10);
    _decodingDictionary.Add('b', 11);
    _decodingDictionary.Add('c', 12);
    _decodingDictionary.Add('d', 13);
    _decodingDictionary.Add('e', 14);
    _decodingDictionary.Add('f', 15);
    _decodingDictionary.Add('g', 16);
    _decodingDictionary.Add('h', 17);
    _decodingDictionary.Add('j', 18);
    _decodingDictionary.Add('k', 19);
    _decodingDictionary.Add('m', 20);
    _decodingDictionary.Add('n', 21);
    _decodingDictionary.Add('p', 22);
    _decodingDictionary.Add('q', 23);
    _decodingDictionary.Add('r', 24);
    _decodingDictionary.Add('s', 25);
    _decodingDictionary.Add('t', 26);
    //u is excluded to prevent accidental 'f_ck's.
    _decodingDictionary.Add('v', 27);
    _decodingDictionary.Add('w', 28);
    _decodingDictionary.Add('x', 29);
    _decodingDictionary.Add('y', 30);
    _decodingDictionary.Add('z', 31);
  }

  /// <summary>
  /// Get the Base32 encode length for a given number
  /// </summary>
  /// <param name="number">Number to encode</param>
  /// <returns>Length of Base32 symbol</returns>
  private static int GetSymbolLength(long number)
  {
    //Length = Floor(Log(number, base32)) + 1
    return Convert.ToInt32(Math.Floor(Convert.ToDecimal(Math.Log(number, Base))) + 1);
  }

  /// <summary>
  /// Get the positional multiplier, where position is counted from right, starting with 1
  /// </summary>
  /// <param name="i">1-based, right-to-left position</param>
  /// <returns>Base32 positional multiplier</returns>
  private static long GetPositionalMultiplier(int i)
  {
    return Convert.ToInt64(Math.Pow(Base, i - 1));
  }

  #endregion (Private Members)

  /// <summary>
  /// Encodes a long integer to a HumanBase32 string.
  /// </summary>
  /// <param name="number">The Base10 number to encode</param>
  /// <returns>The Base32 encoded symbol string</returns>
  public static string Encode(long number)
  {
    string symbol = "";
    try
    {
      number = Math.Abs(number);
      int symbolLength = GetSymbolLength(number);
      char[] symbolArray = new char[symbolLength];
      //start with the highest symbol digit, and proceed down
      for (int i = symbolLength; i > 0; i--)
      {
        //get the positional multiplier: 
        //(e.g. in base10, the number 3 in 321 has a positional multiplier of 10^(3-1) = 100)
        long positionalMultiplier = GetPositionalMultiplier(i);
        //get the symbol digit by Div'ing the number by the positional base
        //(e.g. in 321(base10), the first digit is 321 Div 100 = 3
        int symbolDigit = Convert.ToInt32(Math.DivRem(number, positionalMultiplier, out number));
        //then convert to HumanBase32 symbol
        symbolArray[symbolLength - i] = _encodingDictionary[symbolDigit];
      }
      symbol = new String(symbolArray);
    }
    catch
    {
      symbol = "(NAN)";
    }
    return symbol;
  }

  /// <summary>
  /// Decodes the HumanBase32 symbol to a long integer
  /// </summary>
  /// <param name="symbol">HumanBase32 symbol string</param>
  /// <returns>The long integer result, or -1 if an invalid symbol</returns>
  public static long Decode(string symbol)
  {
    long number = 0;
    try
    {
      symbol = symbol.Trim().ToLower();
      int symbolLength = symbol.Length;
      //loop through the characters from left to right
      for (int i = symbolLength; i > 0; i--)
      {
        //Get the character
        char symbolChar = symbol[symbolLength - i];
        //Decode the digit
        int symbolDigit = _decodingDictionary[symbolChar];
        //Get the positional multiplier for the digit
        long positionalMultiplier = GetPositionalMultiplier(i);
        //Add to the result
        number += symbolDigit * positionalMultiplier;
      }
    }
    catch
    {
      number = -1;
    }
    return number;
  }

}

