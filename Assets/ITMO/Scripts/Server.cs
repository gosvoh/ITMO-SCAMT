using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Remoting;
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
        public static readonly UnityEvent ConnectEvent = new UnityEvent();

        [SerializeField] private NarupaImdSimulation simulation;

        private Process _serverProcess;

        public static bool ServerConnected { get; private set; }

        public void Start()
        {
            if (!TestTcpConnection()) RunServerProcess();
        }

        public void OnApplicationQuit()
        {
            Disconnect();
            if (_serverProcess == null) return;
            var client = new TcpClient("localhost", 7777);
            var buffer = Encoding.UTF8.GetBytes("q");
            client.GetStream().Write(buffer, 0, buffer.Length);
            client.Close();
            Thread.Sleep(1000);
            try
            {
                _serverProcess.StandardInput.Write("");
                _serverProcess.StandardInput.Flush();
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void RunServerProcess()
        {
            try
            {
                _serverProcess = new Process
                {
                    StartInfo =
                    {
                        FileName = Application.dataPath + "\\..\\run_server.bat",
                        WorkingDirectory = Application.dataPath + "\\..\\",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        CreateNoWindow = true
                    }
                };


                _serverProcess.Start();
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

        public async void Connect()
        {
            if (ServerConnected) return;
            for (var i = 0; i < 5; i++)
            {
                await simulation.AutoConnect();
                if (simulation.gameObject.activeSelf) break;
            }

            if (!simulation.gameObject.activeSelf) throw new ServerException("Cannot connect to Narupa server");

            ServerConnected = true;
            ConnectEvent.Invoke();
            EyeInteraction.EyeGazeChangedCounter = 0;
            Reference.Stopwatch.Restart();
        }

        public static async void Send(string pathToMolecule, string host = "127.0.0.1", int port = 7777)
        {
            Reference.Stopwatch.Stop();
            SendEvent.Invoke();
            try
            {
                var client = new TcpClient(host, port);
                var buffer = Encoding.UTF8.GetBytes(pathToMolecule);
                await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

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