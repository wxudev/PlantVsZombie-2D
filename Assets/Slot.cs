using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    //定义一个数组；类型为Card
    public List<Card> Cards;
    //定义能否种植植物的bool变量
    public bool isHavePlant=false;
    private void OnMouseDown()
    {
        print("点击了地块");
        //种植植物
         if (GameDate.currentPlantId != -1 && isHavePlant==false)
         {
             GameObject go=GameObject.Instantiate(Resources.Load<GameObject>
                 ("Plant/"+GameDate.currentPlantId));
            //给go设置父节点
            go.transform.parent= transform;

            //给go设置位置
            go.transform.localPosition=Vector3.zero;

            

            //给当前卡片进入cd状态
            Cards[GameDate.currentPlantId].state=CardState.CD;
            
            //扣除当前阳光
            GameDate.sunPoint-=Cards[GameDate.currentPlantId].needSun;

            //控制当前Slot不能够继续种植植物
            isHavePlant = true;

            //给GameDate.currentPlantId重新赋值为-1
            GameDate.currentPlantId = -1;
        }
    }

}
