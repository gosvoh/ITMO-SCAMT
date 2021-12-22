using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR;
using ViveSR.anipal;
using ViveSR.anipal.Eye;
using System.IO;

public class EyesLogger : MonoBehaviour {

    private static string fileName = "eyes.txt";
    private static StreamWriter sw;

    private void Awake()
    {
        sw = new StreamWriter(fileName, true, System.Text.Encoding.UTF8);
    }

    private void OnApplicationQuit()
    {
        sw.Close();
    }

    public static void Log(string text)
    {
        sw.WriteLine(text);
    }
}
