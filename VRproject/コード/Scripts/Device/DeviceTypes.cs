/// <summary>
///     デバイスの種別。
/// </summary>
public enum DeviceType
{
    /// <summary>
    ///     未設定。
    /// </summary>
    NONE,
    /// <summary>
    ///     Umbreller。
    /// </summary>
    UMBRELLER,
    /// <summary>
    ///     Stephany。
    /// </summary>
    STEPHANY
}

/// <summary>
///     デバイスの接続状態。
/// </summary>
public enum DeviceState
{
    /// <summary>
    ///     未設定。
    /// </summary>
    NONE,
    /// <summary>
    ///     未初期化。
    /// </summary>
    UNINITIALIZED,
    /// <summary>
    ///     切断済み。
    /// </summary>
    DISCONNECTED,
    /// <summary>
    ///     接続済み。
    /// </summary>
    CONNECTED,
    /// <summary>
    ///     接続試行中。
    /// </summary>
    CONNECTING
}

public enum LoggingLevel
{
    CRITICAL,
    DEBUG
}