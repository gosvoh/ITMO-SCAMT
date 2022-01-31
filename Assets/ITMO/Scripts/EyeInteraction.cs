using System;
using System.Collections.Generic;
using ITMO.Scripts;
using UnityEngine;
using Narupa.Visualisation;
using Plugins.Narupa.Frame;
using ViveSR.anipal.Eye;
using Logger = ITMO.Scripts.Logger;

namespace NarupaXR.Interaction
{
    public class EyeInteraction : MonoBehaviour
    {
        [SerializeField] private SynchronisedFrameSource frameSource;
        [SerializeField] private GameObject atomPrefab;

        public static readonly List<GameObject> Spheres = new List<GameObject>();
        public static int VisibilityRadius = 100;
        public static Logger Logger;
        public static int EyeGazeChangedCounter;

        private static int _id = -10;
        
        private readonly GazeIndex[] gazePriority = { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };

        private GameObject parent;
        private GameObject cameraObj;
        private int counter = -1;
        private bool log;

        private void Awake()
        {
            cameraObj = GameObject.Find("Camera");
            SetWalls();
        }

        private void Start()
        {
            Server.SendEvent.AddListener(EventHandler);
        }

        private void EventHandler()
        {
            if (Logger == null) return;
            Logger.AddInfo(
                $"Level - {Level.CurrentLevelName}; Time spent in seconds - {Reference.Stopwatch.Elapsed.TotalSeconds}; Gaze changed - {EyeGazeChangedCounter}");
            Logger.WriteInfo();
            EyeGazeChangedCounter = 0;
        }

        private void Update()
        {
            if (!Server.ServerConnected || Logger == null)
            {
                return;
            }

            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT)
            {
                Debug.LogError("SRanipal Error");
                return;
            }
            
            if (!log)
            {
                Logger.AddInfo("timestamp|position|ID");
                log = true;
            }

            foreach (var index in gazePriority)
            {
                var layer = atomPrefab.layer;
                var eyeFocus = SRanipal_Eye_v2.Focus(index, out _, out var focusInfo, 0,
                    VisibilityRadius, 1 << layer);
                if (!eyeFocus) continue;
                var info = focusInfo.transform.GetComponent<IInfo>();
                if (info == null) break;
                if (info.Index == _id) break;
                _id = info.Index;
                EyeGazeChangedCounter++;
                Logger.AddInfo($"{DateTime.Now:HH:mm:ss.fff}|{info.Obj.transform.position}|{info.Index}");
                Logger.WriteInfo();
            }
        }

        private void OnDisable() => Destroy(parent);

        private void FixedUpdate()
        {
            var frame = frameSource.CurrentFrame;
            if (frame == null && Server.ServerConnected) GameObject.Find("App").GetComponent<App>().Disconnect();
            if (counter++ % 10 != 0) return;
            if (frame?.ParticleCount == 0 || Spheres.Count == frame?.ParticleCount) return;
            Destroy(parent);
            Spheres.Clear();
            CreateSphere(frame);
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

            var particles = frame.Particles;
            var particlePositions = frame.ParticlePositions;
            for (int i = 0, partNum = frame.ParticleCount; i < partNum; ++i)
            {
                if (Vector3.Distance(particlePositions[i], cameraObj.transform.position) >= VisibilityRadius) continue;
                var atom = Spheres.Find(o => o.GetComponent<AtomInfo>().Index == particles[i].Index);
                if (atom != null) continue;
                atom = Instantiate(atomPrefab, particlePositions[i], Quaternion.identity);
                atom.transform.SetParent(parent.transform, false);
                var info = atom.GetComponent<AtomInfo>();
                info.Index = particles[i].Index;
                info.Obj = atom;
                Spheres.Add(atom);
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
    }
}