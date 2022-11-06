using System.Collections.Generic;
using UnityEngine;
using Umbreller.Minerva;


public class DeviceUmbrellerSamplingProgressUpdater : MonoBehaviour
{
    [SerializeField] private GameObject pref;

    /// <summary>
    ///     プログレスバーのリスト。
    /// </summary>
    private List<DeviceUmbrellerSamplingProgressBar> ListProgressBar {
        get; set;
    }


    void Start()
    {
        ListProgressBar = new List<DeviceUmbrellerSamplingProgressBar>();

        // Create UI.
        for (int i = 0; i < 7; ++i) {
            DeviceUmbrellerSamplingProgressBar progressBar = Instantiate(pref, GameObject.Find("Canvas").transform).GetComponent<DeviceUmbrellerSamplingProgressBar>();

            progressBar.Direction = MinervaDirection.GYROSCOPE + i;
            progressBar.Progress = 0f;
            progressBar.Relocate();

            ListProgressBar.Add(progressBar);
        }
    }

    private void Update()
    {
        foreach (DeviceUmbrellerSamplingProgressBar progressBar in ListProgressBar) {
            progressBar.Progress = DeviceUmbreller.GetSamplingProgress(progressBar.Direction);
        }
    }
}
