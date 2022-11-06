using System;
using UnityEngine;


/// <summary>
///     接続試行までのカウントを管理します。
/// </summary>
public class DeviceConnectionManager
{
    private float threshold;

    /// <summary>
    ///     カウント閾値。
    /// </summary>
    public float Limit {
        get {
            return threshold;
        }
        set {
            if (value < 1) {
                throw new ArgumentOutOfRangeException();
            }

            threshold = value;
            Current = 0;
            IsCounting = false;
            IsTimeout = false;
        }
    }
    /// <summary>
    ///     現在のカウント。
    /// </summary>
    public float Current {
        get; private set;
    }
    /// <summary>
    ///     現在カウント中かを示します。
    /// </summary>
    public bool IsCounting {
        get; private set;
    }
    /// <summary>
    ///     カウントが終了したかを示します。
    /// </summary>
    public bool IsTimeout {
        get; private set;
    }
    /// <summary>
    ///     ログの前に付加する文字列。
    /// </summary>
    public string LogPrefix {
        get; set;
    }
    /// <summary>
    ///     ログの後ろに付加する文字列。
    /// </summary>
    public string LogSuffix {
        get; set;
    }


    public DeviceConnectionManager()
    {
        Limit = 10;
        LogPrefix = string.Empty;
        LogSuffix = string.Empty;
    }

    public DeviceConnectionManager(int limit)
    {
        Limit = limit;
        LogPrefix = string.Empty;
        LogSuffix = string.Empty;
    }

    public DeviceConnectionManager(string logPrefix, string logSuffix)
    {
        Limit = 10;
        LogPrefix = logPrefix;
        LogSuffix = logSuffix;
    }

    public DeviceConnectionManager(int limit, string logPrefix, string logSuffix)
    {
        Limit = limit;
        LogPrefix = logPrefix;
        LogSuffix = logSuffix;
    }



    /// <summary>
    ///     内部状態を更新します。
    /// </summary>
    public void Update()
    {
        if (IsCounting && IsTimeout == false) {
            Current += Time.deltaTime;
            IsTimeout = (Current >= Limit);
            if (IsTimeout == true) {
                IsCounting = false;
            }

            Debug.Log($"{LogPrefix}再接続まで:{((int)Limit - (int)Current)}{LogSuffix}");
        }
    }
    
    /// <summary>
    ///     カウントをリセットします。
    /// </summary>
    public void Reset()
    {
        Current = 0;
        IsCounting = false;
        IsTimeout = false;
    }

    /// <summary>
    ///     カウントをリセットして開始します。
    /// </summary>
    public void Restart()
    {
        Current = 0;
        IsCounting = true;
        IsTimeout = false;
    }

    /// <summary>
    ///     カウントを開始します。
    /// </summary>
    public void Start()
    {
        IsCounting = true;
    }

    /// <summary>
    ///     カウントを終了します。
    /// </summary>
    public void Stop()
    {
        IsCounting = false;
    }
}
