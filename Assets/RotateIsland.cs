using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIsland : MonoBehaviour
{
    public float rotateSpeed = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0f, rotateSpeed, 0f));

    }
}
