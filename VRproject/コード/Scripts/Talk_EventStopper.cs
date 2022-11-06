using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 会話イベントが終わるまで移動を制限するクラス
public class Talk_EventStopper : MonoBehaviour
{
    TalkEvent_Scr talkEv;

    [SerializeField]
    Talk_EventArea talkArea;

    [SerializeField]
    NaviMoveVer3 navMove;
    bool isEntry = false;
    bool isPlayerg_AreaText = false;
    // Start is called before the first frame update
    void Start()
    {
        navMove ??= GameObject.Find("Player").GetComponent<NaviMoveVer3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (talkEv == null) { return; }
        if (isEntry == false) { return; }
        isPlayerg_AreaText = talkEv.Playing_Event == talkArea.EventName;
        
        navMove.SetMoveFlag(false);

        if (isPlayerg_AreaText)
        {
            if (talkEv.Is_TextEnd())
            {
                navMove.SetMoveFlag(true);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject[] girlTagObjects = GameObject.FindGameObjectsWithTag("Girl");
            if (girlTagObjects != null && TryGetTalkEvent(girlTagObjects))
            {
                navMove.SetMoveFlag(false);
                isEntry = true;
            }
        }

        bool TryGetTalkEvent(GameObject[] girlTagObjects)
        {
            for (int i = 0; i < girlTagObjects.Length; i++)
            {
                if (girlTagObjects[i].TryGetComponent<TalkEvent_Scr>(out talkEv))
                {
                    return true;
                }
            }
            return false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isEntry = false;
        }
    }
}
