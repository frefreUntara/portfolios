using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]

// ���U���g(��e�̌��t)�̉�b���e�̊Ǘ��N���X
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
            

            /// �����Œe���ߎ�
            /// ��������������b�C�x���g�����ɃX�^�b�N�����Ă�
            talkev_.AddEvent(EventName);
        }
    }
    /// <summary>
    /// �т���G��Q�[�W�����b�C�x���g�̕��������
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
