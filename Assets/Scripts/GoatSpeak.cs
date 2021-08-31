using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatSpeak : MonoBehaviour
{
    public Transform TargetToLook = null;
    public bool invert = false;
    // Start is called before the first frame update
    private void Reset()
    {
        TargetToLook = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = invert ? Quaternion.LookRotation(-TargetToLook.forward, TargetToLook.up) : TargetToLook.rotation;
    }

}
