using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour
{
    public TextMeshProUGUI goldAmountTxt;
    public Button soldierBtn;


    private void Start()
    {
        soldierBtn.onClick.AddListener(() => GameManager.Instance.GetPlayer().GetSelectedCastle().CreateUnit(UnitCreator.UnitType.swordman));
    }

    private void Update()
    {
        goldAmountTxt.text = GameManager.Instance.GetPlayer().GetGold().ToString();
    }
}
