using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] private CardView cardViewPrefab;
    private readonly List<CardView> cardViewsPool = new();

    [Header("References")]
    [SerializeField] private Transform handView;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private RectTransform drawPileBtn;
    [SerializeField] private RectTransform discardPileBtn;

    [Header("Layout")]
    [SerializeField, Range(0f, 1f)] private float centerT = 0.5f;
    [SerializeField, Range(0.001f, 1f)] private float spacingT = 0.1f;

    private readonly List<CardView> hands = new();

    public CardView CreateCardView(Card card, Vector2 pos = default)
    {
        CardView view = null;

        for (int i = 0; i < cardViewsPool.Count; i++)
        {
            CardView v = cardViewsPool[i];
            if (v == null) continue;

            if (!v.gameObject.activeSelf)
            {
                view = v;
                break;
            }
        }

        if (view == null)
        {
            view = Instantiate(cardViewPrefab, handView);
            cardViewsPool.Add(view);
        }

        view.gameObject.SetActive(true);
        view.transform.position = pos;
        view.Setup(card);

        return view;
    }

    public void AddHand(Card card)
    {
        if (card == null) return;

        var view = CreateCardView(card, Camera.main.ScreenToWorldPoint(drawPileBtn.position));
        if (view == null) return;

        view.transform.SetParent(transform, worldPositionStays: true);

        hands.Add(view);
        UpdateHands();
    }
    public void RemoveHand(Card card)
    {
        if (card == null) return;

        for (int i = 0; i < hands.Count; i++)
        {
            var view = hands[i];
            if (view == null) continue;

            if (view.Card == card)
            {
                hands.RemoveAt(i);

                view.Discard(Camera.main.ScreenToWorldPoint(discardPileBtn.position));

                UpdateHands();
                return;
            }
        }
    }
    public void UpdateHands()
    {
        if (splineContainer == null) return;

        var spline = splineContainer.Spline;
        int n = hands.Count;
        if (n <= 0) return;

        float startT = centerT - (n - 1) * 0.5f * spacingT;

        for (int i = 0; i < n; i++)
        {
            var view = hands[i];
            if (view == null) continue;

            float t = startT + i * spacingT;
            t = Mathf.Clamp01(t);

            Vector3 pos = splineContainer.transform.position + (Vector3)spline.EvaluatePosition(t) + new Vector3(0, 0, i * -0.01f);
            Vector3 tan = (Vector3)spline.EvaluateTangent(t);
            float angle = Mathf.Atan2(tan.y, tan.x) * Mathf.Rad2Deg;

            view.SetSortingOrder(i);
            view.MoveTo(pos, angle);
        }
    }
    public void UpdateHandsDesc()
    {
        foreach (CardView hand in hands)
        {
            hand.UpdateDesc();
        }
    }
}
