// Copyright © 2018 – Property of Tobii AB (publ) - All Rights Reserved

using ITMO.Scripts;
using Tobii.G2OM;
using UnityEngine;

namespace Tobii.XR.Examples.GettingStarted
{
    //Monobehaviour which implements the "IGazeFocusable" interface, meaning it will be called on when the object receives focus
    public class HighlightAtGaze : MonoBehaviour, IGazeFocusable
    {
        //The method of the "IGazeFocusable" interface, which will be called when this object receives or loses focus
        public void GazeFocusChanged(bool hasFocus)
        {
            //If this object received focus, fade the object's color to highlight color
            if (hasFocus)
            {
                var data = TobiiXR.Advanced.LatestData;
                if (TryGetComponent(out AtomInfo comp))
                {
                    int atomID = comp.Index;
                    // Reference.Logger.AddInfo(
                    //     $"Position: {gameObject.transform.position}, ID: {atomID}, left pupil: {data.Left.PupilDiameter}, " +
                    //     $"right pupil: {data.Right.PupilDiameter}");
                    Reference.EyeLogger.AddInfo($"Position: {gameObject.transform.position}, ID: {atomID}");
                    Reference.ID = atomID;
                }
                // _targetColor = highlightColor;
            }
        }
    }
}
