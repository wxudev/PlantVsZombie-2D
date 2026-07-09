using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
/*随机
CD时间
生成位置x[-7f,4.5f] y 5.66f
终点位置x         y[-4.44f,3.5f]
 */
public class SunManager : MonoBehaviour
{
    public float timer;
    public float CD;
    public GameObject sunObj;
    // Start is called before the first frame update
    void Start()
    {
        CD= Random.Range(5, 9);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= CD)
        {
            //生成阳光
            GameObject obj = GameObject.Instantiate(sunObj);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = new Vector3(Random.Range(-7f, 4.5f), 5.66f, 0);

            obj.transform.DOLocalMoveY(Random.Range(-4.44f, 3.5f), 0.5f);
            timer = 0;
            CD=Random.Range(5, 9);

        }
    }
}
