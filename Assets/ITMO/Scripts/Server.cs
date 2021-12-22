using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using ITMO.Scripts.SRanipal;
using NarupaXR;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ITMO.Scripts
{
    public class Server : MonoBehaviour
    {
        public static bool ServerConnected { get; private set; }

        private NarupaXRPrototype simulation;

        private Process serverProcess;

        private void RunServer()
        {
            serverProcess = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = Application.streamingAssetsPath + "\\Server\\run.exe",
                    CreateNoWindow = true,
                    Arguments = "-w",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };

            try
            {
                serverProcess.Start();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        public void Send(string line)
        {
            serverProcess.StandardInput.WriteLine(line);
            Read(serverProcess.StandardOutput);
            if (Reference.EyeLogger != null)
            {
                Reference.EyeLogger.AddInfo(
                    $"Changed gaze - {Reference.counter}; Time spent in seconds - {Reference.Stopwatch.Elapsed.TotalSeconds}");
                Reference.EyeLogger.WriteInfo();
            }
            if (Reference.FaceLogger != null) Reference.FaceLogger.WriteInfo();
            Reference.counter = 0;
            Reference.Stopwatch.Restart();
        }

        public void SendAndConnect(string line)
        {
            Send(line);
            Connect();
        }

        public void Start()
        {
            RunServer();
            simulation = GetComponent<NarupaXRPrototype>();
        }

        private static void Read(StreamReader reader)
        {
            // new Thread(() =>
            // {
            //     while (true)
            //     {
            //         string current;
            //         while ((current = reader.ReadLine()) != null)
            //             Debug.LogWarning(current);
            //     }
            // }).Start();
        }


        public void Connect()
        {
            if (ServerConnected) return;
            simulation.AutoConnect();
            ServerConnected = true;
            Reference.EyeLogger = new Logger(false);
            Reference.FaceLogger = new Logger(true);
            Reference.counter = 0;
            Reference.Stopwatch.Restart();
        }

        public void Disconnect()
        {
            if (!ServerConnected) return;
            simulation.Disconnect();
            ServerConnected = false;
            Reference.Stopwatch.Stop();
            Reference.EyeLogger.AddInfo(
                $"Changed gaze - {Reference.counter}; Time spent in seconds - {Reference.Stopwatch.Elapsed.TotalSeconds}");
            Reference.EyeLogger.Close();
            Reference.FaceLogger.Close();
        }

        public void OnApplicationQuit()
        {
            Disconnect();
            serverProcess.Kill();
        }
    }
}