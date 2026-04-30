using System.Collections.Generic;
using UnityEngine;

public abstract class TargetPoolBase
{
    protected Target targetPrefab;
    protected Transform parent;
    protected List<Target> targets = new List<Target>();

    public TargetPoolBase(Target targetPrefab, Transform parent, int poolSize)
    {
        this.targetPrefab = targetPrefab;
        this.parent = parent;

        for (int i = 0; i < poolSize; i++)
        {
            AddTargetToPool();
        }
    }

    public virtual Target GetTarget()
    {
        if (targets.Count == 0)
        {
            AddTargetToPool();
        }

        Target target = targets[0];
        targets.RemoveAt(0);

        target.transform.SetParent(null);
        target.SetPool(this);
        target.ResetTarget();

        return target;
    }

    public virtual void ReturnTarget(Target target)
    {
        target.transform.SetParent(parent);
        target.transform.localPosition = Vector3.zero;
        target.transform.rotation = Quaternion.identity;
        target.gameObject.SetActive(false);

        targets.Add(target);
    }

    protected void AddTargetToPool()
    {
        Target newTarget = Object.Instantiate(targetPrefab, parent);

        newTarget.SetPool(this);
        newTarget.gameObject.SetActive(false);

        targets.Add(newTarget);
    }
}

public class NormalTargetPool : TargetPoolBase
{
    public NormalTargetPool(Target targetPrefab, Transform parent, int poolSize)
        : base(targetPrefab, parent, poolSize)
    {
    }
}

public class FastTargetPool : TargetPoolBase
{
    public FastTargetPool(Target targetPrefab, Transform parent, int poolSize)
        : base(targetPrefab, parent, poolSize)
    {
    }
}

public class StrongTargetPool : TargetPoolBase
{
    public StrongTargetPool(Target targetPrefab, Transform parent, int poolSize)
        : base(targetPrefab, parent, poolSize)
    {
    }
}