using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject goodItem;
    public GameObject badItem;

    [Range(10, 50)]
    public int goodItemCount = 30;
    [Range(10, 50)]
    public int badItemCount = 20;

    public List<GameObject> goodList = new List<GameObject>();  //doesnt need to be public but for checking
    public List<GameObject> badList = new List<GameObject>();


    //Initializes stage: basically restarts it, deletes items and randomizes again
    public void InitStage() {

        foreach (var obj in goodList) {
            Destroy(obj);
        }
        
        
        foreach (var obj in badList) {
            Destroy(obj);
        }

        goodList.Clear();
        badList.Clear();


        //good item
        for (int i = 0; i < goodItemCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-22.0f, 22.0f), 0.05f, Random.Range(-22.0f, 22.0f));  //randomizes position
            Quaternion rotation = Quaternion.Euler(Vector3.up * Random.Range(0,360));
            goodList.Add(Instantiate(goodItem, transform.position + position, rotation, transform));
        
        }

        //bad item
        for (int i = 0; i < badItemCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-22.0f, 22.0f), 0.05f, Random.Range(-22.0f, 22.0f));  //randomizes position
            Quaternion rotation = Quaternion.Euler(Vector3.up * Random.Range(0,360));
            badList.Add(Instantiate(badItem, transform.position + position, rotation, transform));
        }
    
    }

    // Start is called before the first frame update
    void Start()
    {
        InitStage();   
    }


}
