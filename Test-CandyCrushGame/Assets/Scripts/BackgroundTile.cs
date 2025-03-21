using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public GameObject[] Dots;
    // Start is called before the first frame update
    void Start()
    {
        Iniztialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Iniztialize() 
    {
        int dotToUse = Random.Range(0, Dots.Length);
        GameObject dot = Instantiate(Dots[dotToUse], transform.position, Quaternion.identity);
        dot.transform.parent = this.transform;
        dot.name = this.gameObject.name;
    }
}
