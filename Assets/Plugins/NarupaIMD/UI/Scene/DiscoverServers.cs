﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Essd;
using Narupa.Frontend.UI;
using NarupaXR;
using Plugins.Narupa.Core.Async;
using UnityEngine;
using UnityEngine.Events;

namespace NarupaIMD.UI.Scene
{
    public class DiscoverServers : MonoBehaviour
    {
        private Client client;
        private List<ServiceHub> hubs = new List<ServiceHub>();

        [SerializeField]
        private Sprite localServerIcon;

        [SerializeField]
        private Sprite remoteServerIcon;

        [SerializeField]
        private NarupaXRPrototype application;

        [SerializeField]
        private DynamicMenu menu;

        private void OnEnable()
        {
            Refresh();
        }

        /// <summary>
        /// Returns either the <see cref="ServiceHub"/> with a localhost IP address or the first hub.
        /// </summary>
        private ServiceHub SelectBestService(IGrouping<string, ServiceHub> group)
        {
            foreach (var hub in group)
            {
                if (IsLocalhost(hub))
                    return hub;
            }

            return group.First();
        }

        private bool IsLocalhost(ServiceHub hub)
        {
            return hub.Address.Equals("127.0.0.1") || hub.Address.Equals("localhost");
        }

        private Task currentSearchTask = null;

        [SerializeField]
        private UnityEvent startSearch;

        [SerializeField]
        private UnityEvent endSearch;

        public async Task SearchAsync()
        {
            currentSearchTask = client.StartSearch();
            startSearch?.Invoke();
            await Task.WhenAny(Task.Delay(500), currentSearchTask);
            await client.StopSearch();
            currentSearchTask = null;
            endSearch?.Invoke();
            RefreshHubs();
        }

        public void Refresh()
        {
            if (currentSearchTask != null)
            {
                client.StopSearch().AwaitInBackground();
                currentSearchTask = null;
                endSearch?.Invoke();
            }

            client = new Client();
            hubs.Clear();
            client.ServiceFound += (obj, args) => hubs.Add(args);
            SearchAsync().AwaitInBackground();
        }

        public void RefreshHubs()
        {
            hubs = hubs.GroupBy(hub => hub.Id)
                       .Select(SelectBestService)
                       .ToList();

            menu.ClearChildren();
            foreach (var hub in hubs)
            {
                var local = hub.Address.Equals("127.0.0.1") || hub.Address.Equals("localhost");
                menu.AddItem(hub.Name, local ? localServerIcon : remoteServerIcon,
                             () => application.Connect(hub), hub.Address);
            }
        }
    }
}