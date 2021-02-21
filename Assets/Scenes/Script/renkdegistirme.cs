using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renkdegistirme : MonoBehaviour
{
    public Material[] material;
    MeshRenderer rend;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.tag == "Box")
    //    {
    //        rend.sharedMaterial = material[1];
    //    }
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            rend.sharedMaterial = material[1];
        }
    }


}
