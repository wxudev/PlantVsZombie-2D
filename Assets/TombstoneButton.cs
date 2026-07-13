using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TombstoneButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("动画参数")]
    public float hoverScale = 1.05f;   // 悬停放大比例
    public float duration = 0.2f;      // 动画时长

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    // 鼠标悬停：放大
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * hoverScale, duration);
    }

    // 鼠标离开：缩回原大小
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, duration);
    }

    // 点击：回弹一下再跳转
    public void OnPointerClick(PointerEventData eventData)
    {
        // 先缩小再回弹，做按压感
        transform.DOScale(originalScale * 0.95f, 0.1f).OnComplete(() =>
        {
            transform.DOScale(originalScale, 0.1f);
        });
    }
}
