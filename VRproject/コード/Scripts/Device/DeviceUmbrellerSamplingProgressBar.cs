using UnityEngine;
using UnityEngine.UI;
using Umbreller.Minerva;


/// <summary>
///     
/// </summary>
public class DeviceUmbrellerSamplingProgressBar : MonoBehaviour
{
    /// <summary>
    ///     進捗の方向。
    /// </summary>
    [SerializeField] private Text direction;
    /// <summary>
    ///     進捗を示すグラフィカルバー。
    /// </summary>
    [SerializeField] private Image progress_bar;
    /// <summary>
    ///     進捗を示すテキスト。
    /// </summary>
    [SerializeField] private Text progress_text;

    /// <summary>
    ///     進捗の方向。
    /// </summary>
    public MinervaDirection Direction {
        get; set;
    }
    /// <summary>
    ///     進捗。
    /// </summary>
    public float Progress {
        get; set;
    }

    
    private void Start()
    {
        Progress = 0;
    }

    private void Update()
    {
        if (Progress > 1f) {
            Progress = 1f;
        }

        direction.text = Direction.ToString();
        progress_bar.fillAmount = Easing(Progress);
        progress_text.text = ((int)(Progress * 100f)).ToString() + "%";
    }


    /// <summary>
    ///     値の変化に緩急を付けます。
    /// </summary>
    /// <param name="progress"></param>
    /// <returns></returns>
    private static float Easing(float progress)
    {
        float result = 0f;

        if (progress < 0.5f) {
            result = 4f * progress * progress * progress;
        }
        else {
            result = 1f - Mathf.Pow(-2f * progress + 2f, 3f) / 2f;
        }

        return result;
    }

    /// <summary>
    ///     オブジェクトを再配置します。
    /// </summary>
    public void Relocate()
    {
        RectTransform position = GetComponent<RectTransform>();

        position.anchoredPosition = new Vector2(position.sizeDelta.x / 2f, (((int)Direction - (int)Umbreller.Minerva.MinervaDirection.GYROSCOPE) * -30f) - 20f);
    }
}
