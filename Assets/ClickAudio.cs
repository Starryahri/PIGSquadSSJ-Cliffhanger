using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAudio : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Interaction/BAHH", GetComponent<Transform>().position);
            print("BAAHH");
        }
    }

}


