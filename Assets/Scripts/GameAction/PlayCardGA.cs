using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card Card;
    public Entity Target;
    public PlayCardGA(Card card, Entity target)
    {
        Card = card;
        Target = target;
    }
}
