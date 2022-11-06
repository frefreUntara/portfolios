using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
/// 会話イベント発生エリア管理クラス
/// 制作者：日本電子専門学校　ゲーム制作科２年　20CI0205 伊東佳汰　
public class Talk_EventArea : MonoBehaviour
{
    // 実行したい会話イベント
    public string EventName = null;
    // 感知したいターゲットタグ
    public string TargetTag = "Girl";
    // 検知範囲
    public Vector3 PerceptionAreaScale = new Vector3(10, 1, 3);
    // 検知したか判定
    [SerializeField] private bool isPassing = false;
    // こちらを向いたか判定
    public bool isLoockMe
    {
        get;
        set;
    }
   

    // 会話イベントスクリプト
    private TalkEvent_Scr talkev_;
    // 
    private float TimeCounter;
    // 
    private float NextPsee = 3;

    private NavMeshAgent girlAI;

    [SerializeField] private Talk_Event_LookUI_Manager look_manager;

    [SerializeField]
    private Girl_LookRay mGirl_LookRay;

    [SerializeField]
    private NaviMoveVer3 naviMove;

    // Start is called before the first frame update
    void Start()
    {
        TimeCounter = NextPsee;
        // エリア判定用のコライダーを
        BoxCollider Perception_area = GetComponent<BoxCollider>();
        if (Perception_area == null)
        {
            Perception_area = gameObject.AddComponent<BoxCollider>();
        }

        mGirl_LookRay.enabled = false;
        look_manager.enabled = false;

        Perception_area.isTrigger = true;
        Perception_area.size = PerceptionAreaScale;
    }

    private void Update()
    {
        if (talkev_ == null) { return; }

        if (isLoockMe == false)
        {
            TimeCounter += Time.deltaTime;
            if (TimeCounter >= NextPsee)
            {
                talkev_.AddEvent("ねぇねぇ");
                TimeCounter = 0;
            }
        }
        else
        {
            if (isPassing)
            {
                if (talkev_.Is_TargetEvent_End(EventName))
                {
                    girlAI.speed = 3.5f;
                    talkev_ = null;
                    mGirl_LookRay.enabled = false;
                    naviMove.SetMoveFlag(true);
                    naviMove.speed = 3.5f;
                }
            }
            else
            {
                /// ここで弾込め式
                /// 発生させたい会話イベントを後ろにスタックさせてる
                talkev_.AddEvent(EventName);
                isPassing = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPassing) { return; }

        if (other.gameObject.tag == TargetTag)
        {
            look_manager.Girl = other.gameObject;
            look_manager.Create_LookUI();
            talkev_ = other.GetComponent<TalkEvent_Scr>();
            girlAI = other.GetComponent<NavMeshAgent>();
            girlAI.speed = 0;
            naviMove.SetMoveFlag(false);
            naviMove.speed = 0;
            isLoockMe = false;
            mGirl_LookRay.enabled = true;
            look_manager.enabled = true;
        }
    }
    /// <summary>
    /// エリア内に対象が入ったかのフラグ取得
    /// </summary>
    /// <returns></returns>
    public bool IsPassing() => isPassing;
}
