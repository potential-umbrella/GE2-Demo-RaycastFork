using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Handles weaponry, target selection and firing behavior.
    /// </summary>
    public class WeaponManager : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] bool debugMode = false;

        [Header("Refs")]
        [SerializeField] InputManager inputManager;
        [SerializeField] CameraManager cameraManager;

        // In a more robust system, we'd use a weapon class, but I'm lazy.
        [Header("Weapon Details")]
        [SerializeField] float maxRange = 10;

        List<TargetableObject> targets = new();

        private void Awake()
        {
            if(inputManager == null) { Debug.LogException(new Exception("inputman was null. this bad.")); }
            if(cameraManager == null) { Debug.LogException(new Exception("cameraman was null. this bad.")); }
        }

        private void Update()
        {
            UpdateTargets();
        }

        /// <summary>
        /// Called when we want to update the targets.
        /// </summary>
        public void UpdateTargets()
        {
            // Reset targeting state and then discard.
            foreach(TargetableObject target in targets) { target.SetTarget(false); }
            targets.Clear();

            // Get new references
            RaycastHit[] allHits = Physics.RaycastAll(transform.position, cameraManager.playerCamera.transform.forward, maxRange);
            if (debugMode) { Debug.Log($"Caught {allHits.Length} hits."); }

            foreach (RaycastHit hitData in allHits)
            {
                if (hitData.transform.gameObject.TryGetComponent(out TargetableObject target))
                {
                    targets.Add(target);
                    target.SetTarget(true);
                }
            }
        }

        public void Fire()
        {
            // Update targets just in case they are out of date.
            UpdateTargets();

            // For each target, if its a box, disable it.
            foreach (TargetableObject target in targets)
            {
                if (target is DestructableBox)
                {
                    target.gameObject.SetActive(false);
                }
            }
        }
    }
}