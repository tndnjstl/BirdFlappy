using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OneCommand : MonoBehaviour
{
    public GameObject Column, Floor, White;
    public GameObject Quit;
    public AudioSource BirdSound_1, BirdSound_2, BirdSound_3, BirdSound_4;
    public Text Score;
    Rigidbody2D rig;
    float NextTime = 0;
    int i, j, BestScore = 0;
    bool Stop = false;
    GameObject[] gameObjects = new GameObject[3];


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.AddForce(Vector3.up *270); //초반에 270만큼 위로 날라감
        BirdSound_1.Play();
    }

    //1초에 60번을 계속 호출됨
    void Update()
    {
        //뒤로가기시 종료
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        //새
        GetComponent<Animator>().SetFloat("Velocity", rig.velocity.y);

        //맨 위 제한
        if(transform.position.y > 4.75f)
        {
            transform.position = new Vector3(-1.5f, 4.75f, 0f);
        }

        //맨 아리 제한
        if(transform.position.y < -2.55f)
        {   
            rig.simulated = false;
            GameOver();
        }

        //위로 올라갈 경우 고개를 든다
        if(rig.velocity.y > 0)
        {
            transform.rotation = Quaternion.Euler(0,0,Mathf.Lerp(transform.rotation.z, 30f, rig.velocity.y / 8f));
        }
        //내려갈 땐 고개를 바닥으로 내린다
        else
        {
            transform.rotation = Quaternion.Euler(0,0, Mathf.Lerp(transform.rotation.z, -90f, -rig.velocity.y / 8));
        }

        if(Stop)
        {
            return;
        }

        if((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            rig.velocity = Vector3.zero;
            rig.AddForce(Vector3.up *270); //초반에 270만큼 위로 날라감
            BirdSound_1.Play();
        }




        //기둥생성기
        if (Time.time > NextTime)
        {
            NextTime = Time.time + 1.7f;
            gameObjects[j] = (GameObject)Instantiate(Column, new Vector3(4, Random.Range(-1f, 3.2f), 0), Quaternion.identity);

            if (++j == 3)
            {
                j = 0;
            }
        }
        if (gameObjects[0])
        {
            gameObjects[0].transform.Translate(-0.03f, 0, 0);
            if(gameObjects[0].transform.position.x < -4)
            {
                Destroy(gameObjects[0]);
            }
        }

        if (gameObjects[1])
        {
            gameObjects[1].transform.Translate(-0.03f, 0, 0);
            if (gameObjects[1].transform.position.x < -4)
            {
                Destroy(gameObjects[1]);
            }
        }

        if (gameObjects[2])
        {
            gameObjects[2].transform.Translate(-0.03f, 0, 0);
            if (gameObjects[2].transform.position.x < -4)
            {
                Destroy(gameObjects[2]);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        // 컬럼을 통과하면 접수1점
        if (col.gameObject.name == "Column(Clone)")
        {
            Score.text = (++i).ToString();
            BirdSound_2.Play();
        }
        //컬럼을 통과하지 못하면 게임 오버
        else if(!Stop)
        {
            rig.velocity = Vector3.zero; //속도 멈춤
            BirdSound_4.Play();
            GameOver();
        }
    }

    void GameOver()
    {
        // 게임오버
        Debug.Log("GameOver");
        if (!Stop) BirdSound_3.Play();
        Stop = true;
        Floor.GetComponent<Animator>().enabled = false;
        White.SetActive(true);
        Score.gameObject.SetActive(false);
        if(PlayerPrefs.GetInt("BestScore", 0) < int.Parse(Score.text)) PlayerPrefs.SetInt("BestScore", int.Parse(Score.text));
        if(transform.position.y < -2.55f)
        {
            Quit.SetActive(true);
            Quit.transform.Find("ScoreScreen").GetComponent<Text>().text = Score.text;
            Quit.transform.Find("BestScreen").GetComponent<Text>().text = PlayerPrefs.GetInt("BestScore").ToString();
        }
    }


    public void Restart()
    {
        // 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
