using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    public int samplerate = 10000;
    public int resolutionX = 2048;
    public int resolutionY = 1536;
    // App time scale set to 0.1, meaning 10s app time passes as 1s realtime passes, hence 1fps for 10fps in realtime
    private int targetFrameRate = 10;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        Random.seed = 1;
    }

}
