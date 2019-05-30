using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtDb
{
    public static class EncoderUtilities
    {
        public static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            string stringEncoded = Convert.ToBase64String(plainTextBytes);
            return stringEncoded;
        }

        public static string Base64Decoded(string base64String)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64String);
            string plainText = Encoding.UTF8.GetString(base64EncodedBytes);
            return plainText;
        }
    }
}