using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 会話のテキストボックスのY軸回転補間
public class TextBox_Rotate_Correct : MonoBehaviour
{
    private RectTransform anchor;
    // Start is called before the first frame update
    void Start()
    {
        anchor = GetComponent<RectTransform>();
        anchor.sizeDelta = new Vector2(anchor.sizeDelta.x, 60);
        anchor.localRotation = Quaternion.Euler(0, 0, 0);
        anchor.anchoredPosition = Vector2.up * 25;
    }

    // Update is called once per frame
    void Update()
    {
        if (anchor.localRotation.y != 0)
        {
            anchor.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
