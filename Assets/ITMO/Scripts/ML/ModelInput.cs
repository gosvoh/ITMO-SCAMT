using System;
using System.Collections.Generic;
using Microsoft.ML.Data;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts.ML
{
    public class ModelInput
    {
        [ColumnName(@"Eye_Left_Blink")] public float Eye_Left_Blink { get; set; }
        [ColumnName(@"Eye_Left_Wide")] public float Eye_Left_Wide { get; set; }
        [ColumnName(@"Eye_Left_Right")] public float Eye_Left_Right { get; set; }
        [ColumnName(@"Eye_Left_Left")] public float Eye_Left_Left { get; set; }
        [ColumnName(@"Eye_Left_Up")] public float Eye_Left_Up { get; set; }
        [ColumnName(@"Eye_Left_Down")] public float Eye_Left_Down { get; set; }
        [ColumnName(@"Eye_Right_Blink")] public float Eye_Right_Blink { get; set; }
        [ColumnName(@"Eye_Right_Wide")] public float Eye_Right_Wide { get; set; }
        [ColumnName(@"Eye_Right_Right")] public float Eye_Right_Right { get; set; }
        [ColumnName(@"Eye_Right_Left")] public float Eye_Right_Left { get; set; }
        [ColumnName(@"Eye_Right_Up")] public float Eye_Right_Up { get; set; }
        [ColumnName(@"Eye_Right_Down")] public float Eye_Right_Down { get; set; }
        [ColumnName(@"Eye_Frown")] public float Eye_Frown { get; set; }
        [ColumnName(@"Eye_Left_Squeeze")] public float Eye_Left_Squeeze { get; set; }
        [ColumnName(@"Eye_Right_Squeeze")] public float Eye_Right_Squeeze { get; set; }
        [ColumnName(@"Max")] public float Max { get; set; }
        [ColumnName(@"None")] public float None { get; set; }
        [ColumnName(@"l_pupil_diameter")] public float l_pupil_diameter { get; set; }
        [ColumnName(@"r_pupil_diameter")] public float r_pupil_diameter { get; set; }
        [ColumnName(@"Jaw_Right")] public float Jaw_Right { get; set; }
        [ColumnName(@"Jaw_Left")] public float Jaw_Left { get; set; }
        [ColumnName(@"Jaw_Forward")] public float Jaw_Forward { get; set; }
        [ColumnName(@"Jaw_Open")] public float Jaw_Open { get; set; }
        [ColumnName(@"Mouth_Ape_Shape")] public float Mouth_Ape_Shape { get; set; }
        [ColumnName(@"Mouth_Upper_Right")] public float Mouth_Upper_Right { get; set; }
        [ColumnName(@"Mouth_Upper_Left")] public float Mouth_Upper_Left { get; set; }
        [ColumnName(@"Mouth_Lower_Right")] public float Mouth_Lower_Right { get; set; }
        [ColumnName(@"Mouth_Lower_Left")] public float Mouth_Lower_Left { get; set; }
        [ColumnName(@"Mouth_Upper_Overturn")] public float Mouth_Upper_Overturn { get; set; }
        [ColumnName(@"Mouth_Lower_Overturn")] public float Mouth_Lower_Overturn { get; set; }
        [ColumnName(@"Mouth_Pout")] public float Mouth_Pout { get; set; }
        [ColumnName(@"Mouth_Smile_Right")] public float Mouth_Smile_Right { get; set; }
        [ColumnName(@"Mouth_Smile_Left")] public float Mouth_Smile_Left { get; set; }
        [ColumnName(@"Mouth_Sad_Right")] public float Mouth_Sad_Right { get; set; }
        [ColumnName(@"Mouth_Sad_Left")] public float Mouth_Sad_Left { get; set; }
        [ColumnName(@"Cheek_Puff_Right")] public float Cheek_Puff_Right { get; set; }
        [ColumnName(@"Cheek_Puff_Left")] public float Cheek_Puff_Left { get; set; }
        [ColumnName(@"Cheek_Suck")] public float Cheek_Suck { get; set; }
        [ColumnName(@"Mouth_Upper_UpRight")] public float Mouth_Upper_UpRight { get; set; }
        [ColumnName(@"Mouth_Upper_UpLeft")] public float Mouth_Upper_UpLeft { get; set; }
        [ColumnName(@"Mouth_Lower_DownRight")] public float Mouth_Lower_DownRight { get; set; }
        [ColumnName(@"Mouth_Lower_DownLeft")] public float Mouth_Lower_DownLeft { get; set; }
        [ColumnName(@"Mouth_Upper_Inside")] public float Mouth_Upper_Inside { get; set; }
        [ColumnName(@"Mouth_Lower_Inside")] public float Mouth_Lower_Inside { get; set; }
        [ColumnName(@"Mouth_Lower_Overlay")] public float Mouth_Lower_Overlay { get; set; }
        [ColumnName(@"Tongue_LongStep1")] public float Tongue_LongStep1 { get; set; }
        [ColumnName(@"Tongue_Left")] public float Tongue_Left { get; set; }
        [ColumnName(@"Tongue_Right")] public float Tongue_Right { get; set; }
        [ColumnName(@"Tongue_Up")] public float Tongue_Up { get; set; }
        [ColumnName(@"Tongue_Down")] public float Tongue_Down { get; set; }
        [ColumnName(@"Tongue_Roll")] public float Tongue_Roll { get; set; }
        [ColumnName(@"Tongue_LongStep2")] public float Tongue_LongStep2 { get; set; }
        [ColumnName(@"Tongue_UpRight_Morph")] public float Tongue_UpRight_Morph { get; set; }
        [ColumnName(@"Tongue_UpLeft_Morph")] public float Tongue_UpLeft_Morph { get; set; }

        [ColumnName(@"Tongue_DownRight_Morph")]
        public float Tongue_DownRight_Morph { get; set; }

        [ColumnName(@"Tongue_DownLeft_Morph")] public float Tongue_DownLeft_Morph { get; set; }
        [ColumnName(@"emotion")] public string emotion { get; set; }

        ~ModelInput() => GC.Collect();

        /*
        public ModelInput(Dictionary<EyeShape_v2, float> eyeWeightings, Dictionary<LipShape_v2, float> lipWeightings)
        {
            
            foreach (var (key, value) in eyeWeightings)
            {
                var info = GetType().GetProperty(Enum.GetName(typeof(EyeShape_v2), key) ?? string.Empty);
                if (info == null || !info.CanWrite) continue;
                info.SetValue(this, value);
            }
            
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.LEFT, out var ld);
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.RIGHT, out var rd);
            l_pupil_diameter = ld;
            r_pupil_diameter = rd;
            
            foreach (var (key, value) in lipWeightings)
            {
                var info = GetType().GetProperty(Enum.GetName(typeof(LipShape_v2), key) ?? string.Empty);
                if (info == null || !info.CanWrite) continue;
                info.SetValue(this, value);
            }
            
            emotion = string.Empty;
        }
        */

        private ModelInput(IReadOnlyDictionary<EyeShape_v2, float> eyeWeightings,
            IReadOnlyDictionary<LipShape_v2, float> lipWeightings)
        {
            Eye_Left_Blink = eyeWeightings[EyeShape_v2.Eye_Left_Blink];
            Eye_Left_Wide = eyeWeightings[EyeShape_v2.Eye_Left_Wide];
            Eye_Left_Right = eyeWeightings[EyeShape_v2.Eye_Left_Right];
            Eye_Left_Left = eyeWeightings[EyeShape_v2.Eye_Left_Left];
            Eye_Left_Up = eyeWeightings[EyeShape_v2.Eye_Left_Up];
            Eye_Left_Down = eyeWeightings[EyeShape_v2.Eye_Left_Down];
            Eye_Right_Blink = eyeWeightings[EyeShape_v2.Eye_Right_Blink];
            Eye_Right_Wide = eyeWeightings[EyeShape_v2.Eye_Right_Wide];
            Eye_Right_Right = eyeWeightings[EyeShape_v2.Eye_Right_Right];
            Eye_Right_Left = eyeWeightings[EyeShape_v2.Eye_Right_Left];
            Eye_Right_Up = eyeWeightings[EyeShape_v2.Eye_Right_Up];
            Eye_Right_Down = eyeWeightings[EyeShape_v2.Eye_Right_Down];
            Eye_Frown = eyeWeightings[EyeShape_v2.Eye_Frown];
            Eye_Left_Squeeze = eyeWeightings[EyeShape_v2.Eye_Left_Squeeze];
            Eye_Right_Squeeze = eyeWeightings[EyeShape_v2.Eye_Right_Squeeze];

            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.LEFT, out var ld);
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.RIGHT, out var rd);
            l_pupil_diameter = ld;
            r_pupil_diameter = rd;

            Jaw_Right = lipWeightings[LipShape_v2.Jaw_Right];
            Jaw_Left = lipWeightings[LipShape_v2.Jaw_Left];
            Jaw_Forward = lipWeightings[LipShape_v2.Jaw_Forward];
            Jaw_Open = lipWeightings[LipShape_v2.Jaw_Open];
            Mouth_Ape_Shape = lipWeightings[LipShape_v2.Mouth_Ape_Shape];
            Mouth_Upper_Right = lipWeightings[LipShape_v2.Mouth_Upper_Right];
            Mouth_Upper_Left = lipWeightings[LipShape_v2.Mouth_Upper_Left];
            Mouth_Lower_Right = lipWeightings[LipShape_v2.Mouth_Lower_Right];
            Mouth_Lower_Left = lipWeightings[LipShape_v2.Mouth_Lower_Left];
            Mouth_Upper_Overturn = lipWeightings[LipShape_v2.Mouth_Upper_Overturn];
            Mouth_Lower_Overturn = lipWeightings[LipShape_v2.Mouth_Lower_Overturn];
            Mouth_Pout = lipWeightings[LipShape_v2.Mouth_Pout];
            Mouth_Smile_Right = lipWeightings[LipShape_v2.Mouth_Smile_Right];
            Mouth_Smile_Left = lipWeightings[LipShape_v2.Mouth_Smile_Left];
            Mouth_Sad_Right = lipWeightings[LipShape_v2.Mouth_Sad_Right];
            Mouth_Sad_Left = lipWeightings[LipShape_v2.Mouth_Sad_Left];
            Cheek_Puff_Right = lipWeightings[LipShape_v2.Cheek_Puff_Right];
            Cheek_Puff_Left = lipWeightings[LipShape_v2.Cheek_Puff_Left];
            Cheek_Suck = lipWeightings[LipShape_v2.Cheek_Suck];
            Mouth_Upper_UpRight = lipWeightings[LipShape_v2.Mouth_Upper_UpRight];
            Mouth_Upper_UpLeft = lipWeightings[LipShape_v2.Mouth_Upper_UpLeft];
            Mouth_Lower_DownRight = lipWeightings[LipShape_v2.Mouth_Lower_DownRight];
            Mouth_Lower_DownLeft = lipWeightings[LipShape_v2.Mouth_Lower_DownLeft];
            Mouth_Upper_Inside = lipWeightings[LipShape_v2.Mouth_Upper_Inside];
            Mouth_Lower_Inside = lipWeightings[LipShape_v2.Mouth_Lower_Inside];
            Mouth_Lower_Overlay = lipWeightings[LipShape_v2.Mouth_Lower_Overlay];
            Tongue_LongStep1 = lipWeightings[LipShape_v2.Tongue_LongStep1];
            Tongue_Left = lipWeightings[LipShape_v2.Tongue_Left];
            Tongue_Right = lipWeightings[LipShape_v2.Tongue_Right];
            Tongue_Up = lipWeightings[LipShape_v2.Tongue_Up];
            Tongue_Down = lipWeightings[LipShape_v2.Tongue_Down];
            Tongue_Roll = lipWeightings[LipShape_v2.Tongue_Roll];
            Tongue_LongStep2 = lipWeightings[LipShape_v2.Tongue_LongStep2];
            Tongue_UpRight_Morph = lipWeightings[LipShape_v2.Tongue_UpRight_Morph];
            Tongue_UpLeft_Morph = lipWeightings[LipShape_v2.Tongue_UpLeft_Morph];
            Tongue_DownRight_Morph = lipWeightings[LipShape_v2.Tongue_DownRight_Morph];
            Tongue_DownLeft_Morph = lipWeightings[LipShape_v2.Tongue_DownLeft_Morph];

            emotion = string.Empty;
            Max = 0;
            None = 0;
        }

        public static ModelInput Transform(IReadOnlyDictionary<EyeShape_v2, float> eyeWeightings,
            IReadOnlyDictionary<LipShape_v2, float> lipWeightings) =>
            new ModelInput(eyeWeightings, lipWeightings);
    }
}