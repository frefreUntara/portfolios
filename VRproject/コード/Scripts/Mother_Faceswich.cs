using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Face_State
{
    NORMAL,
    ANGRY,
    NUM
}

// 母親キャラの表情オブジェクト管理
public class Mother_Faceswich : MonoBehaviour
{
    [SerializeField] GameObject Normal_face;
    [SerializeField] GameObject Hear_face;
    [SerializeField] GameObject Angry_face;
    [SerializeField] Face_State states = Face_State.NORMAL;

    // Start is called before the first frame update
    void Start()
    {
        // 各種表情がアタッチされていない場合、自動で取ってくる
        if (Normal_face == null)
        {
            Normal_face = transform.Find("Mother_head").gameObject;
        }
        if (Hear_face == null)
        {
            Hear_face = transform.Find("Mother_hair").gameObject;
        }
        if (Angry_face == null)
        {
            Angry_face = transform.Find("hannya").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 表情切り替え
        switch (states)
        {
            case Face_State.NORMAL:
                Normal_face.SetActive(true);
                Hear_face.SetActive(true);
                Angry_face.SetActive(false);
                break;

            case Face_State.ANGRY:
                Normal_face.SetActive(false);
                Hear_face.SetActive(false);
                Angry_face.SetActive(true);
                break;

        }
    }

    public void SetAngry()
    {
        states = Face_State.ANGRY;
    }

    public void Swich_EmotionState()
    {
        states = (Face_State)(((int)states + 1) % (int)Face_State.NUM);
        
    }
}
