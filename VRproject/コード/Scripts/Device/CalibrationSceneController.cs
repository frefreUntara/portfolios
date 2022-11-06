using UnityEngine;


/// <summary>
///     �L�����u���[�V�����V�[���𐧌䂵�܂��B
/// </summary>
public class CalibrationSceneController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private GameObject SceneManagerNext {
        get; set;
    }
    /// <summary>
    ///     �f�o�C�X�B
    /// </summary>
    private DeviceUmbreller Device {
        get; set;
    }


    private async void Update()
    {
        Device ??= GameObject.Find("Umbrella").GetComponent<DeviceUmbreller>();

        if (Input.GetKeyDown(KeyCode.R) == true && DeviceUmbreller.IsInitialized == true) {
            DeviceUmbreller.IsInitialized = false;
            await Device.Connect();
        }
        else if (Input.GetKeyDown(KeyCode.Return) == true) {
            SceneManagerNext = new GameObject();
            SceneManagerNext.AddComponent<SceneChange>().SceneChangeFunc("Title");
        }
    }
}
