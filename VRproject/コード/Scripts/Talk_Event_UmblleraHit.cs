using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 少女に傘をぶつけた時の会話イベント管理クラス
public class Talk_Event_UmblleraHit : MonoBehaviour
{
    private TalkEvent_Scr talk_Ev;
    private AudioSource audio_player;
    [SerializeField] private int HitCount = 0;
    [SerializeField] private string HitObjectName = "pCylinder1";
    [SerializeField] private string[] EventNames;
    [SerializeField] private AudioClip Hit_SE;


    // Start is called before the first frame update
    void Start()
    {
        HitCount = 0;
        talk_Ev = GetComponent<TalkEvent_Scr>();
        audio_player = GetComponent<AudioSource>();
        EventNames = new string[] { "Hit_One", "Hit_Tow", "Hit_Three"};
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.name == HitObjectName)
        {
            if (HitCount < EventNames.Length - 1)
            {
                HitCount++;
            }
            audio_player.PlayOneShot(Hit_SE);
            talk_Ev.AddEvent("HitEvent/" + EventNames[HitCount]);
        }
    }
}
