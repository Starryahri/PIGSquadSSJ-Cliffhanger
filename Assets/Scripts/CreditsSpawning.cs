using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSpawning : MonoBehaviour
{
    public GameObject[] trash;

    public int randnum;
    public int randomXPos;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -0.2f, 0);
        Destroy(this.gameObject, 30);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("TrashSpawner");
        randnum = Random.Range(0, trash.Length);
        randomXPos = Random.Range(270, 300);
    }

    IEnumerator TrashSpawner()
    {
        yield return new WaitForSeconds(2);
        Instantiate(trash[randnum], new Vector3(randomXPos, 677f, 0f), Quaternion.identity);
        //go.GetComponent<Rigidbody>().

    }
}
