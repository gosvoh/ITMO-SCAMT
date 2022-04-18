using System;
using System.Collections.Generic;
using System.Globalization;
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
        private int _pointer = -1;
        private Logger _logger;
        private int _counter = -1;

        private void Awake()
        {
            SRanipal_Eye_Framework.Instance.EnableEye = true;
            SRanipal_Lip_Framework.Instance.EnableLip = true;
            _logger = new Logger("_datasetEmotions");
            var sb = new StringBuilder();
            foreach (var value in Enum.GetNames(typeof(EyeShape_v2))) sb.Append(string.Format(CultureInfo.InvariantCulture, "{0},", value));
            sb.Append("l_pupil_diameter,r_pupil_diameter,");
            foreach (var value in Enum.GetNames(typeof(LipShape_v2)))
            {
                if (value.Equals("Max") || value.Equals("None")) continue;
                sb.Append(string.Format(CultureInfo.InvariantCulture, "{0},", value));
            }

            sb.Append("emotion");
            _logger.AddInfo(sb.ToString());
            _logger.WriteInfo();
        }

        private void FixedUpdate()
        {
            if (_logger is null) return;
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;
            if (SRanipal_Lip_Framework.Status != SRanipal_Lip_Framework.FrameworkStatus.WORKING) return;

            SRanipal_Eye_v2.GetEyeWeightings(out var eyeShapes);
            SRanipal_Lip_v2.GetLipWeightings(out var lipShapes);

            var sb = new StringBuilder();
            foreach (var value in eyeShapes.Values) sb.Append(string.Format(CultureInfo.InvariantCulture, "{0},", value));
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.LEFT, out var lDiam);
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.RIGHT, out var rDiam);
            sb.Append(string.Format(CultureInfo.InvariantCulture, "{0},{1},", lDiam, rDiam));
            foreach (var value in lipShapes.Where(value => value.Key != LipShape_v2.Max && value.Key != LipShape_v2.None))
                sb.Append(string.Format(CultureInfo.InvariantCulture, "{0},", value.Value));

            sb.Append(_pointer == -1 ? "Нейтральная" : $"{images[_pointer].name}");
            _logger.AddInfo(sb.ToString());

            if (++_counter % 20 != 0) return;

            _logger.WriteInfo();
        }

        public void NextImage()
        {
            if (_pointer == images.Length - 1) return;
            image.texture = images[++_pointer];
            Debug.Log(images[_pointer].name);
        }

        public void PrevImage()
        {
            if (_pointer <= 0) return;
            image.texture = images[--_pointer];
        }
    }
}