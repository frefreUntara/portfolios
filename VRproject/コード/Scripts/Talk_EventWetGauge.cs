using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 濡れゲージの進行度に合わせた会話イベント管理
struct Gauge_Levels
{
    PlayerGauge wetGauge;
    TalkEvent_Scr talkevent;
    string chara_Name;
    int now_level;
    int next_needwet;
    int[] wetLevel_table;

    // 参照する濡れゲージをセット
    public void Set_PlayerGauge(PlayerGauge setGauge)
    {
        wetGauge = setGauge;
    }

    /// <summary>
    /// 最初のセットアップ
    /// </summary>
    /// <param eventname="所属するキャラ"></param>
    /// <param talkevent_player="所属するキャラが持つ会話イベント管理クラス"></param>
    public void SetUp(string eventname,TalkEvent_Scr talkevent_player)
    {
        chara_Name = eventname;
        talkevent = talkevent_player;
        wetLevel_table = new int[] { 10, 30, 50, 99999 };
        Reset_Level();
    }

    // 
    void Reset_Level()
    {
        now_level = 0;
        next_needwet = wetLevel_table[now_level];
    }

    // 主人公、少女ごとに濡れゲージが一定値まで上昇したときの会話イベント発火
    public void EventAppaerChecker()
    {
        if (wetGauge.GetGauge() >= next_needwet)
        {
            string EventFileName;
            if (chara_Name == "Player")
            {
                EventFileName = "WetGauge_Player";
            }
            else
            {
                EventFileName = "WetGauge";
            }

            EventFileName += "/?Q?[?W" + next_needwet.ToString();
            talkevent.AddEvent(EventFileName);
            now_level++;
      
            next_needwet = wetLevel_table[now_level];
        }
    }

}

[DefaultExecutionOrder(50)]
public class Talk_EventWetGauge : MonoBehaviour
{
    TalkEvent_Scr talkEvent;
    [SerializeField] Gauge_Levels[] wetgauge_talkeventer;
    [SerializeField] PlayerGauge watch_console;
    // Start is called before the first frame update
    void Start()
    {
        talkEvent = GetComponent<TalkEvent_Scr>();
        wetgauge_talkeventer = new Gauge_Levels[2];
        watch_console = GetComponent<PlayerGauge>();
        Gauge_Levels girl = new Gauge_Levels();
        girl.Set_PlayerGauge(GetComponent<PlayerGauge>());
        girl.SetUp(talkEvent.CharaName,talkEvent);

        Gauge_Levels player = new Gauge_Levels();
        player.Set_PlayerGauge(GameObject.Find("Player").GetComponent<PlayerGauge>());
        player.SetUp("Player", talkEvent);

        wetgauge_talkeventer[0] = girl;
        wetgauge_talkeventer[1] = player;
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < wetgauge_talkeventer.Length; ++i)
        {
            wetgauge_talkeventer[i].EventAppaerChecker();
        }
       
    }
}
