using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour {
    public Light[] lights;

    

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(Flicker());
	}
	
	// Update is called once per frame
	IEnumerator Flicker ()
    {
        float flickerSpeed;

        while (true)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = true;
            }

            flickerSpeed = Random.Range(0.2f, 2.0f);

            yield return new WaitForSeconds(flickerSpeed);
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = false;
            }

            flickerSpeed = Random.Range(0.2f, 2.0f);

            yield return new WaitForSeconds(flickerSpeed);
        }

    }
}
