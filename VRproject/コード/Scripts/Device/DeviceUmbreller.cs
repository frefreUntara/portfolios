using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using COMPortDevice;
using Umbreller;
using Umbreller.Utility;
using Umbreller.Minerva;


public enum OpenCondition
{
    CLOSE,
    OPEN_HALF,
    OPEN_FULL
}

[DefaultExecutionOrder(2)]
public class DeviceUmbreller : MonoBehaviour
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
    ///     現在の状態。（表示用）
    /// </summary>
    [SerializeField] private DeviceState state;
    /// <summary>
    ///     
    /// </summary>
    [SerializeField] private MinervaDirection direction_css = MinervaDirection.NONE;
    [SerializeField] private UnityEngine.Quaternion quaternion;
    [SerializeField] private float position;
    [SerializeField] private OpenCondition condition;

    private static float position_ = 0f;

    /// <summary>
    ///     最大接続試行回数。
    /// </summary>
    private const uint LIMIT_RETRY = 5;
    /// <summary>
    ///     デバイスID。
    /// </summary>
    private const uint DEVICE_ID = 0x7C0A82E3;
    /// <summary>
    ///     開閉度の最大値。
    /// </summary>
    private static int LIMIT_POSITION = 0;
    /// <summary>
    ///     CSSの自動サンプリングモードでサンプリングする方向の数。
    /// </summary>
    private const uint CSS_AUTO_MODE_SAMPLING_POINT = 6;
    /// <summary>
    ///     デバイス。
    /// </summary>
    private static Umbreller.Umbreller Device {
        get; set;
    } = new Umbreller.Umbreller();
    /// <summary>
    ///     デバイスの種別。
    /// </summary>
    private static DeviceType Type {
        get; set;
    } = DeviceType.UMBRELLER;
    /// <summary>
    ///     Madgwickフィルタのパラメータ。
    /// </summary>
    private static MadgwickParameter Madwick_parameter {
        get; set;
    } = new MadgwickParameter(5f);
    /// <summary>
    ///     Mahonyフィルタのパラメータ。
    /// </summary>
    private static MahonyParameter Mahony_parameter {
        get; set;
    } = new MahonyParameter(80f, 0f);
    /// <summary>
    ///     Minerva（キャリブレーションサポートシステム = CSS）のインスタンス。
    /// </summary>
    private static Minerva CSS {
        get; set;
    } = new Minerva();
    private static int CSSAutoModeSampledPoint {
        get; set;
    }

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
    ///     キャリブレーション済みかを示す。
    /// </summary>
    public static bool IsInitialized {
        get; set;
    } = false;
    /// <summary>
    ///     推定オフセット（バイアス）。
    /// </summary>
    public static DoF9Bias Bias {
        get; set;
    } = new DoF9Bias();
    /// <summary>
    ///     推定姿勢四元数。
    /// </summary>
    public static Umbreller.Quaternion Quaternion {
        get; set;
    } = new Umbreller.Quaternion();
    /// <summary>
    ///     開閉度のデータ。
    /// </summary>
    public static float Position {
        get {
            return position_;
        }
        private set {
            if (value < LIMIT_POSITION) {
                position_ = value / LIMIT_POSITION;
            }
            else {
                position_ = 1f;
            }

            if (position_ >= 0.95f) {
                Condition = OpenCondition.OPEN_FULL;
            }
            else if (position_ >= 0.5f) {
                Condition = OpenCondition.OPEN_HALF;
            }
            else {
                Condition = OpenCondition.CLOSE;
            }
        }
    }
    /// <summary>
    ///     開閉状態。
    /// </summary>
    public static OpenCondition Condition {
        get; private set;
    }

    /// <summary>
    ///     プレイヤー。
    /// </summary>
    private GameObject Player {
        get; set;
    }
    /// <summary>
    ///     衝突判定用。
    /// </summary>
    private UmbrellaCollisionManager CollisionManager {
        get; set;
    }
    /// <summary>
    ///     開閉アニメーション。
    /// </summary>
    private anim AnimationOpen {
        get; set;
    }



    private async void Start()
    {
        state = State;
        position = Position;
        DUI = GameObject.Find("DeviceUnconnectedIndicator").GetComponent<DeviceUnconnectedIndicator>();
        Player = GameObject.Find("Player");
        CollisionManager = GetComponent<UmbrellaCollisionManager>();
        AnimationOpen = transform.Find("karikasa_animation3").gameObject.GetComponent<anim>();

        if (State != DeviceState.CONNECTED) {
            await Connect();
        }
    }

    private void Update()
    {
        switch (State) {
            case DeviceState.CONNECTED:
                position = Position;
                condition = Condition;
                CollisionManager.Position = Position;
                AnimationOpen.Time = Position;

                quaternion = new UnityEngine.Quaternion(-Quaternion.X, Quaternion.Z, Quaternion.Y, Quaternion.W);

                gameObject.transform.rotation = Player.transform.rotation * quaternion;
                break;

            default:
                break;
        }
    }

    private void OnDestroy()
    {
        if (CSS == null) {
            return;
        }

        CSS.StopSampling();
    }

    private void OnApplicationQuit()
    {
        Device.Close();
    }


    /// <summary>
    ///     指定した方向のサンプリングの進捗を取得します。
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float GetSamplingProgress(MinervaDirection direction)
    {
        return (float)CSS.GetProgress(direction);
    }

    /// <summary>
    ///     現在の状態を変更します。
    /// </summary>
    /// <param name="state"></param>
    private void ChangeState(DeviceState state)
    {
        if (State == state) {
            return;
        }

        State = state;
        this.state = state;
        if (state == DeviceState.CONNECTED) {
            //DUI.Unregister(Type);
        }
        else {
            //DUI.Register(Type);
            CSS.StopSampling();
            Device.Close();

            Task.Run(async () =>
            {
                while (true) {
                    for (int i = 3; i > 0; --i) {
                        if (logging_level == LoggingLevel.DEBUG) {
                            Debug.Log($"DeviceUmbreller | 再接続まで{i}秒。");
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
    public async Task Connect(bool calibration = false)
    {
        Result result;
        int count = 1;

        // 最大試行数の範囲で接続が確立するまでループ。
        if (logging_level != LoggingLevel.DEBUG) {
            Debug.Log($"DeviceUmbreller | デバイスに接続しています。");
        }
        while (Device.IsAvailable == false && count <= LIMIT_RETRY) {
            while (DeviceStephany.IsConnecting == true) {
                await Task.Delay(100);
            }

            IsConnecting = true;
            StateCustom = $"接続試行中({count})";
            if (port == "") {
                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log($"DeviceUmbreller | デバイス[ID: {DEVICE_ID}]に接続しています。（{count}回目）");
                }
                result = await Device.Open(DEVICE_ID, false);
            }
            else {
                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log($"DeviceUmbreller | デバイス[COMPort: {port}]に接続しています。（{count}回目）");
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

        if (Device.IsAvailable) {
            if (IsInitialized == false) {
                ChangeState(DeviceState.CONNECTED);
                Debug.Log($"DeviceUmbreller | 初期化しています。");

                Device.StreamingStopHandler -= StreamingStop;
                Device.StreamingReceiveHandler -= ReceiveData;
                CSS.ProgressUpdateHandler -= ProgressUpdateAuto;
                CSS.ProgressUpdateHandler += ProgressUpdateInitialization;

                if (CSS.IsAvailable == true) {
                    if (logging_level == LoggingLevel.DEBUG) {
                        Debug.Log($"DeviceUmbreller | サンプリングを停止します。");
                    }
                    result = await CSS.StopSampling();
                }
                if (Device.IsAvailable == true) {
                    if (logging_level == LoggingLevel.DEBUG) {
                        Debug.Log($"DeviceUmbreller | ストリーミングを停止します。");
                    }
                    result = await Device.StopStreaming();
                }
                Debug.Log($"DeviceUmbreller | サンプリングを開始します。");
                StateCustom = "キャリブレーション中";
                result = await CSS.StartSampling(Device, DoFDirection.Y_N, DoFDirection.X_N, 200, false, false);
                direction_css = MinervaDirection.GYROSCOPE;
                CSS.ChangeSamplingState(direction_css);
                await Device.WaitStop(CancellationToken.None);

                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log($"DeviceUmbreller | 推定オフセットを算出します。");
                }
                {
                    MinervaAnalysis analysis = CSS.GetAnalysis();

                    if (analysis != null) {
                        if (logging_level == LoggingLevel.DEBUG) {
                            Debug.Log("DeviceUmbreller | 推定オフセットの算出に成功しました。");
                        }

                        IsInitialized = true;
                        Bias = analysis.Bias;

                        // 以下のパラメータはキャリブレーションの成否判定に使える。しかしながら、検証してみた結果Errorの値は推論を翻す結果となったため恐らく数式が誤っている。2021/09/26 4:24時点では使用を推奨しない。
                        // analysis.Estimation.Error;  // 推定値を仮説関数(球の方程式)に代入した際の誤差の平均。誤差が0に近いほど仮説関数とフィットしていると言える。
                        // analysis.Estimation.StrengthAverage;  磁界強度(磁束密度)の平均値。(ただしRawデータ)
                        // analysis.Estimation.StrengthStandardDeviation;  磁界強度の標準偏差。標準偏差が小さいほどより球に近い磁界であると言える。標準偏差が大きい場合には付近に磁性体(透磁率が高い物体)があるか、それに準じる電磁ノイズを発生させている機器があると考えられる。
                    }
                    else {
                        if (logging_level == LoggingLevel.DEBUG) {
                            Debug.Log("DeviceUmbreller | 推定オフセットの算出に失敗しました。");
                        }
                    }
                }

                CSS.ProgressUpdateHandler -= ProgressUpdateInitialization;
                Device.StreamingStopHandler += StreamingStop;
                Device.StreamingReceiveHandler += ReceiveData;
                CSS.ProgressUpdateHandler += ProgressUpdateAuto;

                // CSS.StartSamplingIntegrated(DoFDirection.Y_N, DoFDirection.X_N, 100, false, false);
                CSS.ChangeSamplingState(MinervaDirection.MAG_UP);
                CSS.ChangeSamplingState(MinervaDirection.MAG_DOWN);
                CSS.ChangeSamplingState(MinervaDirection.MAG_LEFT);
                CSS.ChangeSamplingState(MinervaDirection.MAG_RIGHT);
                CSS.ChangeSamplingState(MinervaDirection.MAG_FRONT);
                CSS.ChangeSamplingState(MinervaDirection.MAG_BACK);

                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log($"DeviceUmbreller | ストリーミングを開始します。");
                }
                await Device.StartStreaming();
            }
            else if (Device.IsStreaming == false) {
                Device.StreamingStopHandler -= StreamingStop;
                Device.StreamingReceiveHandler -= ReceiveData;

                Device.StreamingStopHandler += StreamingStop;
                Device.StreamingReceiveHandler += ReceiveData;

                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log($"DeviceUmbreller | ストリーミングを開始します。");
                }
                await Device.StartStreaming();
            }
        }

        if (Device.IsStreaming == true) {
            Debug.Log($"DeviceUmbreller | 接続に成功しました。");
            LIMIT_POSITION = Device.Context.ResolutionSwooper;
            ChangeState(DeviceState.CONNECTED);
        }
        else {
            // ストリーミングが開始していない場合は接続が確立していても未接続と見做す。
            Debug.Log($"DeviceUmbreller | 接続に失敗しました。");
            ChangeState(DeviceState.DISCONNECTED);
        }
        StateCustom = "";

        return;
    }

    /// <summary>
    ///     Umbreller.StreamingStopHandlerに登録するメソッド。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StreamingStop(object sender, StreamingStopEventArgs e)
    {
        Debug.Log($"DeviceUmbreller | 接続が切断されました。");
        ChangeState(DeviceState.DISCONNECTED);
    }

    /// <summary>
    ///     Umbreller.StreamingReceiveHandlerに登録するメソッド。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReceiveData(object sender, StreamingReceiveEventArgs e)
    {
        if (e.Data.CheckType(typeof(DeviceDataRaw)) == false) {
            return;
        }

        DeviceDataRaw data = e.Data as DeviceDataRaw;

        CSS.SamplingIntegrated(sender, e);
        if (CSSAutoModeSampledPoint == CSS_AUTO_MODE_SAMPLING_POINT) {
            DoF9Bias biasPrevious = Bias;
            MinervaAnalysis analysis = CSS.GetAnalysis();
            Result result;

            if (analysis != null) {
                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log("DeviceUmbreller | 推定オフセットの算出に成功しました。");
                }

                Bias = analysis.Bias;
                Bias.GX = biasPrevious.GX;
                Bias.GY = biasPrevious.GY;
                Bias.GZ = biasPrevious.GZ;
            }
            else {
                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log("DeviceUmbreller | 推定オフセットの算出に失敗しました。");
                }
            }

            result = CSS.ClearSamples(MinervaDirection.ALL);
            if (result != Result.SUCCESS) {
                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log("DeviceUmbreller | バッファのクリアに失敗しました。");
                }
                throw new InvalidOperationException();
            }

            CSSAutoModeSampledPoint = 0;

            Task.Run(async () =>
            {
                await Task.Delay(20000);

                CSS.ChangeSamplingState(MinervaDirection.MAG_UP);
                CSS.ChangeSamplingState(MinervaDirection.MAG_DOWN);
                CSS.ChangeSamplingState(MinervaDirection.MAG_LEFT);
                CSS.ChangeSamplingState(MinervaDirection.MAG_RIGHT);
                CSS.ChangeSamplingState(MinervaDirection.MAG_FRONT);
                CSS.ChangeSamplingState(MinervaDirection.MAG_BACK);
            });
        }

        // ジャイロスコープのドリフト抑制。
        {
            int threshold = 2;

            if (data.GX > -threshold && data.GX < threshold) data.GX = 0;
            if (data.GY > -threshold && data.GY < threshold) data.GY = 0;
            if (data.GZ > -threshold && data.GZ < threshold) data.GZ = 0;
        }

        // DeviceDataRaw.RotateNEU()を呼び出すと内部のDoF9BIasインスタンスも回転するためクローンする。
        data.Bias = Bias.Clone();
        // 座標系変換。
        data.RotateNEU(DoFDirection.Y_N, DoFDirection.X_N);

        // デバイスデータより現在の姿勢を推定する。
        Quaternion = CAE.MahonyFilterAHRS(Quaternion, data, 3e-3f, Mahony_parameter);
        Position = data.Pos;

        // 四元数の端数を切り捨てることでブレを抑制する。値を小さくするほどブレは抑えられるが滑らかさが著しく低減する。
        {
            int qDiscard = 10000;
            float qDiscardInv = 1.0f / qDiscard;  // 除算は演算負荷が大きいので一回だけ演算する。
            Quaternion.X = (int)(Quaternion.X * qDiscard) * qDiscardInv;
            Quaternion.Y = (int)(Quaternion.Y * qDiscard) * qDiscardInv;
            Quaternion.Z = (int)(Quaternion.Z * qDiscard) * qDiscardInv;
            Quaternion.W = (int)(Quaternion.W * qDiscard) * qDiscardInv;
        }

        e.Delay = 1;
        e.Discard = true;
    }

    /// <summary>
    ///     CSS.ProgressUpdateHandlerに登録するメソッド。（初回用）
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProgressUpdateInitialization(object sender, MinervaUpdateEventArgs e)
    {
        int percentage = (int)(e.Progress * 100);

        if (percentage < 100) {
            return;
        }
        switch (e.Direction) {
            case MinervaDirection.GYROSCOPE:
                direction_css = MinervaDirection.MAG_UP;
                CSS.ChangeSamplingState(direction_css);
                break;

            case MinervaDirection.MAG_UP:
                direction_css = MinervaDirection.MAG_DOWN;
                CSS.ChangeSamplingState(direction_css);
                break;

            case MinervaDirection.MAG_DOWN:
                direction_css = MinervaDirection.MAG_FRONT;
                CSS.ChangeSamplingState(direction_css);
                break;

            case MinervaDirection.MAG_FRONT:
                direction_css = MinervaDirection.MAG_BACK;
                CSS.ChangeSamplingState(direction_css);
                break;

            case MinervaDirection.MAG_BACK:
                direction_css = MinervaDirection.MAG_LEFT;
                CSS.ChangeSamplingState(direction_css);
                break;

            case MinervaDirection.MAG_LEFT:
                direction_css = MinervaDirection.MAG_RIGHT;
                CSS.ChangeSamplingState(direction_css);
                break;

            case MinervaDirection.MAG_RIGHT:
                if (logging_level == LoggingLevel.DEBUG) {
                    Debug.Log($"DeviceUmbreller | ストリーミングを停止します。");
                }
                direction_css = MinervaDirection.NONE;
                e.Cancel = true;
                break;
        }

        e.Delay = 60;
    }

    /// <summary>
    ///     CSS.ProgressUpdateHandlerに登録するメソッド。（自動サンプリング用）
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProgressUpdateAuto(object sender, MinervaUpdateEventArgs e)
    {
        int percentage = (int)(e.Progress * 100);

        if (percentage < 100) {
            return;
        }
        direction_css = e.Direction;
        switch (e.Direction) {
            case MinervaDirection.MAG_UP:
            case MinervaDirection.MAG_DOWN:
            case MinervaDirection.MAG_FRONT:
            case MinervaDirection.MAG_BACK:
            case MinervaDirection.MAG_LEFT:
            case MinervaDirection.MAG_RIGHT:
                ++CSSAutoModeSampledPoint;
                break;

            default:
                break;
        }
    }
}