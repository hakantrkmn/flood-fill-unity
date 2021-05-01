using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region singleton


    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public enum gameStates
    {
        start,
        game,
        end
    }

    public gameStates gameState;

    public Material yesil;
    public Material kirmizi;
    public Material beyaz;
    int kutuKontrol = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameState = gameStates.start;
        PlayerController.FloodFill += floodFill;
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    void floodFill(GameObject baslangicKutu)
    {
        //gelen kutu eğer yeşil veya beyazsa hiç bir şey yapılmadan fonksiyondna çıkıyoruz
        if (baslangicKutu.GetComponent<MeshRenderer>().material.name == "yesil (Instance)" || baslangicKutu.GetComponent<MeshRenderer>().material.name == "beyaz (Instance)")
        {
            return;
        }
        //eğer kırmızı veya sarıysa yeşile boyuyoruz ardından kutunun her tarafında bulunan kutuları bu fonksiyona tekrar yolluyoruz. bu şekilde rekursive bir şekilde
        //her kutu boyanmış oluyor
        else if (baslangicKutu.GetComponent<MeshRenderer>().material.name == "kirmizi (Instance)" || baslangicKutu.GetComponent<MeshRenderer>().material.name == "sari (Instance)")
        {
            if (baslangicKutu.GetComponent<MeshRenderer>().material.name == "sari (Instance)")
            {
                baslangicKutu.gameObject.tag = "kKutu";
                baslangicKutu.GetComponent<MeshRenderer>().material = yesil;
            }
            else
            {
                baslangicKutu.GetComponent<MeshRenderer>().material = yesil;

            }
        }
        var rightBox = Physics.OverlapBox(new Vector3(baslangicKutu.transform.position.x + 1, -0, baslangicKutu.transform.position.z), new Vector3(0.1f, 0.1f, 0.1f));
        var leftBox = Physics.OverlapBox(new Vector3(baslangicKutu.transform.position.x - 1, -0, baslangicKutu.transform.position.z), new Vector3(0.1f, 0.1f, 0.1f));
        var upBox = Physics.OverlapBox(new Vector3(baslangicKutu.transform.position.x, -0, baslangicKutu.transform.position.z + 1), new Vector3(0.1f, 0.1f, 0.1f));
        var downBox = Physics.OverlapBox(new Vector3(baslangicKutu.transform.position.x, -0, baslangicKutu.transform.position.z - 1), new Vector3(0.1f, 0.1f, 0.1f));
        if (rightBox.Length > 0)
        {
            floodFill(rightBox[0].gameObject);
        }
        if (leftBox.Length > 0)
        {
            floodFill(leftBox[0].gameObject);
        }
        if (downBox.Length > 0)
        {
            floodFill(downBox[0].gameObject);
        }
        if (upBox.Length > 0)
        {
            floodFill(upBox[0].gameObject);
        }

        checkAllBoxes();


    }

    private void checkAllBoxes()
    {
        var kutular = GameObject.FindGameObjectsWithTag("kKutu");
        var SariKutular = GameObject.FindGameObjectsWithTag("engel");
        foreach (var item in kutular)
        {
            if (item.GetComponent<MeshRenderer>().material.name == "kirmizi (Instance)")
            {
                kutuKontrol = 1;
                break;
            }
        }
        foreach (var item in SariKutular)
        {
            if (item.GetComponent<MeshRenderer>().material.name == "sari (Instance)")
            {
                kutuKontrol = 1;
                break;
            }
        }

        if (kutuKontrol == 0)
        {
            SceneManager.LoadScene(1);

        }
        kutuKontrol = 0;
    }
}
