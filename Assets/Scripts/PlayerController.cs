using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static event Action<GameObject> FloodFill;

    public bool canMove=true;

    public Vector3 ilkKonum;
    public Vector3 sonKonum;


    void Start()
    {
        ilkKonum = transform.position;
    }



    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState==GameManager.gameStates.game)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sonKonum = transform.position;
            StartCoroutine(Move(0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sonKonum = transform.position;
            StartCoroutine(Move(0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            sonKonum = transform.position;
            StartCoroutine(Move(1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            sonKonum = transform.position;
            StartCoroutine(Move(-1, 0));
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            StopAllCoroutines();
        }

    }

    IEnumerator Move(int Zaxis,int Xaxis)
    {
        if (canMove)
        {
            transform.position = transform.position + new Vector3(Xaxis, 0, Zaxis);
            yield return new WaitForSeconds(1);
            StartCoroutine(Move(Zaxis, Xaxis));
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        //engele çarparsak oyun bitiyor
        if (collision.gameObject.tag == "engel")
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        //duvarlara çarparsak kutu duruyor ve gerekli hesaplamalar yapıılıyor
        if (collision.transform.tag == "Wall")
        {
            canMove = false;
            transform.position = sonKonum;
            sonKonum = transform.position;
            Vector3 matris = sonKonum - ilkKonum;
            var box = Physics.OverlapBox(new Vector3((ilkKonum.x + sonKonum.x) / 2, -0.5f, (ilkKonum.z + sonKonum.z) / 2), new Vector3(0.1f, 0.1f, 0.1f));
            FloodFill(box[0].gameObject);
            ilkKonum = sonKonum;
            canMove = true;


        }
    }
}
