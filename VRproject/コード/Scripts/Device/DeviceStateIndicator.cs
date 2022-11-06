using UnityEngine;
using UnityEngine.UI;


/// <summary>
///     デバイスの状態を表示します。
/// </summary>
public class DeviceStateIndicator : MonoBehaviour
{
    [SerializeField] private Image icon;
    /// <summary>
    ///     Umbrellerを示すアイコン。
    /// </summary>
    [SerializeField] private Sprite iconUmbreller;
    /// <summary>
    ///     Stephanyを示すアイコン。
    /// </summary>
    [SerializeField] private Sprite iconStephany;
    /// <summary>
    ///     現在の状態を表示するテキストオブジェクト。
    /// </summary>
    [SerializeField] private Text state;
    /// <summary>
    ///     任意の文字列を表示するテキストオブジェクト。
    /// </summary>
    [SerializeField] private Text value;
    /// <summary>
    ///     デバイスの種別。
    /// </summary>
    [SerializeField] private DeviceType type;

    private DeviceType type_;
    private DeviceState state_;

    /// <summary>
    ///     デバイスの種別。
    /// </summary>
    public DeviceType Type {
        get {
            return type_;
        }
        set {
            type_ = value;
            type = value;
            switch (value) {
                case DeviceType.UMBRELLER:
                    icon.sprite = iconUmbreller;
                    break;

                case DeviceType.STEPHANY:
                    icon.sprite = iconStephany;
                    break;

                default:
                    break;
            }

            // Relocate.
            switch (value) {
                case DeviceType.UMBRELLER:
                    GetComponent<RectTransform>().anchoredPosition = new Vector2(25, 25);
                    break;

                case DeviceType.STEPHANY:
                    GetComponent<RectTransform>().anchoredPosition = new Vector2(25, 75);
                    break;

                default:
                    break;
            }
        }
    }
    /// <summary>
    ///     デバイスの状態。
    /// </summary>
    public DeviceState State {
        get {
            return state_;
        }
        set {
            state_ = value;
            switch (value) {
                case DeviceState.UNINITIALIZED:
                case DeviceState.DISCONNECTED:
                    state.text = "未接続";
                    break;

                case DeviceState.CONNECTED:
                    state.text = "接続済み";
                    break;

                case DeviceState.CONNECTING:
                    state.text = "接続試行中";
                    break;

                default:
                    break;
            }
        }
    }
    /// <summary>
    ///     デバイスの状態を示す任意の文字列。
    /// </summary>
    public string StateText {
        get {
            return state.text;
        }
        set {
            state.text = value;
        }
    }
    /// <summary>
    ///     任意の文字列。
    /// </summary>
    public string Text {
        get {
            return value.text;
        }
        set {
            this.value.text = value;
        }
    }


    private void Start()
    {
        type_ = DeviceType.NONE;
        state_ = DeviceState.NONE;
    }
}
