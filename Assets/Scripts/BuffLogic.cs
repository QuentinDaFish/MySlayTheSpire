using UnityEngine;

public abstract class BuffLogic
{
    public virtual void OnAdd(Entity holder) { }
    public virtual void OnRemove(Entity holder) { }
    public virtual void OnChange(Entity holder, int amount) { }
}
