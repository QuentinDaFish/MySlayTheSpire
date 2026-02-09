using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscardPileBtnUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pileCountText;
    private Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    private void OnEnable()
    {
        CardSystem.discardPileChanged += UpdatePileCount;
    }
    private void OnDisable()
    {
        CardSystem.discardPileChanged -= UpdatePileCount;
    }
    public void UpdatePileCount(int count)
    {
        pileCountText.text = count.ToString();
    }
}
