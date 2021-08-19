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

    public void onTriggerEnter(Collider other)
    {
        Debug.Log("Chomp chomp");
        /*
        if(other.gameObject.CompareTag("Object"))
        {
            other.transform.parent = this.gameObject.transform;
        }
        */
    }
}
