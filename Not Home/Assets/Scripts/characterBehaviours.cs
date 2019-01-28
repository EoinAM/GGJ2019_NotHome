using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{

    public class characterBehaviours : MonoBehaviour
    {

        public Slider fearMeter;
        public Text teddiesText;
        public GameObject cursorIndicator;
        public Text winGameText;
        public Text loseGameText;
        public Text doorText;
        public Text lockedDoorText;
        private int maxTeddies = 8;
        private float currentFear = 0;
        private float maxFear = 50;
        private int numOfTeddies = 0;
        private float initialTimer = 6;

        bool win = false;
        bool gameOver = false;


        // Use this for initialization
        void Start()
        {
            winGameText.enabled = false;
            loseGameText.enabled = false;
            doorText.enabled = false;
            lockedDoorText.enabled = false;
            win = false;
            gameOver = false;
            numOfTeddies = 0;
            currentFear = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (!gameOver)
            {

                var mainCamera = GetComponent<Camera>();

                // We need to actually hit an object
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin, mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100, Physics.DefaultRaycastLayers))
                {
                    GameObject hitObject = hit.collider.gameObject; // Get the gameObject of the object looked at
                    float distanceTo = hit.distance; // Get the distance to the click point

                    if (hitObject.tag == "Teddies" && distanceTo < 1.2f) // Check the tag on the object and the distance to the click point
                    {
                        cursorIndicator.SetActive(true); // Make the cursor object visible

                        // Make sure the user pressed the mouse down
                        if (Input.GetMouseButtonDown(0))
                        {
                            hitObject.SetActive(false); // Deactivate the other object

                            currentFear -= 10; // Decrease the fear
                            numOfTeddies++; // Increase the number of teddies collected
                        }
                    }
                    else
                    {
                        cursorIndicator.SetActive(false); // Deactivate the cursor
                    }

                    if (hitObject.tag == "Door" && distanceTo < 2.0f) // Check the tag on the object and the distance to the click point
                    {
                        doorText.enabled = true;
                    }
                    else
                    {
                        doorText.enabled = false;
                    }

                    if (hitObject.tag == "Locked" && distanceTo < 2.0f) // Check the tag on the object and the distance to the click point
                    {
                        lockedDoorText.enabled = true;
                    }
                    else
                    {
                        lockedDoorText.enabled = false;
                    }
                }
                else
                {
                    cursorIndicator.SetActive(false); // Deactivate the cursor
                }

                // If the fear goes below zero, reset it
                if (currentFear < 0)
                {
                    currentFear = 0.0f;
                }

                teddiesText.text = numOfTeddies.ToString(); // Update the teddies text

                if (initialTimer <= 0)
                {
                    currentFear += Time.deltaTime; // Update the fear variable
                    fearMeter.value = currentFear / maxFear; // Update the fear bar
                }
                else
                {
                    initialTimer -= Time.deltaTime;
                }

                if (numOfTeddies >= maxTeddies)
                {
                    gameOver = true;
                    win = true;
                }
                if (currentFear >= maxFear)
                {
                    gameOver = true;
                    win = false;
                }
            }
            else
            {
                cursorIndicator.SetActive(false); // Deactivate the cursor
                if (win)
                {
                    winGameText.enabled = true;
                }
                else
                {
                    loseGameText.enabled = true;
                }
            }
        }
    }
}

