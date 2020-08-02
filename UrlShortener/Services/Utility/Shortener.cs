using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Utility
{
    public class Shortener
    {
        private static readonly string alphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string alphaLower = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string numbers = "0123456789";

        public static string Shorten ()
        {
            // There is potentially a more secure algorithm that could create this. But for the small amount of URLs needed this should be okay
            Random r = new Random();

            // Choose 2 random capital letter, 2 random lower, 2 numbers
            List<char> chars = new List<char> {
                alphaUpper[r.Next(0, alphaUpper.Length)],
                alphaUpper[r.Next(0, alphaUpper.Length)],
                alphaLower[r.Next(0, alphaUpper.Length)],
                alphaLower[r.Next(0, alphaUpper.Length)],
                numbers[r.Next(0, numbers.Length)],
                numbers[r.Next(0, numbers.Length)]
            };

            // Now jumble the array into a new string
            StringBuilder sb = new StringBuilder();
            while (chars.Count != 0)
            {
                char character = chars[r.Next(0, chars.Count)];
                sb.Append(character);
                chars.Remove(character);
            }

            return sb.ToString();
        }
    }
}
