using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SubInfo
{
    [field: SerializeField] public Keyword keyword { get; private set; }
    [field: SerializeField] public ReactionTiming timing { get; private set; }
    [field: SerializeField] public List<WrappedEffects> wrappers { get; private set; }
}

public enum Keyword
{
    TurnBegin,
    TurnOver,
}