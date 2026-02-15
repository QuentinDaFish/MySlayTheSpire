using System.Collections.Generic;
using UnityEngine;

public class DiscardCardGA : GameAction
{
    public Card Card;
    public DiscardCardGA(Card card)
    {
        Card = card;
    }
}
