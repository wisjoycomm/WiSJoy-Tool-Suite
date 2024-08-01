using System;
using System.Collections.Generic;
using System.Text;
namespace WiSJoy.Extend
{
    public static class Utility
    {
        // Helper method to generate a more secure file name
        public static string EncodeStringToBase64(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        // Convert a Base64 string back to a normal string
        public static string DecodeBase64ToString(string input)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }

        public static byte[] ConvertStringToByte(string input)
        {
            return Convert.FromBase64String(input);
        }
        public static string ConvertByteToString(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        // Extension method to shuffle a List
        public static void Shuffle<T>(this List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        // Extension method to random a List
        public static T Random<T>(this List<T> list)
        {
            Random rng = new Random();
            return list[rng.Next(list.Count)];
        }

        // Extension method to shuffle an array
        public static void Shuffle<T>(this T[] array)
        {
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }
        public static T Random<T>(this T[] array)
        {
            Random rng = new Random();
            return array[rng.Next(array.Length)];
        }
    }
}