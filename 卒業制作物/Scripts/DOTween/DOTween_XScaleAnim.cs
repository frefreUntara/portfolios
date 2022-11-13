using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTween_XScaleAnim : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_rectTransform;

    [SerializeField]
    private float Scale = 1;

    [SerializeField]
    private float TweenTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (m_rectTransform == null) 
        {
            m_rectTransform = GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_rectTransform.DOScaleX(Scale, TweenTime);
    }
}
