using System;
using UnityEngine;

namespace ITMO.Scripts
{
    public class Settings : MonoBehaviour
    {
        private void OnApplicationQuit() => SaveSettings();

        private void Awake() => LoadSettings();

        private static void SaveSettings()
        {
            PlayerPrefs.SetInt("TipGazeCounter", TaskPanel.TipGazeCounter);
            PlayerPrefs.SetInt("Tip1TimeSeconds", TaskPanel.Tip1TimeSeconds);
            PlayerPrefs.SetInt("Tip2TimeSeconds", TaskPanel.Tip2TimeSeconds);
            PlayerPrefs.SetInt("Tip3TimeSeconds", TaskPanel.Tip3TimeSeconds);
            PlayerPrefs.SetInt("Reference.TrackersTick", Reference.TrackersTick);
            PlayerPrefs.Save();
        }

        private static void LoadSettings()
        {
            TaskPanel.TipGazeCounter = PlayerPrefs.GetInt("TipGazeCounter", 250);
            TaskPanel.Tip1TimeSeconds = PlayerPrefs.GetInt("Tip1TimeSeconds", 30);
            TaskPanel.Tip2TimeSeconds = PlayerPrefs.GetInt("Tip2TimeSeconds", 50);
            TaskPanel.Tip3TimeSeconds = PlayerPrefs.GetInt("Tip3TimeSeconds", 70);
            Reference.TrackersTick = PlayerPrefs.GetInt("Reference.TrackersTick", 10);
        }
    }
}