using NarupaIMD;
using UnityEngine;

public class RendererGenerator : MonoBehaviour
{
    [SerializeField]
    private NarupaImdSimulation simulation;

    private bool isCreated = false;

    private void Start()
    {
        if (simulation == null)
            return;
    }

    /*private void Update()
    {
        if (!isCreated && simulation.FrameSynchronizer != null && simulation.FrameSynchronizer.CurrentFrame != null)
        {
            var frame = simulation.FrameSynchronizer.CurrentFrame;

            var positions = frame.ParticlePositions;
            var names = frame.ParticleNames;

            for (int i = 0; i < frame.ParticleCount; i++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                obj.transform.position = positions[i];
                obj.name = names[i];

                GameObject createdObj = Instantiate(obj, gameObject.transform);

                AtomInfo atomInfo = createdObj.GetComponent<AtomInfo>();

                atomInfo.Index = i;
                atomInfo.Obj = createdObj;

                Debug.LogError("index: " + i);
            }

            isCreated = true;

            throw new System.Exception("KEK");
        }
    }*/
}
