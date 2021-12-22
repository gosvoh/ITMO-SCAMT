using Tobii.G2OM;
using Tobii.XR;
using UnityEngine;

namespace ITMO.Scripts
{
    public class WallInfo : MonoBehaviour, IGazeFocusable
    {
        public int Index { get; set; }

        public GameObject Obj { get; set; }

        public void GazeFocusChanged(bool hasFocus)
        {
            if (!hasFocus || Reference.ID == Index || !Server.ServerConnected || Reference.EyeLogger == null) return;
            var data = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
            Reference.EyeLogger.AddInfo($"Position: {Obj.transform.position}, ID: {Index}");
            Reference.ID = Index;
            Reference.counter++;
        }
    }
}