using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateController1 : MonoBehaviour
{
    
    // App time scale set to 0.1, meaning 10s app time passes as 1s realtime passes, hence 1fps for 10fps in realtime
    public int targetFrameRate = 60;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        Random.seed = 1;
    }

}
