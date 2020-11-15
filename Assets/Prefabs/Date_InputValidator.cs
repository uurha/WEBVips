using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Date_InputValidator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        Debug.Log($"Text = {text}; pos = {pos}; chr = {ch}");
        // If the typed character is a number, insert it into the text argument at the text insertion position (pos argument)
        if (char.IsNumber(ch) && text.Length < 10)
        {
            // Insert the character at the given position if we're working in the Unity Editor
            #if UNITY_EDITOR
            text = text.Insert(pos, ch.ToString());
            #endif
            // Increment the insertion point by 1
            pos++;
            // Convert the text to a number
            long number = ConvertToInt(text);
            // If the number is greater than 9999999999 / 11 characters or longer
            // Then reparse the number string but trimmed to 10 characters
            if (number > 9999999999)
            {
                number = long.Parse(number.ToString().Substring(0, 10));
            }

            // Format the string incrementally by the character count of our number
            // Format: XX/XX/XXXX
            switch (number.ToString().Length)
            {
                case 1:
                    text = $"{number:#}";
                    break;
                case 2:
                    text = $"{number:##/}";
                    break;
                case 3:
                    text = $"{number:##/#}";
                    break;
                case 4:
                    text = $"{number:##/##/}";
                    break;
                case 5:
                    text = $"{number:##/##/#}";
                    break;
                case 6:
                    text = $"{number:##/##/##}";
                    break;
                case 7:
                    text = $"{number:##/##/###}";
                    break;
                case 8:
                    text = $"{number:##/##/####}";
                    break;
                default:
                    return '\0';
            }

            int[] checkValue   = {31, 12, 9999};
            var   split        = text.Split('/').Where(x => x != string.Empty).ToArray();
            var   comparePairs = split.Zip(checkValue, (v, s) => new {str = v, value = s});
            foreach (var pair in comparePairs)
            {
                if (!int.TryParse(pair.str, out var value)) continue;
                if (value > pair.value)
                {
                    text = text.Remove(text.Length - 2, 2);
                }
            }

            // Increment the text insertion position by 1 in the following positions
            // XX
            // XX/XX
            if (pos == 2 || pos == 5)
            {
                pos++;
            }
            return ch;
        }
        // If the character is not a number, return null
        else
        {
            return '\0';
        }
    }

    /// <summary>
    /// Converts a string input into a long integer
    /// </summary>
    /// <param name="inputText">Input text</param>
    /// <returns>Returns the number of the text</returns>
    private long ConvertToInt(string inputText)
    {
        // Create a string builder to cache our number  characters
        System.Text.StringBuilder number = new System.Text.StringBuilder();
        // Iterate through each character in the input text
        // If the character found is a digit, append the digit to our string builder
        foreach (char character in inputText)
        {
            if (char.IsDigit(character))
            {
                number.Append(character);
            }
        }
        // Return the numbered string
        return long.Parse(number.ToString());
    }
}