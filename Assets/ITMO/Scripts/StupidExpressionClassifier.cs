using System.Diagnostics;
using TMPro;
using UnityEngine;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts
{
    public class StupidExpressionClassifier : MonoBehaviour
    {
        public static Logger Logger;

        public static float Quality = 0.3f;
        public TMP_Text text;

        public SkinnedMeshRenderer meshRenderer;

        private Emotions currentEmotion = Emotions.Neutral;
        private Emotions prevEmotion = Emotions.Neutral;

        private readonly Stopwatch stopwatch = new Stopwatch();

        private void Start() => Server.SendEvent.AddListener(EventHandler);

        private void EventHandler()
        {
            if (Logger == null) return;
            Logger.AddInfo(currentEmotion.ToString());
            Logger.WriteInfo();
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
            if (!Server.ServerConnected || FaceTracker.Shapes == null || EyeTracker.Shapes == null)
            {
                if (text.enabled) text.enabled = false;
                return;
            }

            if (!text.enabled) text.enabled = true;

            /*if (meshRenderer.GetBlendShapeWeight(27) / 100 > q &&
                meshRenderer.GetBlendShapeWeight(33) / 100 > q)
            {
                currentEmotion = Emotions.Surprise;
                /* Страх
                 * 1+2+4+5*+20*+25, 26, или 27
                 * 1+2+4+5*+25, 26, или 27
                 #1#
                if ((!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds < 1000) &&
                    (meshRenderer.GetBlendShapeWeight(3) / 100 < q || meshRenderer.GetBlendShapeWeight(14) / 100 < q ||
                     meshRenderer.GetBlendShapeWeight(15) / 100 < q)) return;
                stopwatch.Stop();
                stopwatch.Reset();
                currentEmotion = Emotions.Fear;
            }
            else if (meshRenderer.GetBlendShapeWeight(12) / 100 > q &&
                     meshRenderer.GetBlendShapeWeight(13) / 100 > q)
                currentEmotion = Emotions.Joy;
            else if (meshRenderer.GetBlendShapeWeight(14) / 100 > q &&
                     meshRenderer.GetBlendShapeWeight(15) / 100 > q)
            {
                currentEmotion = Emotions.Sadness;
            }
            else if (meshRenderer.GetBlendShapeWeight(38) / 100 > q &&
                     meshRenderer.GetBlendShapeWeight(19) / 100 > q &&
                     meshRenderer.GetBlendShapeWeight(20) / 100 > q)
                currentEmotion = Emotions.Angry;
            else currentEmotion = Emotions.Neutral;*/

            if (EyeTracker.Shapes[EyeShape_v2.Eye_Left_Wide] > Quality &&
                EyeTracker.Shapes[EyeShape_v2.Eye_Right_Wide] > Quality)
            {
                if (currentEmotion != Emotions.Surprise && currentEmotion != Emotions.Fear)
                    stopwatch.Start();
                currentEmotion = Emotions.Surprise;
                if ((!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds < 1000) &&
                    (FaceTracker.Shapes[LipShape_v2.Jaw_Open] < Quality ||
                     FaceTracker.Shapes[LipShape_v2.Mouth_Sad_Left] < Quality ||
                     FaceTracker.Shapes[LipShape_v2.Mouth_Sad_Right] < Quality)) return;
                stopwatch.Stop();
                stopwatch.Reset();
                currentEmotion = Emotions.Fear;
            }
            else if (FaceTracker.Shapes[LipShape_v2.Mouth_Smile_Left] > Quality &&
                     FaceTracker.Shapes[LipShape_v2.Mouth_Smile_Right] > Quality)
            {
                currentEmotion = Emotions.Joy;
            }
            else if (FaceTracker.Shapes[LipShape_v2.Mouth_Sad_Left] > Quality &&
                     FaceTracker.Shapes[LipShape_v2.Mouth_Sad_Right] > Quality)
            {
                currentEmotion = Emotions.Sadness;
            }
            else if (EyeTracker.Shapes[EyeShape_v2.Eye_Frown] > Quality &&
                     FaceTracker.Shapes[LipShape_v2.Mouth_Upper_UpLeft] > Quality &&
                     FaceTracker.Shapes[LipShape_v2.Mouth_Upper_UpRight] > Quality)
            {
                currentEmotion = Emotions.Angry;
            }
            else
            {
                currentEmotion = Emotions.Neutral;
            }

            text.text = currentEmotion.ToString();

            if (currentEmotion == prevEmotion) return;
            if (Logger is null) return;
            Logger.AddInfo(currentEmotion.ToString());
            Logger.WriteInfo();
            prevEmotion = currentEmotion;

            /*if (EyeTracker.Shapes[EyeShape_v2.Eye_Left_Wide] > 0.2 &&
                EyeTracker.Shapes[EyeShape_v2.Eye_Right_Wide] > 0.2) text.text = "Surprise";
            else text.text = "Neutral";*/
        }

        private enum Emotions
        {
            Surprise,
            Fear,
            Joy,
            Sadness,
            Angry,
            Neutral
        }
    }
}