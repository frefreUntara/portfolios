using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]

// リザルト(母親の言葉)の会話内容の管理クラス
public class Talk_EventResult : MonoBehaviour
{
    public string EventName;
    [SerializeField] int GaugeVel = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Girl")
        {
            TalkEvent_Scr talkev_ = other.GetComponent<TalkEvent_Scr>();
            string Scenename = SceneManager.GetActiveScene().name;
            if (Scenename == "Result")
            {
                GaugeVel = GaugeStaticker.Bisyo_Gauge_;
            }
            else
            {
                PlayerGauge WetGuge = other.gameObject.GetComponent<PlayerGauge>();
                GaugeVel = WetGuge.GetGauge();
            }

            Select_EventName(GaugeVel);
            

            /// ここで弾込め式
            /// 発生させたい会話イベントを後ろにスタックさせてる
            talkev_.AddEvent(EventName);
        }
    }
    /// <summary>
    /// びしょ濡れゲージから会話イベントの分岐をする
    /// </summary>
    void Select_EventName(int WetGauge)
    {
        int[] WetGaugeTable = { 10, 30, 50 };
        string[] TalkEventTable = { "Great", "Bad", "Terrible" };
        SetEventFineName("Excellent");

        for (int i = 0; i < WetGaugeTable.Length; ++i)
        {
            if (WetGauge <= WetGaugeTable[i])
            {
                break;
            }

            SetEventFineName(TalkEventTable[i]);
        }
    }

    void SetEventFineName(string SettxtName)
    {
        EventName = "Result/" + SettxtName;
    }
}
