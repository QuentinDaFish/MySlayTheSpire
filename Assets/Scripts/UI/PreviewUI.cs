using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewUI : MonoBehaviour
{
    private CanvasGroup cg;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    private Tween showTween;
    [SerializeField] private float showDuration;
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }
    public void UpdateUI(ExposeType type, int attackTimes, int damage)
    {
        cg.alpha = 0;

        if (attackTimes == 0)
        {
            text.gameObject.SetActive(false);
            return;
        }
        text.gameObject.SetActive(true);
        text.text = attackTimes > 1 ? (attackTimes.ToString() + " X ") : "" + damage.ToString();

        if (showTween != null && showTween.IsActive()) showTween.Kill();
        showTween = cg.DOFade(1, showDuration);
    }
}
