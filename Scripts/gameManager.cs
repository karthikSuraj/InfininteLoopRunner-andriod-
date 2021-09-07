using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance { get; set; }

    public GameObject mainPlayer;
    public GameObject coinPrefab;
    public GameObject[] SpawnPlatforms;
    public List<GameObject> pooledPlatforms;
    public int numberofCoins,numberOfCoinsToSpawn;
    public float Score;

    public static bool gameOver;
    public GameObject gameOverPanel;

    
    public GameObject buttonsPanel;


    public Transform front;
    public Transform back;

    public Vector3 distance;

    public Text coinsText;
    public Text scoreText;

    public Text finalScore;
    public Text coinsCollected;

    public GameObject nextLevel;

    private GameObject MainCamera;

    GameObject obj;

    


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        numberofCoins = 0;
        Score = 0;
        gameOver = false;
        
        Time.timeScale = 1;
        mainPlayer = GameObject.FindGameObjectWithTag("Player");
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        distance = MainCamera.transform.position - mainPlayer.transform.position;


        pooledPlatforms = new List<GameObject>();


        for (int j = 0; j <= SpawnPlatforms.Length - 1; j++)
        {
            obj = Instantiate(SpawnPlatforms[j]);
            obj.SetActive(false);
            pooledPlatforms.Add(obj);
        }


    }

    public GameObject GetPooledPlatform()
    {
        GameObject a;
        for (int i = 0; i < SpawnPlatforms.Length; i++)
        {
            if (!pooledPlatforms[i].activeInHierarchy)
            {

                a = pooledPlatforms[i];
                return a;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

        Score = (int)mainPlayer.transform.position.z/3;
        coinsText.text = "Coins : " + gameManager.instance.numberofCoins;
        scoreText.text = "Score : " + gameManager.instance.Score;

        coinsCollected.text = "Coins Collected :" + gameManager.instance.numberofCoins;
        finalScore.text = "Final Score :" + gameManager.instance.Score;


        for (int i = 0; i < SpawnPlatforms.Length; i++)
        {
            GameObject Clone = gameManager.instance.GetPooledPlatform();
            if (mainPlayer.transform.position.z - pooledPlatforms[i].transform.position.z <= 100f)
            {
                if (Clone != null)
                {
                    Clone.transform.position = front.transform.position;
                    Clone.transform.rotation = front.transform.rotation ;
                    Clone.SetActive(true);
                    front.transform.position += new Vector3(0, 0, 100f);

                }
            }

            if (mainPlayer.transform.position.z - pooledPlatforms[i].transform.position.z >= 100f)
            {
                pooledPlatforms[i].SetActive(false);
                
            }
        }

        if(gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            buttonsPanel.SetActive(false);
        }

        if(gameManager.instance.numberofCoins>=100)
        {
            nextLevel.SetActive(true);
            Time.timeScale = 0;
        }

    }

    private void LateUpdate()
    {
        followPlayer(MainCamera);
    }



    public void followPlayer(GameObject camera)
    {

        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, distance.z + mainPlayer.transform.position.z);

    }
    
}
