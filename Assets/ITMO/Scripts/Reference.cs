using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Logger = ITMO.Scripts.Logger;

public class Reference : MonoBehaviour
{
    public static int counter;
    public static int ID = -1;
    public static Logger EyeLogger;
    public static Logger FaceLogger;
    public static Stopwatch Stopwatch = new Stopwatch();
}
