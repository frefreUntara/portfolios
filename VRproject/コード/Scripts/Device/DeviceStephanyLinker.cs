using UnityEngine;


/// <summary>
/// �����݃f�o�C�X�̏���Unity�ň�����悤�ɂ��郊���J�[�B
/// </summary>
[RequireComponent(typeof(DeviceStephany))]
public class DeviceStephanyLinker : MonoBehaviour
{
    // private const float SCALE = 1300;

    /// <summary>
    ///     �߂�����ړ������B
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
