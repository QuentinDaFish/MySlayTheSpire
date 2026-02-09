using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawPileBtnUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pileCountText;
    private Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    private void OnEnable()
    {
        CardSystem.drawPileChanged += UpdatePileCount;
    }
    private void OnDisable()
    {
        CardSystem.drawPileChanged -= UpdatePileCount;
    }
    public void UpdatePileCount(int count)
    {
        pileCountText.text = count.ToString();
    }
}
