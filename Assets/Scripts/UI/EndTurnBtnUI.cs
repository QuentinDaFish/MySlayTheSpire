using UnityEngine;
using UnityEngine.UI;

public class EndTurnBtnUI : MonoBehaviour
{
    private Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    private void OnEnable()
    {
        btn.onClick.AddListener(() => GameManager.Instance.PlayerTurnOver());
    }
    private void OnDisable()
    {
        btn.onClick.RemoveAllListeners();
    }
}
