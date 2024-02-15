using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTransparency : MonoBehaviour
{
    public float alpha = 0.5f;

    private void Update()
    {
        Color color = GetComponent<Renderer>().material.color;
        color.a = alpha;
        GetComponent<Renderer>().material.color = color;
    }
}
