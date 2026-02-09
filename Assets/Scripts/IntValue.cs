using System;
using UnityEngine;

[Serializable]
public abstract class IntValue
{
    public abstract int Eval(EffectContext ctx);
}

[Serializable]
public class ConstInt : IntValue
{
    public int v;
    public override int Eval(EffectContext ctx) => v;
}

[Serializable]
public class ValueInt : IntValue
{
    public string id;
    public override int Eval(EffectContext ctx) => ctx.Get<int>(id);
}

[Serializable]
public class MulInt : IntValue
{
    [SerializeReference] public IntValue a;
    public int k;
    public override int Eval(EffectContext ctx) => a.Eval(ctx) * k;
}