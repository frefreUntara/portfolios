using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 最初の下駄箱での少女選択UI決定システム
public class UIMove_to_Center : MonoBehaviour
{
    [SerializeField] bool mIsMove;
    Girl_LookRay girl_ray;
    Image mImage;

    float mScale = 5;

    // Start is called before the first frame update
    void Start()
    {
        mImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (girl_ray.IsLook_Girl())
        {
            if(mImage.color== Color.clear)
            {
                mImage.color = Color.white; 
            }

            mScale -= girl_ray.Look_Time();
            if (mScale < 0) { mScale = 0; }
            mImage.rectTransform.localScale = Vector3.one * mScale;
        }
        else
        {
            mImage.color = Color.clear;
        }
    }

    public void Set_GirlRay(Girl_LookRay set_component)
    {
        girl_ray = set_component;
    }
}
