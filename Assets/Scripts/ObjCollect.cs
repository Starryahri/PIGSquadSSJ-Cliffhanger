using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ObjCollect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onTriggerEnter(Collider other)
    {
        Debug.Log("Chomp chomp");
        /*
        if(other.gameObject.CompareTag("Collectible"))
        {
            other.transform.parent = this.gameObject.transform;
        }
        */
    }

}
