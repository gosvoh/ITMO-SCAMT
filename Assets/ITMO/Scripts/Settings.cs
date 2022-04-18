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
            PlayerPrefs.Save();
        }
        
        private static void LoadSettings()
        {
            if (PlayerPrefs.HasKey("TipGazeCounter"))
                TaskPanel.TipGazeCounter = PlayerPrefs.GetInt("TipGazeCounter");
            
            if (PlayerPrefs.HasKey("Tip1TimeSeconds"))
                TaskPanel.Tip1TimeSeconds = PlayerPrefs.GetInt("Tip1TimeSeconds");
            
            if (PlayerPrefs.HasKey("Tip2TimeSeconds"))
                TaskPanel.Tip2TimeSeconds = PlayerPrefs.GetInt("Tip2TimeSeconds");
            
            if (PlayerPrefs.HasKey("Tip3TimeSeconds"))
                TaskPanel.Tip3TimeSeconds = PlayerPrefs.GetInt("Tip3TimeSeconds");
        }
    }
}