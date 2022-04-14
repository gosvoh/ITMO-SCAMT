namespace ML
{
    public class Data : RealData
    {
        public string timestamp { get; set; }
    }

    public class RealData
    {
        public float Eye_Left_Blink { get; set; }
        public float Eye_Left_Wide { get; set; }
        public float Eye_Left_Right { get; set; }
        public float Eye_Left_Left { get; set; }
        public float Eye_Left_Up { get; set; }
        public float Eye_Left_Down { get; set; }
        public float Eye_Right_Blink { get; set; }
        public float Eye_Right_Wide { get; set; }
        public float Eye_Right_Right { get; set; }
        public float Eye_Right_Left { get; set; }
        public float Eye_Right_Up { get; set; }
        public float Eye_Right_Down { get; set; }
        public float Eye_Frown { get; set; }
        public float Eye_Left_Squeeze { get; set; }
        public float Eye_Right_Squeeze { get; set; }
        public float Max { get; set; }
        public float None { get; set; }
        public float l_pupil_diameter { get; set; }
        public float r_pupil_diameter { get; set; }
        public float Jaw_Right { get; set; }
        public float Jaw_Left { get; set; }
        public float Jaw_Forward { get; set; }
        public float Jaw_Open { get; set; }
        public float Mouth_Ape_Shape { get; set; }
        public float Mouth_Upper_Right { get; set; }
        public float Mouth_Upper_Left { get; set; }
        public float Mouth_Lower_Right { get; set; }
        public float Mouth_Lower_Left { get; set; }
        public float Mouth_Upper_Overturn { get; set; }
        public float Mouth_Lower_Overturn { get; set; }
        public float Mouth_Pout { get; set; }
        public float Mouth_Smile_Right { get; set; }
        public float Mouth_Smile_Left { get; set; }
        public float Mouth_Sad_Right { get; set; }
        public float Mouth_Sad_Left { get; set; }
        public float Cheek_Puff_Right { get; set; }
        public float Cheek_Puff_Left { get; set; }
        public float Cheek_Suck { get; set; }
        public float Mouth_Upper_UpRight { get; set; }
        public float Mouth_Upper_UpLeft { get; set; }
        public float Mouth_Lower_DownRight { get; set; }
        public float Mouth_Lower_DownLeft { get; set; }
        public float Mouth_Upper_Inside { get; set; }
        public float Mouth_Lower_Inside { get; set; }
        public float Mouth_Lower_Overlay { get; set; }
        public float Tongue_LongStep1 { get; set; }
        public float Tongue_Left { get; set; }
        public float Tongue_Right { get; set; }
        public float Tongue_Up { get; set; }
        public float Tongue_Down { get; set; }
        public float Tongue_Roll { get; set; }
        public float Tongue_LongStep2 { get; set; }
        public float Tongue_UpRight_Morph { get; set; }
        public float Tongue_UpLeft_Morph { get; set; }
        public float Tongue_DownRight_Morph { get; set; }
        public float Tongue_DownLeft_Morph { get; set; }
        public string emotion { get; set; }
    }

    public class RealDataPredict
    {
        public float Eye_Left_Blink { get; set; }
        public float Eye_Left_Wide { get; set; }
        public float Eye_Left_Right { get; set; }
        public float Eye_Left_Left { get; set; }
        public float Eye_Left_Up { get; set; }
        public float Eye_Left_Down { get; set; }
        public float Eye_Right_Blink { get; set; }
        public float Eye_Right_Wide { get; set; }
        public float Eye_Right_Right { get; set; }
        public float Eye_Right_Left { get; set; }
        public float Eye_Right_Up { get; set; }
        public float Eye_Right_Down { get; set; }
        public float Eye_Frown { get; set; }
        public float Eye_Left_Squeeze { get; set; }
        public float Eye_Right_Squeeze { get; set; }
        public float Max { get; set; }
        public float None { get; set; }
        public float l_pupil_diameter { get; set; }
        public float r_pupil_diameter { get; set; }
        public float Jaw_Right { get; set; }
        public float Jaw_Left { get; set; }
        public float Jaw_Forward { get; set; }
        public float Jaw_Open { get; set; }
        public float Mouth_Ape_Shape { get; set; }
        public float Mouth_Upper_Right { get; set; }
        public float Mouth_Upper_Left { get; set; }
        public float Mouth_Lower_Right { get; set; }
        public float Mouth_Lower_Left { get; set; }
        public float Mouth_Upper_Overturn { get; set; }
        public float Mouth_Lower_Overturn { get; set; }
        public float Mouth_Pout { get; set; }
        public float Mouth_Smile_Right { get; set; }
        public float Mouth_Smile_Left { get; set; }
        public float Mouth_Sad_Right { get; set; }
        public float Mouth_Sad_Left { get; set; }
        public float Cheek_Puff_Right { get; set; }
        public float Cheek_Puff_Left { get; set; }
        public float Cheek_Suck { get; set; }
        public float Mouth_Upper_UpRight { get; set; }
        public float Mouth_Upper_UpLeft { get; set; }
        public float Mouth_Lower_DownRight { get; set; }
        public float Mouth_Lower_DownLeft { get; set; }
        public float Mouth_Upper_Inside { get; set; }
        public float Mouth_Lower_Inside { get; set; }
        public float Mouth_Lower_Overlay { get; set; }
        public float Tongue_LongStep1 { get; set; }
        public float Tongue_Left { get; set; }
        public float Tongue_Right { get; set; }
        public float Tongue_Up { get; set; }
        public float Tongue_Down { get; set; }
        public float Tongue_Roll { get; set; }
        public float Tongue_LongStep2 { get; set; }
        public float Tongue_UpRight_Morph { get; set; }
        public float Tongue_UpLeft_Morph { get; set; }
        public float Tongue_DownRight_Morph { get; set; }
        public float Tongue_DownLeft_Morph { get; set; }
        public uint emotion { get; set; }
    }
}