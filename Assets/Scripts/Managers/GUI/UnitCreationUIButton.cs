using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitCreationUIButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitNameTxt;
    [SerializeField] private TextMeshProUGUI numOfUnitsTxt;
    [SerializeField] private Image progressImage;

    public void UpdateProgress(float progress)
    {
        if (progress >= 0.95f)
        {
            progressImage.gameObject.SetActive(false);
        }
        else
        {
            progressImage.gameObject.SetActive(true);
            progressImage.transform.localScale = new Vector3(progress, 1, 1);
        }
    }

    public void UpdateUnitsAmount(int amount)
    {
        numOfUnitsTxt.text = amount.ToString();
    }
}
