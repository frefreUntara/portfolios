using UnityEngine;
using UnityEngine.UI;


/// <summary>
///     �f�o�C�X�̏�Ԃ�\�����܂��B
/// </summary>
public class DeviceStateIndicator : MonoBehaviour
{
    [SerializeField] private Image icon;
    /// <summary>
    ///     Umbreller�������A�C�R���B
    /// </summary>
    [SerializeField] private Sprite iconUmbreller;
    /// <summary>
    ///     Stephany�������A�C�R���B
    /// </summary>
    [SerializeField] private Sprite iconStephany;
    /// <summary>
    ///     ���݂̏�Ԃ�\������e�L�X�g�I�u�W�F�N�g�B
    /// </summary>
    [SerializeField] private Text state;
    /// <summary>
    ///     �C�ӂ̕������\������e�L�X�g�I�u�W�F�N�g�B
    /// </summary>
    [SerializeField] private Text value;
    /// <summary>
    ///     �f�o�C�X�̎�ʁB
    /// </summary>
    [SerializeField] private DeviceType type;

    private DeviceType type_;
    private DeviceState state_;

    /// <summary>
    ///     �f�o�C�X�̎�ʁB
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
    ///     �f�o�C�X�̏�ԁB
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
                    state.text = "���ڑ�";
                    break;

                case DeviceState.CONNECTED:
                    state.text = "�ڑ��ς�";
                    break;

                case DeviceState.CONNECTING:
                    state.text = "�ڑ����s��";
                    break;

                default:
                    break;
            }
        }
    }
    /// <summary>
    ///     �f�o�C�X�̏�Ԃ������C�ӂ̕�����B
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
    ///     �C�ӂ̕�����B
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
