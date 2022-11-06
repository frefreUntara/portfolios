using System;
using UnityEngine;


/// <summary>
///     �ڑ����s�܂ł̃J�E���g���Ǘ����܂��B
/// </summary>
public class DeviceConnectionManager
{
    private float threshold;

    /// <summary>
    ///     �J�E���g臒l�B
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
    ///     ���݂̃J�E���g�B
    /// </summary>
    public float Current {
        get; private set;
    }
    /// <summary>
    ///     ���݃J�E���g�����������܂��B
    /// </summary>
    public bool IsCounting {
        get; private set;
    }
    /// <summary>
    ///     �J�E���g���I���������������܂��B
    /// </summary>
    public bool IsTimeout {
        get; private set;
    }
    /// <summary>
    ///     ���O�̑O�ɕt�����镶����B
    /// </summary>
    public string LogPrefix {
        get; set;
    }
    /// <summary>
    ///     ���O�̌��ɕt�����镶����B
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
    ///     ������Ԃ��X�V���܂��B
    /// </summary>
    public void Update()
    {
        if (IsCounting && IsTimeout == false) {
            Current += Time.deltaTime;
            IsTimeout = (Current >= Limit);
            if (IsTimeout == true) {
                IsCounting = false;
            }

            Debug.Log($"{LogPrefix}�Đڑ��܂�:{((int)Limit - (int)Current)}{LogSuffix}");
        }
    }
    
    /// <summary>
    ///     �J�E���g�����Z�b�g���܂��B
    /// </summary>
    public void Reset()
    {
        Current = 0;
        IsCounting = false;
        IsTimeout = false;
    }

    /// <summary>
    ///     �J�E���g�����Z�b�g���ĊJ�n���܂��B
    /// </summary>
    public void Restart()
    {
        Current = 0;
        IsCounting = true;
        IsTimeout = false;
    }

    /// <summary>
    ///     �J�E���g���J�n���܂��B
    /// </summary>
    public void Start()
    {
        IsCounting = true;
    }

    /// <summary>
    ///     �J�E���g���I�����܂��B
    /// </summary>
    public void Stop()
    {
        IsCounting = false;
    }
}
