using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ITMO.Scripts.SRanipal;
using NarupaXR;
using NarupaXR.Interaction;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace ITMO.Scripts
{
    public class Server : MonoBehaviour
    {
        [SerializeField] private NarupaXRPrototype simulation;

        private Process serverProcess;

        public static bool ServerConnected { get; private set; }
        public static readonly UnityEvent SendEvent = new UnityEvent();

        public void Start()
        {
            if (!TestTcpConnection())
                RunServerProcess();
        }

        public void OnApplicationQuit()
        {
            Disconnect();
            if (serverProcess == null) return;
            var buffer = Encoding.UTF8.GetBytes("q");
            var client = new TcpClient("localhost", 7777);
            client.GetStream().Write(buffer, 0, buffer.Length);
            client.Close();
            Thread.Sleep(1000);
            try
            {
                serverProcess.Kill();
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void RunServerProcess()
        {
            serverProcess = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    RedirectStandardInput = true
                }
            };

            try
            {
                serverProcess.Start();

                using (var si = serverProcess.StandardInput)
                {
                    if (!si.BaseStream.CanWrite) return;
#if UNITY_EDITOR
                    const string condaPath = "D:\\htc-vive-pro-eye-master\\Miniconda3";
#else
                    const string condaPath = ".\\Miniconda3";
#endif
                    si.WriteLine($"call {condaPath}\\Scripts\\activate.bat");
                    si.WriteLine($"python {condaPath}\\Scripts\\run.py");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
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
            simulation.AutoConnect();
            ServerConnected = true;
            EyeInteraction.Logger = new Logger();
            EyeTracker.Logger = new Logger("_eyeTracker");
            FaceTracker.Logger = new Logger("_faceTracker");
            EyeInteraction.EyeGazeChangedCounter = 0;
            Reference.Stopwatch.Restart();
        }

        public void Send(string line)
        {
            Reference.Stopwatch.Stop();
            SendEvent.Invoke();
            var client = new TcpClient("localhost", 7777);
            var buffer = Encoding.UTF8.GetBytes(line);
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