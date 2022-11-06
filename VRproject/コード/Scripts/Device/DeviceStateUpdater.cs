using UnityEngine;


/// <summary>
///     �f�o�C�X�̏�Ԃ�\������C���W�P�[�^�̏����X�V���܂��B
/// </summary>
public class DeviceStateUpdater : MonoBehaviour
{
    [SerializeField] GameObject pref;

    /// <summary>
    ///     Umbreller�̏�Ԃ�\������C���W�P�[�^�B
    /// </summary>
    private DeviceStateIndicator DSIUmbreller {
        get; set;
    }
    /// <summary>
    ///     Stephany�̏�Ԃ�\������C���W�P�[�^�B
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
