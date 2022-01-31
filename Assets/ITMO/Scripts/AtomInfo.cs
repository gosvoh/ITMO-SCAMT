using NarupaXR.Interaction;
using UnityEngine;

namespace ITMO.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class AtomInfo : MonoBehaviour, IInfo
    {
        public int Index { get; set; }
        public GameObject Obj { get; set; }

        private GameObject cameraObj;

        private void Awake()
        {
            cameraObj = GameObject.Find("Camera");
        }

        private void Update()
        {
            var thisPos = transform.position;
            var camPos = cameraObj.transform.position;
            if (Vector3.Distance(thisPos, camPos) < EyeInteraction.VisibilityRadius) return;
            EyeInteraction.Spheres.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}