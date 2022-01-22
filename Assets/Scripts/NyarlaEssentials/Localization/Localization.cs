using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NyarlaEssentials.Localization
{
    public static class Localization
    {
        private static int language = 1;

        public static string Translate(string[] variants)
        {
            if (variants.Length == 0)
                throw new NullReferenceException();
            if (language < variants.Length)
                return variants[language];
            return variants[0];
        }
    }
}