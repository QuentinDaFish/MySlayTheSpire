using DG.Tweening;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.Rendering;

public class CardView : MonoBehaviour, IPointerEnter, IPointerExit, IPointerDown
{
    private SortingGroup sg;
    private BoxCollider2D bc;

    public Card Card { get; private set; }
    [SerializeField] private Transform wrapper;
    [SerializeField] private SpriteRenderer border;
    [SerializeField] private TextMeshPro rarityText;
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private TextMeshPro descText;
    [SerializeField] private SpriteRenderer manaIcon;
    [SerializeField] private TextMeshPro manaText;

    [Header("Move details")]
    [SerializeField] private float moveDuration;
    private Sequence moveTween;

    [Header("Scale details")]
    [SerializeField] private float highlightScale;
    [SerializeField] private float scaleDuration;
    private Tween scaleTween;

    private void Awake()
    {
        sg = GetComponent<SortingGroup>();
        bc = GetComponent<BoxCollider2D>();
    }
    public void Setup(Card card)
    {
        Card = card;
        nameText.text = card.CardName;

        UpdateDesc();
        
        image.sprite = card.Image;
        manaText.text = card.Mana.ToString();

        // set manaIcon
        // set border
        // set rarity

        bc.enabled = true;
        wrapper.localScale = Vector3.zero;
    }

    public void UpdateDesc(Entity target = null) => descText.text = Card.GetDesc(target);

    public void Discard(Vector3 pos)
    {
        if (moveTween != null && moveTween.IsActive()) moveTween.Kill();

        moveTween = DOTween.Sequence();
        moveTween.Append(transform.DOMove(pos, moveDuration).SetEase(Ease.OutCubic));
        moveTween.Join(wrapper.DOScale(0, moveDuration));
        moveTween.Join(transform.DORotate(new Vector3(0f, 0f, 120), moveDuration));

        moveTween.OnComplete(() => gameObject.SetActive(false));
    }

    // pointer
    public void OnPointerEnter()
    {
        if (SelectSystem.Instance.selectedHand != null) return;
        SetScale(highlightScale);
    }
    public void OnPointerExit()
    {
        if (SelectSystem.Instance.selectedHand != null) return;
        SetScale(1);
    }
    public void OnPointerDown()
    {
        if (SelectSystem.Instance.selectedHand == null) SelectSystem.Instance.SelectHand(this);
        else if(!SelectSystem.Instance.readyToPlay) SelectSystem.Instance.SelectHand(null);
    }

    public void SetSortingOrder(int index) => sg.sortingOrder = index;
    public void MoveTo(Vector3 pos, float angle = 0)
    {
        if (moveTween != null && moveTween.IsActive())moveTween.Kill();

        moveTween = DOTween.Sequence();
        moveTween.Append(transform.DOMove(pos, moveDuration).SetEase(Ease.OutCubic));
        moveTween.Join(wrapper.DOScale(1, moveDuration));
        moveTween.Join(transform.DORotate(new Vector3(0f, 0f, angle), moveDuration));
    }
    public void SetScale(float scale)
    {
        if (scaleTween != null && scaleTween.IsActive()) scaleTween.Kill();
        scaleTween = wrapper.DOScale(scale, scaleDuration);
    }
}
