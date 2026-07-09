using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 卡牌状态
 * CD：冷却中
 * NoSun：没有足够的阳光
 * Ready：准备就绪
 */

public enum CardState
{
    CD,
    NoSun,
    Ready
}
public class Card : MonoBehaviour
{
    //植物id
    public int id;
    //卡牌状态
    public CardState state;
    //定义这三张图片
    public Button ready;
    public GameObject noSun;
    public Image cdImg;
    //定义cd时间
    public float timer;
    public float CD;
    //定义植物当前需要的阳光数
    public int needSun;
    //定义UIRoot
    public UIRoot uiroot;
    // Start is called before the first frame update
    void Start()
    {
        state = CardState.CD;

        ready.onClick.AddListener(OnclickedReady);
        
    }

    private void OnclickedReady()
    {
        print("点击了准备就绪按钮");
        //确认当前所选植物
        
        //GameDate.sunPoint-= needSun; 

        uiroot.currentPlant.gameObject.SetActive(true);
        //给图片赋值
        //动态加载图片：Resources.Load<Sprite>("路径")
        uiroot.currentPlant.sprite=Resources.Load<Sprite>("Card/"+id);

        GameDate.currentPlantId = id;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case CardState.CD:
                cdImg.gameObject.SetActive(true);
                noSun.gameObject.SetActive(true);
                ready.gameObject.SetActive(true);
                //计时器开始计时
                //Time.deltaTime：每帧的时间
                timer += Time.deltaTime;
                cdImg.fillAmount=(CD-timer)/ CD;
                //当计时器大于等于cd时间，切换状态
                if (timer >= CD)
                {
                    state = CardState.NoSun;
                    timer = 0;
                }
                break;

            case CardState.NoSun:
                cdImg.gameObject.SetActive(false);
                noSun.gameObject.SetActive(true);
                ready.gameObject.SetActive(true);
                //判断阳光是否充足
                   if (GameDate.sunPoint >= needSun)
                    {
                        state = CardState.Ready;
                }
                break;

            case CardState.Ready:
                cdImg.gameObject.SetActive(false);
                noSun.gameObject.SetActive(false);
                ready.gameObject.SetActive(true);
                if(GameDate.sunPoint < needSun)
                {
                    state = CardState.NoSun;
                }
                break;
        }
    }
}
