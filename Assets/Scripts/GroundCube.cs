using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCube : MonoBehaviour
{

    public Material color;
    Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rend.material = color;
        }
    }
}
