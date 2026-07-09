using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plant_0 : MonoBehaviour
{
    public float timer;
    public float CD;
    public GameObject sunObj;
    // Start is called before the first frame update
    void Start()
    {
        CD=Random.Range(15, 19);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= CD)
        {
            GameObject obj = GameObject.Instantiate(sunObj);
            obj.transform.SetParent(GameObject.Find("SunManager").transform);
            obj.transform.position=transform.position+new Vector3(0.81f,0.7f,0);
            timer = 0;
            CD=Random.Range(15, 19);
        }
    }
}
