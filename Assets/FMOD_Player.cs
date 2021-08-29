using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_Player : MonoBehaviour
{
    // Start is called before the first frame update
    void PlaySound(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, GetComponent<Transform>().position);
    }

}
