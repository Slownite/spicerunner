﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Framework
{
    public class StringHelpers
    {
        public static string Format(string str, int chars)
        {
            var length = chars - str.Length;

            for (int i = 0; i < length; i++)
            {
                str = "0" + str;
            }

            return str;
        }
    }
}