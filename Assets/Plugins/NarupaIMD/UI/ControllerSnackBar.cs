﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NarupaIMD.UI
{
    public class ControllerSnackBar : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        private float strength = 0;

        [SerializeField]
        private float decaySpeed = 1;

        private void Awake()
        {
            Assert.IsNotNull(text);
        }

        private void Update()
        {
            if (strength > 0)
            {
                text.enabled = true;
                strength -= Time.deltaTime * decaySpeed;
                text.color = new Color(1, 1, 1, strength * strength * (3 - 2 * strength));
                this.transform.rotation =
                    Quaternion.LookRotation(-(Camera.main.transform.position - this.transform.position),
                                            this.transform.parent.forward);
            }
            else
            {
                text.enabled = false;
            }
        
        }

        public void PushNotification(string text)
        {
            this.text.text = text;
            strength = 1;
        }
    }
}
