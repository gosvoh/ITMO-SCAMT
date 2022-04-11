using System;
using System.Collections.Generic;
using Narupa.Visualisation;
using Plugins.Narupa.Frame;
using UnityEngine;
using ViveSR.anipal.Eye;

namespace ITMO.Scripts
{
    public class EyeInteraction : MonoBehaviour
    {
        [SerializeField] private SynchronisedFrameSource frameSource;
        [SerializeField] private GameObject atomPrefab;

        public static Logger Logger;
        public static int EyeGazeChangedCounter;
        public static LayerMask PrefabLayer;
        public static int LastID = -10;
        
        private static readonly List<GameObject> Spheres = new List<GameObject>();

        // private readonly GazeIndex[] gazePriority = { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
        private GameObject parent;
        private int counter = -1;
        private bool logHeaderSet;

        private void Awake()
        {
            PrefabLayer = LayerMask.NameToLayer("GazeCollider");
            Server.SendEvent.AddListener(SendEventHandler);
            Server.ConnectEvent.AddListener(ConnectEventHandler);
            SetWalls();
        }

        private static void SendEventHandler()
        {
            if (Logger == null) return;
            Logger.AddInfo(
                $"Level - {Level.CurrentLevelName}; Time spent in seconds - {Reference.Stopwatch.Elapsed.TotalSeconds}; Gaze changed - {EyeGazeChangedCounter}");
            Logger.WriteInfo();
            EyeGazeChangedCounter = 0;
        }

        private static void ConnectEventHandler()
        {
            Logger = new Logger();
            Logger.AddInfo("API|timestamp|position|ID");
        }

        // private void Update()
        // {
        //     if (!Server.ServerConnected || Logger == null) return;
        //     if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;
        //
        //     foreach (var index in gazePriority)
        //     {
        //         var eyeFocus = SRanipal_Eye_v2.Focus(index, out _, out var focusInfo, 0,
        //             float.MaxValue, 1 << PrefabLayer);
        //         if (!eyeFocus) continue;
        //         var info = focusInfo.transform.GetComponent<Info>();
        //         if (info == null) break;
        //         if (info.Index == LastID) break;
        //         LastID = info.Index;
        //         EyeGazeChangedCounter++;
        //         Logger.AddInfo($"SRanipal|{DateTime.Now:HH:mm:ss.fff}|{info.Obj.transform.position}|{info.Index}");
        //         Logger.WriteInfo();
        //     }
        // }

        private void FixedUpdate()
        {
            var frame = frameSource.CurrentFrame;
            if (counter++ % 10 != 0) return;
            if (frame?.ParticleCount == 0 || Spheres.Count == frame?.ParticleCount) return;
            Destroy(parent);
            Spheres.Clear();
            CreateSphere(frame);
        }

        private void CreateSphere(Frame frame)
        {
            // Reset simulation space
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
            for (int i = 0, partCount = frame.ParticleCount; i < partCount; ++i)
            {
                var atom = Instantiate(atomPrefab, particlePositions[i], Quaternion.identity);
                atom.transform.SetParent(parent.transform, false);
                var info = atom.GetComponent<Info>();
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
                var wallInfo = wall.GetComponent<Info>();
                wallInfo.Index = -(i + 1);
                wallInfo.Obj = wall;
            }
        }

        private void OnDisable() => Destroy(parent);
    }
}