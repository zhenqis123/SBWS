using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Players : MonoBehaviour
{
    public TextMeshProUGUI PlayerName;
    public string MyName;
    public TextMeshProUGUI CardNumber;
    public string MyCardNumber;
    public Image PlayerImage;
    public Sprite MyImage;

    private 
    // Start is called before the first frame update
    void Start()
    {
        PlayerName.text = MyName;
        CardNumber.text = MyCardNumber;
        PlayerImage.sprite = MyImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
