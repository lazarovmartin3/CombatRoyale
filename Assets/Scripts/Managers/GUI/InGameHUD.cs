using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour
{
    public TextMeshProUGUI goldAmountTxt;

    private void Start()
    {
      
    }

    private void Update()
    {
        goldAmountTxt.text = GameManager.Instance.GetPlayer().GetGold().ToString();
    }

}
