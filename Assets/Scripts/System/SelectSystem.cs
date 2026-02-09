using UnityEngine;

public class SelectSystem : Singleton<SelectSystem>
{
    [Header("Reference")]
    [SerializeField] private float readyLine;
    [SerializeField] private float cancelLine;
    [SerializeField] private Transform aimPoint;

    public CardView selectedHand { get; private set; }
    public bool readyToPlay { get; private set; }
    public bool playable { get; private set; }
    public EntityView target { get; private set; }

    private void Update()
    {
        if (selectedHand != null)
        {
            if (!(selectedHand.Card.NeedTarget && readyToPlay)) selectedHand.MoveTo(PointerSystem.Instance.MousePos());

            Vector2 mousePos = PointerSystem.Instance.MousePos();
            
            if (mousePos.y > readyLine && !readyToPlay)
            {
                if (GameManager.Instance.CurrentMana < selectedHand.Card.Mana)
                {
                    SelectHand(null);
                    return;
                }

                readyToPlay = true;
                if (!selectedHand.Card.NeedTarget) playable = true;
                else selectedHand.MoveTo(aimPoint.position);
            }
            else if(mousePos.y < readyLine && readyToPlay)
            {
                if (!selectedHand.Card.NeedTarget)
                {
                    playable = false;
                    readyToPlay = false;
                }
            }

            if (mousePos.y < cancelLine)
            {
                SelectHand(null);
                return;
            }

            if (PointerSystem.Instance.Down(1))
            {
                SelectHand(null);
            }
            else if (PointerSystem.Instance.Up(0) || PointerSystem.Instance.Down(0))
            {
                if (playable) CardSystem.Instance.Play(selectedHand.Card, target?.Entity);
            }
        }
    }

    public void SelectHand(CardView cardView)
    {
        if (selectedHand != null)
        {
            selectedHand.SetScale(1);
            CardManager.Instance.UpdateHands();
        }

        readyToPlay = false;
        playable = false;
        target = null;

        selectedHand = cardView;
        if (cardView)
        {
            cardView.SetSortingOrder(10);
        }
    }
    public void SetTarget(EntityView entityView)
    {
        if (target != null)
        {
            //
        }

        selectedHand.UpdateDesc(entityView == null ? null : entityView.Entity);

        if (entityView == null)
        {
            target = null;
            if (selectedHand.Card.NeedTarget) playable = false;
        }
        else if (entityView.Entity.Faction == Faction.Enemy)
        {
            target = entityView;
            if (selectedHand.Card.NeedTarget) playable = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-20f, readyLine, 0f), new Vector3(20f, readyLine, 0f));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-20f, cancelLine, 0f), new Vector3(20f, cancelLine, 0f));
    }
}
