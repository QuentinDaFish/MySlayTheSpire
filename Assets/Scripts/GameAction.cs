using System.Collections.Generic;
using UnityEngine;

public class GameAction
{
    public List<GameAction> preReactions { get; private set; } = new();
    public List<GameAction> performReactions { get; private set; } = new();
    public List<GameAction> postReactions { get; private set; } = new();
}
