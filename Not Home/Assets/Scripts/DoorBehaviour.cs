using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Utility
{

    public class DoorBehaviour : MonoBehaviour
    {

        private bool open = false;
        private bool active = false;
        private bool locked = true;
        private GameObject hitHinge;
        private float closedRotation;
        private float openRotation;
        public float rotationSpeed = 80.0f;

        // Use this for initialization
        void Start()
        {
            gameObject.tag = "Locked";
            hitHinge = gameObject.transform.parent.gameObject;
            closedRotation = gameObject.transform.rotation.y;
            openRotation = closedRotation + 0.8f;
        }

        // Update is called once per frame
        void Update()
        {


            var mainCamera = FindCamera();

            if (!locked)
            {
                // Make sure the user pressed the mouse down
                if (Input.GetMouseButtonDown(0) && !active)
                {
                    // Check if the player is looking at the door
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin, mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100, Physics.DefaultRaycastLayers))
                    {
                        GameObject hitObject = hit.collider.gameObject; // Get the gameObject of the object looked at
                        float distanceTo = hit.distance; // Get the distance to the click point

                        if (hitObject != null && hitObject == gameObject && distanceTo < 3.0f)
                        {
                            active = true;
                        }
                    }
                }

                if (active && hitHinge != null)
                {
                    if (!open)
                    {
                        if (gameObject.transform.rotation.y <= openRotation)
                        {
                            hitHinge.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
                        }
                        else
                        {
                            open = true;
                            active = false;
                        }
                    }
                    else
                    {
                        if (gameObject.transform.rotation.y >= closedRotation)
                        {
                            hitHinge.transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
                        }
                        else
                        {
                            open = false;
                            active = false;
                        }
                    }
                }
            }
        }

        private Camera FindCamera()
        {
            if (GetComponent<Camera>())
            {
                return GetComponent<Camera>();
            }

            return Camera.main;
        }

        void OnTriggerEnter(Collider t_other)
        {
            if (t_other.gameObject.tag == "Key" && gameObject.tag == "Locked")
            {
                t_other.gameObject.SetActive(false);
                locked = false;
                gameObject.tag = "Door";
            }
        }
    }
}
