using System;
using ITMO.Scripts.ML;
using UnityEngine;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts
{
    public class MLObject : MonoBehaviour
    {
        public static string CurrentEmotion = string.Empty;
        private Logger _logger;
        private string _prevEmotion = string.Empty;
        private void Awake()
        {
            if (!SRanipal_Eye_Framework.Instance.EnableEye) enabled = false;
            Server.SendEvent.AddListener(EventHandler);
            Server.ConnectionEvent.AddListener(ConnectionHandler);
        }

        private void ConnectionHandler()
        {
            _logger = new Logger("_emotions");
            _logger.AddInfo("timestamp|emotion");
        }

        private void EventHandler()
        {
            if (!Server.ServerConnected) return;

            _logger.AddInfo(
                $"Level - {Level.CurrentLevelName};");
            _logger.WriteInfo();
        }

        private void FixedUpdate()
        {
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING ||
                SRanipal_Lip_Framework.Status != SRanipal_Lip_Framework.FrameworkStatus.WORKING)
            {
                CurrentEmotion = "Not working";
                return;
            }
            
            var data = ModelInput.Transform(EyeTracker.Shapes, FaceTracker.Shapes);
            CurrentEmotion = ML.ML.Predict(data).PredictedLabel;
            if (_prevEmotion.Equals(CurrentEmotion)) return;
            _prevEmotion = CurrentEmotion;
            _logger.AddInfo($"{DateTime.Now:HH:mm:ss.fff}|{CurrentEmotion}");
            _logger.WriteInfo();
        }
    }
}