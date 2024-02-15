using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour
{
    public VNectBarracudaRunner15Basket VNectModel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.1f;
        Debug.Log(Time.realtimeSinceStartup);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        VNectModel.ToggleAnimation();
    }

    public void ResumeGame()
    {
        VNectModel.StartCoroutine(VNectModel.WaitLoad());
    }
}
