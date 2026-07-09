using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class plant_1 : MonoBehaviour
{
    //定义一个计时器
    public float timer;
    //定义一个CD时间
    public float CD;
    //定义一个子弹预制体
    public GameObject peaBulletObj;
    //定义子弹生成数量
    public int Count;
    //定义生成池和死亡池
    public Transform livePool;
    public Transform deadPool;
    // Start is called before the first frame update
    void Start()
    {
        livePool = GameObject.Find("LivePool").transform;
        deadPool = GameObject.Find("DeadPool").transform;

    }

    // Update is called once per frame
    void Update()
    {
        timer  += Time.deltaTime;
        if (timer >= CD)
        {
            //生成一颗子弹
            GameObject go;
            //去死亡池中寻找子弹
            if (deadPool.childCount != 0)
                {
                    go = deadPool.GetChild(0).gameObject;
                    //将go放到生成池中
                    go.transform.SetParent(livePool);
                    
                }
            else
                {
                    //生成一颗子弹
                    go=GameObject.Instantiate(peaBulletObj);
                    Count++;
            }
            go.transform.SetParent(livePool);
            go.transform.position = transform.position + new Vector3(0.5f, 0.5f, 0);
            //将go激活（显示）
            go.SetActive(true);
            print("生成一颗子弹");
            
            //计时器清零
            timer = 0;
            
            
        }
    }
}
