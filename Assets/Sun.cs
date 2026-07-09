using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public void OnMouseDown()
    {
        //播放声音
        transform.parent.GetComponent<AudioSource>().Play();
        //点击阳光，阳光会移动到指定位置,增加阳光数并删除阳光
        transform.DOLocalMove(
            new Vector3(-6.39f, 4.22f, 0), 0.5f).OnComplete(EndMove);
           
    }

    private void EndMove()
    {
        //增加阳光数
        GameDate.sunPoint += 25;
        //删除阳光
        Destroy(gameObject);
    }


}
