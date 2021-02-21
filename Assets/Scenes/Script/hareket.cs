using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hareket : MonoBehaviour
{

    int kutuKontrol = 0;
    public Material yesil;
    public Material kirmizi;
    public Material beyaz;

    public Vector3Int gridPosition;
    public float gridMoveTimer;
    public float gridMoveTimerMax;
    public Vector3Int gridMoveDirection;


    public Vector3 ilkKonum;
    public Vector3 sonKonum;

    [SerializeField]
    bool canMove = true;

    private void Awake()
    {
        //başladığı konumu kaydediyoruz.
        ilkKonum = transform.position;
        // kutunun başlangıç pozisyonunu kaydediyoruz.
        gridPosition = new Vector3Int(0, 1, -5);
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector3Int(0, 0, 0);
    }
     void Update()
    {
        //yön tuşlarına basınca hareketi true yapıp hangi yönde gideceğine göre atama yapıyoruz
        if (Input.GetKey(KeyCode.UpArrow))
        {

            canMove = true;

            gridMoveDirection.x = 0;
            gridMoveDirection.y = 0;
            gridMoveDirection.z = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            canMove = true;
            gridMoveDirection.x = 0;
            gridMoveDirection.y = 0;
            gridMoveDirection.z = -1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            canMove = true;
            gridMoveDirection.x = -1;
            gridMoveDirection.y = 0;
            gridMoveDirection.z = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            canMove = true;
            gridMoveDirection.x = 1;
            gridMoveDirection.y = 0;
            gridMoveDirection.z = 0;
        }
        //hareket eğer true ise belirlediğimiz değişkenlere göre hareket ediyoruz
        if (canMove==true)
        {
            gridMoveTimer += Time.deltaTime;
            if (gridMoveTimer >= gridMoveTimerMax)
            {
                gridPosition += gridMoveDirection;
                gridMoveTimer -= gridMoveTimerMax;
            }
            transform.position = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z);
        }
        

       
    }

  

    private void OnCollisionEnter(Collision collision)
    {
        //engele çarparsak oyun bitiyor
        if (collision.gameObject.tag == "engel")
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        //duvarlara çarparsak kutu duruyor ve gerekli hesaplamalar yapıılıyor
        if (collision.transform.tag == "beyaz")
        {
            canMove = false;
            // hareket float olduğu için gerekli değerleri yuvarlayarak belli bir konumda durmasını sağlıyoruz
            transform.position = new Vector3(int.Parse(transform.position.x.ToString("f0")), int.Parse(transform.position.y.ToString("f0")), int.Parse(transform.position.z.ToString("f0")));
            gridPosition.x = (int)transform.position.x;
            gridPosition.y = (int)transform.position.y;
            gridPosition.z = (int)transform.position.z;
            sonKonum = transform.position;
            Vector3 matris = sonKonum - ilkKonum;
            //boyanacak tarafı seçmek için ilk konum ile son konum arasındaki bir noktayı seçtim. bu değiştirilebilir. ilk konumla son konum arasındaki
            // kutuyu buluyoruz ve bunu floodfill fonksiyonuna yolluyoruz
            var box = Physics.OverlapBox(new Vector3((ilkKonum.x + sonKonum.x)/2, -0.5f, (ilkKonum.z + sonKonum.z)/2), new Vector3(0.1f, 0.1f, 0.1f));
            floodFill(box[0].gameObject);

            //burda tüm kutuları boyadık mı onun kontrolu var. eğer tümü boyanmışsa sonraki lvla geçiyor.
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
            Debug.LogError(kutuKontrol);
            if (kutuKontrol==0)
            {
                    SceneManager.LoadScene(1);
                
            }
            kutuKontrol = 0;
            //sonraki harekete geçmeden önce ilk konumu güncelliyoruz
            ilkKonum = sonKonum;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="engel")
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }


    void floodFill(GameObject baslangicKutu)
    {
        //gelen kutu eğer yeşil veya beyazsa hiç bir şey yapılmadan fonksiyondna çıkıyoruz
        if (baslangicKutu.GetComponent<MeshRenderer>().material.name== "yesil (Instance)" || baslangicKutu.GetComponent<MeshRenderer>().material.name == "beyaz (Instance)")
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
        var rightBox = Physics.OverlapBox(new Vector3(baslangicKutu.transform.position.x + 1, -0.5f, baslangicKutu.transform.position.z), new Vector3(0.1f, 0.1f, 0.1f));
        var leftBox = Physics.OverlapBox(new Vector3(baslangicKutu.transform.position.x - 1, -0.5f, baslangicKutu.transform.position.z), new Vector3(0.1f, 0.1f, 0.1f));
        var upBox = Physics.OverlapBox(new Vector3(baslangicKutu.transform.position.x , -0.5f, baslangicKutu.transform.position.z + 1), new Vector3(0.1f, 0.1f, 0.1f));
        var downBox = Physics.OverlapBox(new Vector3(baslangicKutu.transform.position.x , -0.5f, baslangicKutu.transform.position.z -1), new Vector3(0.1f, 0.1f, 0.1f));
        if (rightBox.Length>0)
        {
            floodFill(rightBox[0].gameObject);
        }
        if (leftBox.Length>0)
        {
            floodFill(leftBox[0].gameObject);
        }
        if (downBox.Length>0)
        {
            floodFill(downBox[0].gameObject);
        }
        if (upBox.Length>0)
        {
            floodFill(upBox[0].gameObject);
        }


    }
}



