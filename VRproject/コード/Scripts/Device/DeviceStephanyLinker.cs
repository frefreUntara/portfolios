using UnityEngine;


/// <summary>
/// 足踏みデバイスの情報をUnityで扱えるようにするリンカー。
/// </summary>
[RequireComponent(typeof(DeviceStephany))]
public class DeviceStephanyLinker : MonoBehaviour
{
    // private const float SCALE = 1300;

    /// <summary>
    ///     めっちゃ移動するやつ。
    /// </summary>
    [SerializeField] private NaviMoveVer3 navi;

    private void Start()
    {
        
    }

    private void Update()
    {
        navi.GetDeviceValue(DeviceStephany.FootL, DeviceStephany.FootR);
    }
}
