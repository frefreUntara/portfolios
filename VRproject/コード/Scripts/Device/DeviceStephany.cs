using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using COMPortDevice;


/// <summary>
///     Stephanyを管理します。
/// </summary>
[DefaultExecutionOrder(3)]
public class DeviceStephany : MonoBehaviour
{
    /// <summary>
    ///     ロギングレベル。
    /// </summary>
    [SerializeField] private LoggingLevel logging_level = LoggingLevel.CRITICAL;
    /// <summary>
    ///     COMポート名。
    /// </summary>
    [SerializeField] private string port = "";
    /// <summary>
    ///     キーボード操作時の移動ステップ。
    /// </summary>
    [SerializeField] private float step = 0.1f;
    /// <summary>
    ///     現在の状態。（表示用）
    /// </summary>
    [SerializeField] private DeviceState state;
    /// <summary>
    ///     左足のデータ。（表示用）
    /// </summary>
    [SerializeField] private float footL = 0f;
    /// <summary>
    ///     右足のデータ。（表示用）
    /// </summary>
    [SerializeField] private float footR = 0f;


    /// <summary>
    ///     最大接続試行回数。
    /// </summary>
    private const uint LIMIT_RETRY = 5;
    /// <summary>
    ///     デバイスID。
    /// </summary>
    private const uint DEVICE_ID = 0x8E0F7834;
    /// <summary>
    ///     デバイス。
    /// </summary>
    private static Stephany.Stephany Device {
        get; set;
    } = new Stephany.Stephany();
    /// <summary>
    ///     デバイスの種別。
    /// </summary>
    private static DeviceType Type {
        get; set;
    } = DeviceType.STEPHANY;

    /// <summary>
    ///     未接続状態通知用。
    /// </summary>
    private DeviceUnconnectedIndicator DUI {
        get; set;
    }

    /// <summary>
    ///     現在の状態。
    /// </summary>
    public static DeviceState State {
        get; private set;
    } = DeviceState.UNINITIALIZED;
    /// <summary>
    ///     現在の状態を示すカスタムな文字列。<br/>
    ///     カスタムな文字列が指定されていない場合はStateが現在の状態を示します。
    /// </summary>
    public static string StateCustom {
        get; private set;
    } = "";
    /// <summary>
    ///     接続試行中かを示す。
    /// </summary>
    public static bool IsConnecting {
        get; private set;
    } = false;
    /// <summary>
    ///     左足のデータ。
    /// </summary>
    public static float FootL {
        get; private set;
    } = 0f;
    /// <summary>
    ///     右足のデータ。
    /// </summary>
    public static float FootR {
        get; private set;
    } = 0f;


    private async void Start()
    {
        state = State;
        footL = FootL;
        footR = FootR;
        DUI = GameObject.Find("DeviceUnconnectedIndicator").GetComponent<DeviceUnconnectedIndicator>();

        if (State != DeviceState.CONNECTED) {
            await Connect();
        }
    }

    void Update()
    {
        switch (State) {
            case DeviceState.CONNECTED:
                Stephany.DeviceData data = Device.Data as Stephany.DeviceData;

                if (data != null) {
                    FootR = data.DEGR;
                    FootL = data.DEGL;
                }
                else {
                    FootR = 0f;
                    FootL = 0f;
                }
                break;

            default:
                if (Input.GetKey(KeyCode.I)) {
                    FootR += step;
                    if (FootR > 1f) {
                        FootR = 1f;
                    }
                }
                else {
                    FootR -= step;
                    if (FootR < 0f) {
                        FootR = 0f;
                    }
                }

                if (Input.GetKey(KeyCode.W)) {
                    FootL += step;
                    if (FootL > 1f) {
                        FootL = 1f;
                    }
                }
                else {
                    FootL -= step;
                    if (FootL < 0f) {
                        FootL = 0f;
                    }
                }
                break;
        }

        footL = FootL;
        footR = FootR;
    }

    private void OnApplicationQuit()
    {
        if (Device != null) {
            Device.Close();
        }
    }


    /// <summary>
    ///     現在の状態を変更します。
    /// </summary>
    /// <param name="state"></param>
    private void ChangeState(DeviceState state)
    {
        if (state == State) {
            return;
        }

        State = state;
        this.state = state;
        if (state == DeviceState.CONNECTED) {
            //DUI.Unregister(Type);
        }
        else {
            //DUI.Register(Type);
            Device.Close();

            Task.Run(async () =>
            {
                while (true) {
                    for (int i = 3; i > 0; --i) {
                        if (logging_level == LoggingLevel.DEBUG) {
                            Debug.Log($"DeviceStephany | 再接続まで{i}秒。");
                        }
                        await Task.Delay(1000);
                    }

                    await Connect();

                    if (State == DeviceState.CONNECTED) {
                        break;
                    }
                }
            });
        }
    }

    /// <summary>
    ///     デバイスに接続します。
    /// </summary>
    /// <returns></returns>
    private async Task Connect()
    {
        Result result;
        int count = 1;

        // 最大試行数の範囲で接続が確立するまでループ。
        if (logging_level != LoggingLevel.DEBUG) {
            Debug.Log($"DeviceStephany | デバイスに接続しています。");
        }
        while (Device.IsAvailable == false && count <= LIMIT_RETRY) {
            while (DeviceUmbreller.IsConnecting == true) {
                await Task.Delay(100);
            }

            IsConnecting = true;
            StateCustom = $"接続試行中({count})";
            if (port == "") {
                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log($"DeviceStephany | デバイス[ID: {DEVICE_ID}]に接続しています。（{count}回目）");
                }
                result = await Device.Open(DEVICE_ID, false);
            }
            else {
                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log($"DeviceStephany | デバイス[COMPort: {port}]に接続しています。（{count}回目）");
                }
                result = await Device.Open(port, DEVICE_ID, false);
            }
            IsConnecting = false;
            await Task.Delay(500);
            if (result == Result.SUCCESS) {
                break;
            }
            ++count;
        }

        if (Device.IsAvailable == true) {
            Device.StreamingStopHandler -= StopStreaming;
            Device.StreamingReceiveHandler -= ReceiveData;
            Device.StreamingStopHandler += StopStreaming;
            Device.StreamingReceiveHandler += ReceiveData;

            if (logging_level == LoggingLevel.DEBUG) {
                Debug.Log($"DeviceStephany | ストリーミングを開始します。");
            }
            await Device.StartStreaming();
        }

        if (Device.IsStreaming == true) {
            Debug.Log($"DeviceStephany | 接続に成功しました。");
            ChangeState(DeviceState.CONNECTED);
        }
        else {
            // ストリーミングが開始していない場合は接続が確立していても未接続と見做す。
            Debug.Log($"DeviceStephany | 接続に失敗しました。");
            ChangeState(DeviceState.DISCONNECTED);
        }
        StateCustom = "";

        return;
    }

    /// <summary>
    ///     Stephany.StreamingReceiveHandlerに登録するメソッド。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StopStreaming(object sender, StreamingStopEventArgs e)
    {
        Debug.Log($"DeviceStephany | 接続が切断されました。");
        ChangeState(DeviceState.DISCONNECTED);
    }

    /// <summary>
    ///     Stephany.StreamingStopHandlerに登録するメソッド。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReceiveData(object sender, StreamingReceiveEventArgs e)
    {
        e.Delay = 1;
        e.Discard = true;
    }
}
