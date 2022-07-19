using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ITMO.Scripts;
using UnityEngine;
using Logger = ITMO.Scripts.Logger;

namespace ITMO.Scripts
{
    public static class Reference
    {
        public static int ID = -1;
        public static Stopwatch Stopwatch = new Stopwatch();
        public static int TrackersTick = 10;
    }

    public static class Extentions
    {
        public static int Round(this int i, int roundTo)
        {
            return ((int)Math.Round(i / (double)roundTo)) * roundTo;
        }
    }
}
