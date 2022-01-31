using UnityEngine;

namespace ITMO.Scripts
{
    public interface IInfo
    {
        int Index { get; }
        
        GameObject Obj { get; }

        // void Focus();
    }
}