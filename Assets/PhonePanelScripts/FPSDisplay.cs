using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public Text fpsText;
    private float deltaTime;

    void Start()
    {
        InvokeRepeating("UpdateFPSDisplay", 0, 5.0f);
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void UpdateFPSDisplay()
    {
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("FPS:{0:0.0}", fps);
    }
}