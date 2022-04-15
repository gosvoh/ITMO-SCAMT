using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts
{
    public class StupidExpressionClassifier : MonoBehaviour
    {
        public static float Quality = 0.3f;

        public static Emotions CurrentEmotion = Emotions.Neutral;
        private Emotions _prevEmotion = Emotions.Neutral;
        private Logger _logger;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private void Awake()
        {
            Server.SendEvent.AddListener(EventHandler);
            Server.ConnectionEvent.AddListener(ConnectionHandler);
        }

        private void EventHandler()
        {
            if (!Server.ServerConnected) return;

            _logger.AddInfo($"{DateTime.Now:HH:mm:ss.fff}|{CurrentEmotion.ToString()}");
            _logger.WriteInfo();
        }

        private void ConnectionHandler()
        {
            _logger = new Logger("_emotions");
            _logger.AddInfo("timestamp|emotion");
        }

        /* Удивление
         * 1+2+5B+26
         * 1+2+5B+27
         * 
         * 1+2+5B
         * 1+2+26
         * 1+2+27
         * 5B+26
         * 5B+27
         *
         * Страх
         * 1+2+4+5*+20*+25, 26, или 27
         * 1+2+4+5*+25, 26, или 27
         * 
         * 1+2+4+5*+L или R20*+25, 26, или 27
         * 1+2+4+5*
         * 1+2+5*, с/без 25, 26, 27
         * 5*+20* с/без 25, 26, 27
         *
         * Радость
         * 6+12*
         * 12C/D
         *
         * Печаль
         * 1+4+11+15B с/без 54+64
         * 1+4+15* с/без 54+64
         * 6+15* с/без 54+64
         * 
         * 1+4+11 с/без 54+64
         * 1+4+15B с/без 54+64
         * 1+4+15B+17 с/без 54+64
         * 11+15B с/без 54+64
         * 11+17
         *
         * Гнев
         * 4+5*+7+10*+22+23+25, 26
         * 4+5*+7+10*+23+25, 26
         * 4+5*+7+23+25, 26
         * 4+5*+7+17+23
         * 4+5*+7+17+24
         * 4+5*+7+23
         * 4+5*+7+24
         * 
         * Любые из прототипов без любой из следующих ДЕ: 4, 5, 7 или 10.
         */
        private void FixedUpdate()
        {
            if (!Server.ServerConnected || FaceTracker.Shapes is null || EyeTracker.Shapes is null) return;

            if (EyeTracker.Shapes[EyeShape_v2.Eye_Left_Wide] > Quality &&
                EyeTracker.Shapes[EyeShape_v2.Eye_Right_Wide] > Quality)
            {
                if (CurrentEmotion != Emotions.Surprise && CurrentEmotion != Emotions.Fear)
                    _stopwatch.Start();
                CurrentEmotion = Emotions.Surprise;
                if ((!_stopwatch.IsRunning || _stopwatch.ElapsedMilliseconds < 1000) &&
                    (FaceTracker.Shapes[LipShape_v2.Jaw_Open] < Quality ||
                     FaceTracker.Shapes[LipShape_v2.Mouth_Sad_Left] < Quality ||
                     FaceTracker.Shapes[LipShape_v2.Mouth_Sad_Right] < Quality)) return;
                _stopwatch.Stop();
                _stopwatch.Reset();
                CurrentEmotion = Emotions.Fear;
            }
            else if (FaceTracker.Shapes[LipShape_v2.Mouth_Smile_Left] > Quality &&
                     FaceTracker.Shapes[LipShape_v2.Mouth_Smile_Right] > Quality)
            {
                CurrentEmotion = Emotions.Joy;
            }
            else if (FaceTracker.Shapes[LipShape_v2.Mouth_Sad_Left] > Quality &&
                     FaceTracker.Shapes[LipShape_v2.Mouth_Sad_Right] > Quality)
            {
                CurrentEmotion = Emotions.Sadness;
            }
            else if (EyeTracker.Shapes[EyeShape_v2.Eye_Frown] > Quality &&
                     FaceTracker.Shapes[LipShape_v2.Mouth_Upper_UpLeft] > Quality &&
                     FaceTracker.Shapes[LipShape_v2.Mouth_Upper_UpRight] > Quality)
            {
                CurrentEmotion = Emotions.Angry;
            }
            else
            {
                CurrentEmotion = Emotions.Neutral;
            }

            if (CurrentEmotion == _prevEmotion) return;
            if (_logger is null) return;
            _logger.AddInfo($"{DateTime.Now:HH:mm:ss.fff}|{CurrentEmotion.ToString()}");
            _logger.WriteInfo();
            _prevEmotion = CurrentEmotion;
        }
    }

    public enum Emotions
    {
        Surprise,
        Fear,
        Joy,
        Sadness,
        Angry,
        Neutral
    }
}