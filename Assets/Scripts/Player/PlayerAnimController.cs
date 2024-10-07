using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    internal class PlayerAnimController:MonoBehaviour
    {
        [SerializeField]Animator animator;
        [SerializeField] float speedScale = 8;
        Vector3 pos;
        Quaternion rot;

        private void Awake()
        {
            pos = transform.localPosition;
            rot = transform.localRotation;
        }

        private void Update()
        {
            // For some silly reason rotation and position is really broken. Forcibly fix that every frame.
            transform.localRotation = rot;
            transform.localPosition = pos;
        }

        public void SetAnimatorStateWithSpeed(Vector3 velocity)
        {
            Vector2 hVel = new(velocity.x, velocity.z);

            animator.SetFloat("motionState", hVel.magnitude/speedScale);
            Debug.Log($"{animator.GetFloat("motionState")}, {hVel.magnitude}");
        }
    }
}
