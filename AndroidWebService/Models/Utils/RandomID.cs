﻿using System;
using System.Text;

namespace AndroidWebService.Models.Utils
{
    public class RandomID
    {
        private static Random random = new Random();
        private static readonly string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string Get(int size = 5)
        {
            StringBuilder builder = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                int randomIndex = random.Next(characters.Length);
                char randomChar = characters[randomIndex];
                builder.Append(randomChar);
            }

            return builder.ToString();
        }
    }
}