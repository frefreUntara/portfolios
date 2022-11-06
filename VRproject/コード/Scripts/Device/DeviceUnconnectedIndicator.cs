using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     ���ڑ��f�o�C�X����ʉE��Œʒm���܂��B
/// </summary>
[DefaultExecutionOrder(1)]
public class DeviceUnconnectedIndicator : MonoBehaviour
{
    /// <summary>
    ///     Umbreller�����ڑ��ł��邱�Ƃ������A�C�R���B
    /// </summary>
    [SerializeField] private GameObject uiUmbreller;
    /// <summary>
    ///     Stephany�����ڑ��ł��邱�Ƃ������A�C�R���B
    /// </summary>
    [SerializeField] private GameObject uiStephany;
    
    /// <summary>
    ///     GameObject�N���[�����Ɏg�p����C���X�^���X�B
    /// </summary>
    private static GameObject Canvas {
        get; set;
    }

    /// <summary>
    ///     ���ݕ\�����Ă��関�ڑ��f�o�C�X�̃��X�g�B
    /// </summary>
    private List<DeviceType> ListDisplayed {
        get; set;
    }
    /// <summary>
    ///     ���ݕ\�����Ă���A�C�R���̃��X�g�B
    /// </summary>
    private List<GameObject> listUI {
        get; set;
    }


    private void Start()
    {
        Canvas = GameObject.Find("Canvas");
        ListDisplayed = new List<DeviceType>() { DeviceType.UMBRELLER, DeviceType.STEPHANY };
        listUI = new List<GameObject>();
    }

    private void OnDestroy()
    {
        if (listUI != null)
        {
            foreach (var ui in listUI)
            {
                if (ui != null)
                {
                    Destroy(ui);
                }
            }
            listUI.Clear();
        }
    }


    /// <summary>
    ///     �w�肵���f�o�C�X�𖢐ڑ����X�g�ɓo�^���܂��B
    /// </summary>
    /// <param name="type"></param>
    public void Register(DeviceType type)
    {
        // if (SceneManager.GetActiveScene() == null) {
        //     return;
        // }

        if (ExistTag(type) == false) {
            CreateUI(new List<DeviceType>() { type });
        }
    }

    /// <summary>
    ///     �w�肵���f�o�C�X�𖢐ڑ����X�g����o�^�������܂��B
    /// </summary>
    /// <param name="type"></param>
    public void Unregister(DeviceType type)
    {
        // if (SceneManager.GetActiveScene() == null) {
        //     return;
        // }
        
        if (ExistTag(type) == true) {
            List<DeviceType> list = new List<DeviceType>();

            // �����̏����d�߁B�SUI�폜�{�Ĕz�u�́A�f�o�C�X�̌������Ȃ����炱���ł���r�ƁB
            ListDisplayed.Remove(type);
            foreach (DeviceType type_ in ListDisplayed) {
                list.Add(type_);
            }
            ListDisplayed.Clear();
            OnDestroy();

            CreateUI(list);
        }
    }

    /// <summary>
    ///     �^�O�����݂��Ă��邩�`�F�b�N���܂��B
    /// </summary>
    /// <param name="type"></param>
    private bool ExistTag(DeviceType type) {
        bool result = false;

        foreach (DeviceType type_ in ListDisplayed) {
            if (type_ == type) {
                result = true;
                break;
            }
        }

        return result;
    }

    /// <summary>
    ///     UI�𐶐����܂��B
    /// </summary>
    private void CreateUI(List<DeviceType> list)
    {
        foreach (DeviceType type in list) {
            switch (type) {
                case DeviceType.UMBRELLER:
                    listUI.Add(Instantiate(uiUmbreller, Canvas.transform));
                    break;

                case DeviceType.STEPHANY:
                    listUI.Add(Instantiate(uiStephany, Canvas.transform));
                    break;

                default:
                    break;
            }

            ListDisplayed.Add(type);
        }

        // Relocate.
        for (int i = 0; i < listUI.Count; ++i) {
            if (listUI[i] != null) {
                RectTransform oldPosition = listUI[i].GetComponent<RectTransform>();
                Vector2 newPosition = Vector2.zero;

                newPosition.y = -oldPosition.sizeDelta.y / 2;
                newPosition.x = -oldPosition.sizeDelta.x / 2 - oldPosition.sizeDelta.x * i;
                oldPosition.anchoredPosition = newPosition;
            }
        }
    }
}
