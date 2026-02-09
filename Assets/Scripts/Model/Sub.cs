using UnityEngine;

public abstract class Sub
{
    protected Entity Holder;
    protected SubInfo Info;
    public Sub(Entity holder, SubInfo info)
    {
        Holder = holder;
        Info = info;
    }
    public abstract void Add();
    public abstract void Remove();
}