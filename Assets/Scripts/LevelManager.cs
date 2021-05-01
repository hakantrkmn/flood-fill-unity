using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{



    public GameObject Kkutu;
    public GameObject groundParent;

    void Start()
    {
        createLevel(new Vector3(0,0,-5));
        GameManager.Instance.gameState = GameManager.gameStates.game;
    }

    private void createLevel(Vector3 point)
    {
        var box = Physics.OverlapBox(point, new Vector3(0.1f, 0.1f, 0.1f));
        if (box.Length!=0)
        {
            if (box[0].gameObject.GetComponent<MeshRenderer>().material.name == "sari (Instance)" || box[0].gameObject.GetComponent<MeshRenderer>().material.name == "beyaz (Instance)")
            {
                return;
            }
            //eğer kırmızı veya sarıysa yeşile boyuyoruz ardından kutunun her tarafında bulunan kutuları bu fonksiyona tekrar yolluyoruz. bu şekilde rekursive bir şekilde
            //her kutu boyanmış oluyor
            else if (box[0].gameObject.GetComponent<MeshRenderer>().material.name == "kirmizi (Instance)")
            {
                return;
            }
            else
            {
                Instantiate(Kkutu, point, Quaternion.identity, groundParent.transform);
            }


            createLevel(new Vector3(point.x + 1, -0, point.z));
            createLevel(new Vector3(point.x - 1, -0, point.z));
            createLevel(new Vector3(point.x, -0, point.z + 1));
            createLevel(new Vector3(point.x, -0, point.z - 1));

        }
        else
        {
            Instantiate(Kkutu, point, Quaternion.identity, groundParent.transform);
            createLevel(new Vector3(point.x + 1, -0, point.z));
            createLevel(new Vector3(point.x - 1, -0, point.z));
            createLevel(new Vector3(point.x, -0, point.z + 1));
            createLevel(new Vector3(point.x, -0, point.z - 1));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
