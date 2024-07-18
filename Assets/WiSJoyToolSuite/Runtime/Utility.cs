using System;
using System.Text;

namespace WiSdom
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
    }
}