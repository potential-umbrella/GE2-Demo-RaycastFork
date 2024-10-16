using UnityEngine;

namespace Assets.Scripts.Managers
{
    internal class WeaponManager : MonoBehaviour
    {
        // camera refs.
        [SerializeField] CameraManager camMan;
        Camera playerCam;

        [SerializeField] float defaultDistance = 10;

        // ticking stuff. better to encapsulate this into a "tickprovider" class or something.
        [SerializeField] int raycastInterval = 5;
        int tickCount = 1;

        // Layer masks.
        [SerializeField] LayerMask cubeFilter;
        [SerializeField] LayerMask ground;

        private void Start()
        {
            playerCam = camMan.playerCamera;
        }

        private void FixedUpdate()
        {
            // Tick system to not blow up the computer.
            tickCount++;
            if (tickCount >= raycastInterval)
            {
                tickCount = 1;

                float distance = defaultDistance;

                // Get the camera's position, which we'll use as the origin.
                Vector3 camPos = playerCam.transform.position;
                // get the camera's forward direction, which we use as the raycast's direction.
                Vector3 camForward = playerCam.transform.forward;

                // raycast and draw ray. DO NOT DO THIS EVERY FRAME! (I don't do this anymore! :) )
                if (Physics.Raycast(camPos, camForward, out RaycastHit hit, 10, ground.value))
                {
                    distance = hit.distance;
                }
                Debug.Log($"Distance is {distance}");

                // Debug. Adding camera position to an upscaled forward vector makes an end point further out.
                Debug.DrawLine(camPos, camPos + camForward * distance);

                RaycastHit[] hits = Physics.RaycastAll(camPos, camForward, distance, cubeFilter.value);
                foreach (RaycastHit hit2 in hits)
                {
                    if (hit2.collider.TryGetComponent(out Renderer renderer))
                    {
                        renderer.material.color = Color.red;
                    }
                }
                Debug.Log($"Hit {hits.Length} cubes!");
            }
            Debug.Log($"Ticknum = {tickCount}.");
        }
    }
}
