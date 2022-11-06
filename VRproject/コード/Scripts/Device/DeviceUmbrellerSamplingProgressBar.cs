using UnityEngine;
using UnityEngine.UI;
using Umbreller.Minerva;


/// <summary>
///     
/// </summary>
public class DeviceUmbrellerSamplingProgressBar : MonoBehaviour
{
    /// <summary>
    ///     �i���̕����B
    /// </summary>
    [SerializeField] private Text direction;
    /// <summary>
    ///     �i���������O���t�B�J���o�[�B
    /// </summary>
    [SerializeField] private Image progress_bar;
    /// <summary>
    ///     �i���������e�L�X�g�B
    /// </summary>
    [SerializeField] private Text progress_text;

    /// <summary>
    ///     �i���̕����B
    /// </summary>
    public MinervaDirection Direction {
        get; set;
    }
    /// <summary>
    ///     �i���B
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
    ///     �l�̕ω��Ɋɋ}��t���܂��B
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
    ///     �I�u�W�F�N�g���Ĕz�u���܂��B
    /// </summary>
    public void Relocate()
    {
        RectTransform position = GetComponent<RectTransform>();

        position.anchoredPosition = new Vector2(position.sizeDelta.x / 2f, (((int)Direction - (int)Umbreller.Minerva.MinervaDirection.GYROSCOPE) * -30f) - 20f);
    }
}
