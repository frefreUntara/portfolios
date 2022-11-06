using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Tracking_State
{
    SERCHING,   // 検索中
    FIND,       // 検出
    DEMO,       // デモ(デスクトップ版)
    MISSING     // ロスト
}

// 傘をプレイヤーにトラッキングさせるクラス
public class Tracking_Umbllera_to_Player : MonoBehaviour
{

    [SerializeField] private Tracking_State tracking_State = Tracking_State.SERCHING;
    [SerializeField] private Vector3 Offset_pos;
    private GameObject tracking_target;
    private GameObject Demo_trackingTarget;

    private GameObject player;
    [SerializeField] private float adjustment;
    [SerializeField] private Vector3 adjustmentPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            var find_target = player.transform.Find("OVRCameraRig");

            if (find_target)
            {
                tracking_target = find_target.Find("CenterEyeAnchor").gameObject;
                TrackingState_Change(Tracking_State.FIND);
            }
            else
            {
                Demo_trackingTarget = player;
                TrackingState_Change(Tracking_State.DEMO);
            }
        }
        else
        {
            TrackingState_Change(Tracking_State.MISSING);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (tracking_State)
        {
            case Tracking_State.FIND:
                transform.position = tracking_target.transform.position;
                transform.localPosition += Offset_pos;
                break;
            case Tracking_State.DEMO:
                transform.position = Demo_trackingTarget.transform.position + Offset_pos;
                break;
            default:
                transform.position = Offset_pos;
                break;
        }

        TrackingFront();
    }

    private void TrackingState_Change(Tracking_State change_state)
    {
        if (tracking_State == change_state) { return; }
        tracking_State = change_state;
    }

    private void TrackingFront()
    {
        this.transform.position = player.transform.position + player.transform.forward * adjustment + adjustmentPos;
    }
}
