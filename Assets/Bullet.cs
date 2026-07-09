using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int atk;
    public Transform deadPool;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position+=Vector3.right*5*Time.deltaTime;
        if(transform.position.x > 20.5f)
        {
            //将子弹放入死亡池
            transform.SetParent(GameObject.Find("DeadPool").transform);
            //将子弹隐藏
            gameObject.SetActive(false);
        }
    }
}
