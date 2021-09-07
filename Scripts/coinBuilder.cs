using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinBuilder : MonoBehaviour
{
    public static coinBuilder sharedInstance;
    private List<GameObject> poolobjects;
    private GameObject coinPrefab;
    int numberOfCoins,difulcty=0;

    // Start is called before the first frame update
    public void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
        else if (sharedInstance != null)
            DontDestroyOnLoad(gameObject);

    }
    public void Start()
    {
        numberOfCoins = gameManager.instance.numberOfCoinsToSpawn;
      
        coinPrefab = gameManager.instance.coinPrefab;
        poolobjects = new List<GameObject>();
        for (int i = 0; i <= numberOfCoins; i++)
        {


            GameObject obj = (GameObject)Instantiate(coinPrefab);
            obj.transform.position = new Vector3(Random.Range(-2.4f, 4), 1, Random.Range(i, 100 + difulcty));
            obj.SetActive(true);
            poolobjects.Add(obj);
        }
    }

    // Update is called once per frame
    public void Update()
    {

        difulcty = UIManager.sharedInstance.levelDefulty;

        for (int i = 0; i < numberOfCoins; i++)
        {
            print(difulcty);

            if ((transform.position.z - poolobjects[i].transform.position.z >= 10) || !poolobjects[i].activeInHierarchy)

            {
               

                poolobjects[i].SetActive(false);
                GameObject clonePlatform = coinBuilder.sharedInstance.AllocatePoolIteam();

                if (clonePlatform != null)
                {
                    clonePlatform.transform.position = transform.position + new Vector3(-transform.position.x + Random.Range(-4f, 4), -transform.position.y +1, Random.Range(1, 100+difulcty));


                    clonePlatform.SetActive(true);
                }
            }


        }

    }


    public GameObject AllocatePoolIteam()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            if (!poolobjects[i].activeInHierarchy)
                return poolobjects[i];

        }
        return null;
    }

    

}
