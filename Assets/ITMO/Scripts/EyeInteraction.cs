using System.Collections.Generic;
using ITMO.Scripts;
using UnityEngine;
using Narupa.Visualisation;
using Plugins.Narupa.Frame;

namespace NarupaXR.Interaction
{
    public class EyeInteraction : MonoBehaviour
    {
        [Header("The provider of the frames which can be grabbed.")]
        [SerializeField]
        private SynchronisedFrameSource frameSource;

        [SerializeField]
        [Header("This object is atom prefab for raycasting")]
        private GameObject atomPrefab;

        private List<GameObject> spheres;

        private bool isInit;
        private GameObject parent;

        private void Awake() => SetWalls();

        public void OnEnable() => isInit = false;

        private void OnDisable() => Destroy(parent);

        private void Update()
        {
            if (isInit)
            {
                var frame = frameSource.CurrentFrame;
                UpdateSpherePositions(frame);
            }
            else
            {
                var frame = frameSource.CurrentFrame;

                if (isInit || frame?.ParticlePositions == null) return;
                isInit = true;
                CreateSphere(frame);
            }
        }

        private void CreateSphere(Frame frame)
        {
            var simSpace = GameObject.Find("Simulation Space");
            simSpace.transform.localPosition = Vector3.zero;
            simSpace.transform.rotation = new Quaternion();
            
            parent = new GameObject("ParentEyeInteraction")
            {
                transform =
                {
                    parent = gameObject.transform,
                    localPosition = Vector3.zero,
                    localScale = Vector3.one,
                    rotation = new Quaternion()
                }
            };

            spheres = new List<GameObject>();
            
            for (var i = 0; i < frame.ParticlePositions.Length; ++i)
            {
                var particlePosition = frame.ParticlePositions[i];
                var sphere = Instantiate(atomPrefab, particlePosition, Quaternion.identity, parent.transform);
                var atomInfo = sphere.GetComponent<AtomInfo>();
                spheres.Add(sphere);
                atomInfo.Index = frame.Particles[i].Index;
                atomInfo.Obj = sphere;
            }
        }

        private static void SetWalls()
        {
            for (var i = 0; i < 6; i++)
            {
                var wall = GameObject.Find("Wall" + i);
                var wallInfo = wall.GetComponent<WallInfo>();
                wallInfo.Index = -(i + 1);
                wallInfo.Obj = wall;
            }
        }

        private void UpdateSpherePositions(Frame frame)
        {
            if (frame.ParticlePositions.Length != spheres.Count)
            {
                OnDisable();
                CreateSphere(frame);
            }
            
            for (var i = 0; i < frame.ParticlePositions.Length; ++i)
            {
                var atomPos = frame.ParticlePositions[i];
                if (spheres.Count > 0)
                    spheres[i].transform.localPosition = new Vector3(atomPos.x, atomPos.y, atomPos.z);
            }
        }
    }
}
