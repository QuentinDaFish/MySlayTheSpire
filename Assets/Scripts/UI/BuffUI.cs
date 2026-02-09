using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI stackText;

    public void Setup(Buff buffInfo)
    {
        image.sprite = buffInfo.Image;
        stackText.gameObject.SetActive(true);
    }
    public void UpdateStack(int amount)
    {
        stackText.text = amount.ToString();
    }
    public void HideStack() => stackText.gameObject.SetActive(false);
}
