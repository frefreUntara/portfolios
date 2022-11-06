/// <summary>
///     �f�o�C�X�̎�ʁB
/// </summary>
public enum DeviceType
{
    /// <summary>
    ///     ���ݒ�B
    /// </summary>
    NONE,
    /// <summary>
    ///     Umbreller�B
    /// </summary>
    UMBRELLER,
    /// <summary>
    ///     Stephany�B
    /// </summary>
    STEPHANY
}

/// <summary>
///     �f�o�C�X�̐ڑ���ԁB
/// </summary>
public enum DeviceState
{
    /// <summary>
    ///     ���ݒ�B
    /// </summary>
    NONE,
    /// <summary>
    ///     ���������B
    /// </summary>
    UNINITIALIZED,
    /// <summary>
    ///     �ؒf�ς݁B
    /// </summary>
    DISCONNECTED,
    /// <summary>
    ///     �ڑ��ς݁B
    /// </summary>
    CONNECTED,
    /// <summary>
    ///     �ڑ����s���B
    /// </summary>
    CONNECTING
}

public enum LoggingLevel
{
    CRITICAL,
    DEBUG
}