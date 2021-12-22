using Tobii.G2OM;
using UnityEngine;

public class AtomInfo : MonoBehaviour, IGazeFocusable
{
    public int Index { get; set; }

    public GameObject Obj { get; set; }

    public void GazeFocusChanged(bool hasFocus)
    {
        if (!hasFocus || Reference.EyeLogger == null) return;
            
        // var data = TobiiXR.Advanced.LatestData;
        if (Reference.ID == Index) return;
        Reference.EyeLogger.AddInfo($"Position: {Obj.transform.position}, ID: {Index + 1}");
        Reference.ID = Index;
        Reference.counter++;

        // Reference.Logger.AddInfo(
        //     $"Position: {gameObject.transform.position}, ID: {atomID}, left pupil: {data.Left.PupilDiameter}, " +
        //     $"right pupil: {data.Right.PupilDiameter}");
    }
}
