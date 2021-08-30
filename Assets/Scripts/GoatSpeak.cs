using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatSpeak : MonoBehaviour
{
    public TextWriter endSpeech;
    // Start is called before the first frame update
    void Start()
    {
        endSpeech = GetComponent<TextWriter>();
        string test = endSpeech.text[0];
        Debug.Log(test);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator WaitForWords()
    {
        yield return new WaitForSeconds(10);
    }
}
