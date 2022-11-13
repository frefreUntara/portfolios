using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 画面フェードイン、アウト実行クラス
public class Image_Fade_In_Out : MonoBehaviour
{
    enum Mode
    {
        EFADEIN,
        EFADEOUT
    }

    [SerializeField]
    private Image image;

    [SerializeField]
    private float time;

    [SerializeField]
    Mode DOTOWEEN_MODE = Mode.EFADEIN;

    bool isComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        switch (DOTOWEEN_MODE)
        {
            case Mode.EFADEIN:
                FadeIn();
                break;

            case Mode.EFADEOUT:
                FadeOut();
                break;
        }
    }

    /// <summary>
    /// 透明になる
    /// </summary>
    public void FadeOut()
    {
        float alpha = 0;
        Color color = image.color;
        color.a = 1;
        image.color = color;

        image.DOFade(alpha, time).OnComplete(() =>
        {
            isComplete = true;
            Destroy(gameObject);
        });
    }
    /// <summary>
    /// 色が出てくる
    /// </summary>
    public void FadeIn()
    {
        float alpha = 1;
        Color color = image.color;
        color.a = 0;
        image.color = color;

        image.DOFade(alpha, time).OnComplete(() =>
        {
            isComplete = true;
        });
    }

    public bool IsCompleted()
    {
        return isComplete;
    }  
}
