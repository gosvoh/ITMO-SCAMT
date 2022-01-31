using UnityEngine;

namespace ITMO.Scripts
{
    public class WallInfo : MonoBehaviour, IInfo
    {
        public int Index { get; set; }
        public GameObject Obj { get; set; }
    }
}