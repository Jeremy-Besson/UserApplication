using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalticAmadeusTask.Helpers
{
    public static class RandomGeneratorHelper
    {
        private static Random randomer = new Random(Guid.NewGuid().GetHashCode() + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond);

        public static Color RandomColor()
        {
            return Color.FromArgb(RandomGeneratorHelper.RandomNumber(0, 255), RandomGeneratorHelper.RandomNumber(0, 255), RandomGeneratorHelper.RandomNumber(0, 255));
        }

        public static String RandomColorHtml()
        {
            return ColorToHexString(RandomColor());
        }

        public static string ColorToHexString(Color color)
        {
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            byte[] bytes = new byte[3];
            bytes[0] = color.R;
            bytes[1] = color.G;
            bytes[2] = color.B;
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return "#" + new string(chars);
            //return "yellow";
        }

        public static String RandomAlphaNumNotCapitalized(int length)
        {
            return "abcdefghijklmnopqrstuvwxyz0123456789".random(length);
        }

        public static String RandomAlphaNum(int length)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".random(length);
        }

        public static String RandomAlphaCapitalized(int length)
        {
            if (length < 1)
            {
                length = 1;
            }
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".random(1) + "abcdefghijklmnopqrstuvwxyz".random(length - 1);
        }

        public static String randomText()
        {
            return RandomAlpha(32);
        }

        public static String RandomAlpha(int length)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".random(length);
        }

        public static String random(this string chars, int length = 8)
        {
            var randomString = new StringBuilder();

            for (int i = 0; i < length; i++)
                randomString.Append(chars[randomer.Next(chars.Length)]);

            return randomString.ToString();
        }

        public static int RandomNumber()
        {
            return randomer.Next(100);
        }

        public static int RandomNumber(int min, int max)
        {
            return randomer.Next(max + 1 - min) + min;
        }

        public static string RandomEmail()
        {
            var email = new StringBuilder();
            email.Append(RandomAlpha(10));
            email.Append("@");
            email.Append(RandomAlpha(8));
            email.Append(".");
            email.Append(RandomAlpha(5));
            return email.ToString();
        }

        public static string LoremIpsum(int minWords, int maxWords, int minSentences, int maxSentences, int numLines)
        {
            var words = new[] { "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat" };

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences;
            int numWords = rand.Next(maxWords - minWords) + minWords;

            var sb = new StringBuilder();
            for (int p = 0; p < numLines; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { sb.Append(" "); }
                        string word = words[rand.Next(words.Length)];
                        if (w == 0) { word = word.Substring(0, 1).Trim().ToUpper() + word.Substring(1); }
                        sb.Append(word);
                    }
                    sb.Append(". ");
                }
                if (p < numLines - 1) sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}

