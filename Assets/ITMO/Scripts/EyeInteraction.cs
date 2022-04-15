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

        private Logger _logger;
        public static int EyeGazeChangedCounter;

        private static int _id = -10;
        private static readonly List<GameObject> Spheres = new List<GameObject>();

        // private readonly GazeIndex[] _gazePriority = { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
        private GameObject _parent;
        private int _counter = -1;

        private void Awake() => SetWalls();

        private void Start()
        {
            Server.SendEvent.AddListener(EventHandler);
            Server.ConnectionEvent.AddListener(ConnectionHandler);
        }

        private void ConnectionHandler()
        {
            _logger = new Logger();
            _logger.AddInfo("timestamp|position|ID");
        }

        private void EventHandler()
        {
            if (!Server.ServerConnected) return;
            
            _logger.AddInfo(
                $"Level - {Level.CurrentLevelName}; Time spent in seconds - {Reference.Stopwatch.Elapsed.TotalSeconds}; Gaze changed - {EyeGazeChangedCounter}");
            _logger.WriteInfo();
            EyeGazeChangedCounter = 0;
        }

        private void OnDisable() => Destroy(_parent);

        private void FixedUpdate()
        {
            if (!Server.ServerConnected || _logger == null) return;
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;
            if (_counter++ % 10 != 0) return;
            
            UpdateScene();
            
            // foreach (var index in _gazePriority)
            // {
            //     var layer = atomPrefab.layer;
            //     var eyeFocus = SRanipal_Eye_v2.Focus(index, out _, out var focusInfo, 0,
            //         float.MaxValue, 1 << layer);
            //     if (!eyeFocus) continue;
            //     var info = focusInfo.transform.GetComponent<Info>();
            //     if (info == null) break;
            //     if (info.Index == _id) break;
            //     _id = info.Index;
            //     EyeGazeChangedCounter++;
            //     _logger.AddInfo($"{DateTime.Now:HH:mm:ss.fff}|{info.Obj.transform.position}|{info.Index}");
            //     _logger.WriteInfo();
            // }
            
            var layer = atomPrefab.layer;
            var eyeFocus = SRanipal_Eye_v2.Focus(GazeIndex.COMBINE, out _, out var focusInfo, 0,
                float.MaxValue, 1 << layer);
            if (!eyeFocus) return;
            var info = focusInfo.transform.GetComponent<Info>();
            if (info == null || info.Index == _id) return;
            _id = info.Index;
            EyeGazeChangedCounter++;
            _logger.AddInfo($"{DateTime.Now:HH:mm:ss.fff}|{info.Obj.transform.position}|{info.Index}");
            _logger.WriteInfo();
        }

        private void UpdateScene()
        {
            var frame = frameSource.CurrentFrame;
            if (frame?.ParticleCount == 0 || Spheres.Count == frame?.ParticleCount) return;
            Destroy(_parent);
            Spheres.Clear();
            CreateSphere(frame);
        }

        private void CreateSphere(Frame frame)
        {
            // Reset simulation space
            var simSpace = GameObject.Find("Simulation Space");
            simSpace.transform.localPosition = Vector3.zero;
            simSpace.transform.rotation = new Quaternion();

            _parent = new GameObject("ParentEyeInteraction")
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
                atom.transform.SetParent(_parent.transform, false);
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
    }
}