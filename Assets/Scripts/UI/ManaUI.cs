using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI manaText;

    private void OnEnable()
    {
        GameManager.ManaChanged += UpdateManaUI;
    }
    private void OnDisable()
    {
        GameManager.ManaChanged -= UpdateManaUI;
    }

    public void UpdateManaUI(int current, int max)
    {
        manaText.text = $"{current}/{max}";
    }
}
