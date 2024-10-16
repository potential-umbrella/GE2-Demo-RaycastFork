using UnityEngine;

namespace Assets.Scripts.Managers
{
    internal class WeaponManager : MonoBehaviour
    {
        // camera refs.
        [SerializeField] CameraManager camMan;
        Camera playerCam;

        [SerializeField] float defaultDistance = 10;
        [SerializeField] int raycastInterval = 5;
        int tickCount = 1;

        //
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
                // raycast and draw ray. DO NOT DO THIS EVERY FRAME! I'M JUST SILLY!
                if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, 10, ground.value))
                {
                    distance = hit.distance;
                }
                Debug.Log($"Distance is {distance}");

                // Debug.
                Vector3 camPos = playerCam.transform.position;
                Debug.DrawLine(camPos, camPos + playerCam.transform.forward * 10);

                RaycastHit[] hits = Physics.RaycastAll(playerCam.transform.position, playerCam.transform.forward, distance, cubeFilter.value);
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
