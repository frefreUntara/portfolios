using UnityEngine;


/// <summary>
///     デバイスの状態を表示するインジケータの情報を更新します。
/// </summary>
public class DeviceStateUpdater : MonoBehaviour
{
    [SerializeField] GameObject pref;

    /// <summary>
    ///     Umbrellerの状態を表示するインジケータ。
    /// </summary>
    private DeviceStateIndicator DSIUmbreller {
        get; set;
    }
    /// <summary>
    ///     Stephanyの状態を表示するインジケータ。
    /// </summary>
    private DeviceStateIndicator DSIStephany {
        get; set;
    }


    private void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");

        DSIUmbreller = Instantiate(pref, canvas.transform).GetComponent<DeviceStateIndicator>();
        DSIUmbreller.Type = DeviceType.UMBRELLER;

        DSIStephany = Instantiate(pref, canvas.transform).GetComponent<DeviceStateIndicator>();
        DSIStephany.Type = DeviceType.STEPHANY;
    }

    private void Update()
    {
        if (DeviceUmbreller.StateCustom == "") {
            DSIUmbreller.State = DeviceUmbreller.State;
        }
        else {
            DSIUmbreller.StateText = DeviceUmbreller.StateCustom;
        }
        DSIUmbreller.Text = DeviceUmbreller.Position.ToString("F2");


        if (DeviceStephany.StateCustom == "") {
            DSIStephany.State = DeviceStephany.State;
        }
        else {
            DSIStephany.StateText = DeviceStephany.StateCustom;

        }
        DSIStephany.Text = $"L:{DeviceStephany.FootL:F1}, R: {DeviceStephany.FootR:F1}";
    }
}
