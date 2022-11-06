using UnityEngine;
using UnityEngine.UI;


/// <summary>
///     DeviceUnconnectedIconIndicatorで扱うアイコンを点滅させます。
/// </summary>
public class UIBlinker : MonoBehaviour
{
    /// <summary>
    ///     適用対象。
    /// </summary>
    private Image Target {
        get; set;
    }
    /// <summary>
    ///     表示色。
    /// </summary>
    private Color Color;
    /// <summary>
    ///     経過時間。
    /// </summary>
    private float Count {
        get; set;
    }


    private void Start()
    {
        Color = Color.red;
        Target = GetComponent<Image>();
        Count = 0;

        //Vector3 createPos = Vector3.zero;
        //createPos.x = -Image.rectTransform.sizeDelta.x / 2;
        //createPos.y = -Image.rectTransform.sizeDelta.y / 2;
        //Image.rectTransform.anchoredPosition = createPos;
    }

    // Update is called once per frame
    private void Update()
    {
        Count += Time.deltaTime;
        Color.a = 0.3f + Mathf.Sin(2 * Count);
        Target.color = Color;
    }

    private void OnApplicationQuit()
    {
        Destroy(this);
    }
}
