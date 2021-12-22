﻿// Copyright (c) 2019 Intangible Realities Lab. All rights reserved.
// Licensed under the GPL. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Narupa.Frontend.Manipulation;
using Narupa.Grpc.Multiplayer;
using Plugins.Narupa.Core.Async;
using Plugins.Narupa.Core.Math;
using UnityEngine;

namespace NarupaXR.Interaction
{
    /// <summary>
    /// Provides the ability to move the simulation scene, but preventing this
    /// if multiplayer is active and the user does not have a lock on the
    /// scene.
    /// </summary>
    public class ManipulableScenePose
    {
        private readonly Transform sceneTransform;
        private readonly ManipulableTransform manipulable;
        private readonly MultiplayerSession multiplayer;
        private readonly NarupaXRPrototype prototype;

        private readonly HashSet<IActiveManipulation> manipulations
            = new HashSet<IActiveManipulation>();

        public bool CurrentlyEditingScene => manipulations.Any();

        public ManipulableScenePose(Transform sceneTransform,
                                    MultiplayerSession multiplayer,
                                    NarupaXRPrototype prototype)
        {
            this.sceneTransform = sceneTransform;
            this.multiplayer = multiplayer;
            this.prototype = prototype;
            manipulable = new ManipulableTransform(sceneTransform);
            this.multiplayer.SimulationPose.LockRejected += SimulationPoseLockRejected;
            this.multiplayer.SimulationPose.RemoteValueChanged +=
                MultiplayerSimulationPoseChanged;

            Update().AwaitInBackground();
        }

        /// <summary>
        /// Callback for when the simulation pose value is changed in the multiplayer dictionary.
        /// </summary>
        private void MultiplayerSimulationPoseChanged()
        {
            // If manipulations are active, then I'm controlling my box position.
            if (!CurrentlyEditingScene)
            {
                CopyMultiplayerPoseToLocal();
            }
        }

        /// <summary>
        /// Handler for if the simulation pose lock is rejected.
        /// </summary>
        /// <remarks>
        /// If rejected, the manipulation is ended, and the simulation pose is set to the latest pose received, ignoring any user input. 
        /// </remarks>
        private void SimulationPoseLockRejected()
        {
            EndAllManipulations();
            CopyMultiplayerPoseToLocal();
        }

        /// <summary>
        /// Copy the pose stored in the multiplayer to the current scene transform.
        /// </summary>
        private void CopyMultiplayerPoseToLocal()
        {
            var worldPose = prototype.CalibratedSpace
                                     .TransformPoseCalibratedToWorld(multiplayer.SimulationPose
                                                                                .Value);
            worldPose.CopyToTransformRelativeToParent(sceneTransform);
        }

        /// <summary>
        /// Attempt to start a grab manipulation on this box, with a 
        /// manipulator at the current pose.
        /// </summary>
        public IActiveManipulation StartGrabManipulation(UnitScaleTransformation manipulatorPose)
        {
            if (manipulable.StartGrabManipulation(manipulatorPose) is IActiveManipulation
                    manipulation)
            {
                manipulations.Add(manipulation);
                manipulation.ManipulationEnded += () => OnManipulationEnded(manipulation);
                return manipulation;
            }

            return null;
        }

        /// <summary>
        /// Callback for when a manipulation is ended by the user.
        /// </summary>
        private void OnManipulationEnded(IActiveManipulation manipulation)
        {
            manipulations.Remove(manipulation);
            // If manipulations are over, then release the lock.
            if (!CurrentlyEditingScene)
            {
                multiplayer.SimulationPose.ReleaseLock();
                CopyMultiplayerPoseToLocal();
            }
        }

        private async Task Update()
        {
            while (true)
            {
                if (CurrentlyEditingScene)
                {
                    var worldPose = Transformation.FromTransformRelativeToParent(sceneTransform);
                    ClampToSensibleValues(worldPose);
                    var calibPose = prototype.CalibratedSpace
                                             .TransformPoseWorldToCalibrated(worldPose);
                    multiplayer.SimulationPose.UpdateValueWithLock(calibPose);
                }

                await Task.Delay(10);
            }
        }

        private void ClampToSensibleValues(Transformation worldPose)
        {
            if (float.IsNaN(worldPose.Position.x)
             || float.IsNaN(worldPose.Position.y)
             || float.IsNaN(worldPose.Position.z))
                worldPose.Position = Vector3.zero;
            worldPose.Position = Vector3.ClampMagnitude(worldPose.Position, 100f);
            
            if (float.IsNaN(worldPose.Scale.x)
             || float.IsNaN(worldPose.Scale.y)
             || float.IsNaN(worldPose.Scale.z))
                worldPose.Scale = Vector3.one;
            worldPose.Scale.x = Mathf.Clamp(worldPose.Scale.x, 0.001f, 1000f);
            worldPose.Scale.y = Mathf.Clamp(worldPose.Scale.y, 0.001f, 1000f);
            worldPose.Scale.z = Mathf.Clamp(worldPose.Scale.z, 0.001f, 1000f);
        }

        private void EndAllManipulations()
        {
            foreach (var manipulation in manipulations.ToList())
                manipulation.EndManipulation();
        }
    }
}