using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts.EmotionsScene
{
    [RequireComponent(typeof(RawImage))]
    public class Image : MonoBehaviour
    {
        [SerializeField] private RawImage image;
        [SerializeField] private Texture[] images;
        private int pointer = -1;
        private Logger logger;
        private int counter = -1;

        private void Awake()
        {
            SRanipal_Eye_Framework.Instance.EnableEye = true;
            SRanipal_Lip_Framework.Instance.EnableLip = true;
            logger = new Logger("_datasetEmotions");
            var sb = new StringBuilder();
            sb.Append("timestamp|");
            foreach (var value in Enum.GetNames(typeof(EyeShape_v2))) sb.Append($"{value}|");
            sb.Append("l_pupil_diameter|r_pupil_diameter|");
            foreach (var value in Enum.GetNames(typeof(LipShape_v2)))
            {
                if (value.Equals("Max") || value.Equals("None")) continue;
                sb.Append($"{value}|");
            }

            sb.Append("emotion");
            logger.AddInfo(sb.ToString());
            logger.WriteInfo();
        }

        private void FixedUpdate()
        {
            if (logger is null) return;
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;
            if (SRanipal_Lip_Framework.Status != SRanipal_Lip_Framework.FrameworkStatus.WORKING) return;

            SRanipal_Eye_v2.GetEyeWeightings(out var eyeShapes);
            SRanipal_Lip_v2.GetLipWeightings(out var lipShapes);

            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("HH:mm:ss.fff")).Append("|");
            foreach (var value in eyeShapes.Values) sb.Append($"{value}|");
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.LEFT, out var lDiam);
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.RIGHT, out var rDiam);
            sb.Append($"{lDiam}|{rDiam}|");
            foreach (var value in lipShapes.Where(value => value.Key != LipShape_v2.Max && value.Key != LipShape_v2.None))
                sb.Append($"{value.Value}|");

            sb.Append(pointer == -1 ? "Нейтральная" : $"{images[pointer].name}");
            logger.AddInfo(sb.ToString());

            if (++counter % 20 != 0) return;

            logger.WriteInfo();
        }

        public void NextImage()
        {
            if (pointer == images.Length - 1) return;
            image.texture = images[++pointer];
            Debug.Log(images[pointer].name);
        }

        public void PrevImage()
        {
            if (pointer <= 0) return;
            image.texture = images[--pointer];
        }
    }
}