using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{
    public Text sunPointTxt;
    //定义currentPlant
    public Image currentPlant;

    public Transform bk;

    // Start is called before the first frame update

    void Start()
    {
        //游戏一开始禁用这张图片
        currentPlant.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        sunPointTxt.text = GameDate.sunPoint.ToString();
        //让currentPlant跟随鼠标移动
        currentPlant.transform.position= Input.mousePosition;
        if(GameDate.currentPlantId==-1)
        {
            currentPlant.gameObject.SetActive(false);
            for(int i=0;i<bk.childCount;i++)
            {
                bk.GetChild(i).gameObject.layer = 2;
            }
        }
        else
        {
            for (int i = 0; i < bk.childCount; i++)
            {
                bk.GetChild(i).gameObject.layer = 0;
            }
        }

    }
}
