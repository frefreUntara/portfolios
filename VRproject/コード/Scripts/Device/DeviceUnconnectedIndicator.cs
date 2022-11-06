using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     未接続デバイスを画面右上で通知します。
/// </summary>
[DefaultExecutionOrder(1)]
public class DeviceUnconnectedIndicator : MonoBehaviour
{
    /// <summary>
    ///     Umbrellerが未接続であることを示すアイコン。
    /// </summary>
    [SerializeField] private GameObject uiUmbreller;
    /// <summary>
    ///     Stephanyが未接続であることを示すアイコン。
    /// </summary>
    [SerializeField] private GameObject uiStephany;
    
    /// <summary>
    ///     GameObjectクローン時に使用するインスタンス。
    /// </summary>
    private static GameObject Canvas {
        get; set;
    }

    /// <summary>
    ///     現在表示している未接続デバイスのリスト。
    /// </summary>
    private List<DeviceType> ListDisplayed {
        get; set;
    }
    /// <summary>
    ///     現在表示しているアイコンのリスト。
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
    ///     指定したデバイスを未接続リストに登録します。
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
    ///     指定したデバイスを未接続リストから登録解除します。
    /// </summary>
    /// <param name="type"></param>
    public void Unregister(DeviceType type)
    {
        // if (SceneManager.GetActiveScene() == null) {
        //     return;
        // }
        
        if (ExistTag(type) == true) {
            List<DeviceType> list = new List<DeviceType>();

            // ここの処理重め。全UI削除＋再配置の、デバイスの個数が少ないからこそできる荒業。
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
    ///     タグが存在しているかチェックします。
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
    ///     UIを生成します。
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
