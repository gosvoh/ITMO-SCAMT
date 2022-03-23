using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NarupaIMD;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace ITMO.Scripts
{
    public class Server : MonoBehaviour
    {
        public static readonly UnityEvent SendEvent = new UnityEvent();
        [SerializeField] private NarupaImdSimulation simulation;

        private Process serverProcess;

        public static bool ServerConnected { get; private set; }

        public void Start()
        {
            if (!TestTcpConnection()) RunServerProcess();
        }

        public void OnApplicationQuit()
        {
            Disconnect();
            if (serverProcess == null) return;
            var client = new TcpClient("localhost", 7777);
            var buffer = Encoding.UTF8.GetBytes("q");
            client.GetStream().Write(buffer, 0, buffer.Length);
            client.Close();
            Thread.Sleep(1000);
            try
            {
                serverProcess.StandardInput.Write("");
                serverProcess.StandardInput.Flush();
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void RunServerProcess()
        {
            try
            {
                serverProcess = new Process
                {
                    StartInfo =
                    {
#if UNITY_EDITOR
                        FileName = "C:\\Users\\gosvoh\\Unity\\run_server.bat",
                        WorkingDirectory = "C:\\Users\\gosvoh\\Unity\\",
#else
                    FileName = Application.dataPath + "\\..\\run_server.bat",
                    WorkingDirectory = Application.dataPath + "\\..\\",
#endif
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        CreateNoWindow = true
                    }
                };


                serverProcess.Start();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static bool TestTcpConnection(string host = "127.0.0.1", int port = 7777,
            int timeout = 3000)
        {
            var client = new TcpClient();
            try
            {
                var result = client.BeginConnect(host, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(timeout);
                if (success) client.EndConnect(result);
                client.Close();
                return success;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public void Connect()
        {
            if (ServerConnected) return;
            for (var i = 0; i < 50; i++)
            {
                simulation.AutoConnect();
                Thread.Sleep(100);
                if (simulation.gameObject.activeSelf) break;
            }

            ServerConnected = true;
            InitLoggers();
            EyeInteraction.EyeGazeChangedCounter = 0;
            Reference.Stopwatch.Restart();
        }

        private void InitLoggers()
        {
            EyeInteraction.Logger = new Logger();
            EyeTracker.Logger = new Logger("_eyeTracker");
            FaceTracker.Logger = new Logger("_faceTracker");
            StupidExpressionClassifier.Logger = new Logger("_emotions");
        }

        public static void Send(string pathToMolecule, string host = "127.0.0.1", int port = 7777)
        {
            Reference.Stopwatch.Stop();
            SendEvent.Invoke();
            var client = new TcpClient(host, port);
            var buffer = Encoding.UTF8.GetBytes(pathToMolecule);
            client.GetStream().Write(buffer, 0, buffer.Length);
            client.Close();
            Reference.Stopwatch.Restart();
        }

        // private Task<string> Read()
        // {
        //     throw new NotImplementedException();
        //     // var buffer = new byte[256];
        //     // await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
        //     // return Encoding.UTF8.GetString(buffer);
        // }

        public void Disconnect()
        {
            if (!ServerConnected) return;
            simulation.Disconnect();
            ServerConnected = false;
            Reference.Stopwatch.Stop();
            SendEvent.Invoke();
        }
    }
}