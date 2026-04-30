using UnityEngine;

public enum TargetType
{
    Normal,
    Fast,
    Strong
}

public class TargetFacade : MonoBehaviour
{
    private static TargetFacade instance;

    public static TargetFacade Instance => instance;

    [Header("Target Prefabs")]
    [SerializeField]
    private Target normalTargetPrefab;

    [SerializeField]
    private Target fastTargetPrefab;

    [SerializeField]
    private Target strongTargetPrefab;

    [Header("Pool")]
    [SerializeField]
    private int poolSize = 5;

    private NormalTargetPool normalTargetPool;
    private FastTargetPool fastTargetPool;
    private StrongTargetPool strongTargetPool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        normalTargetPool = new NormalTargetPool(normalTargetPrefab, transform, poolSize);
        fastTargetPool = new FastTargetPool(fastTargetPrefab, transform, poolSize);
        strongTargetPool = new StrongTargetPool(strongTargetPrefab, transform, poolSize);
    }

    public Target CreateInstance()
    {
        int randomValue = Random.Range(0, 3);
        TargetType targetType = (TargetType)randomValue;

        return CreateInstance(targetType);
    }

    public Target CreateInstance(TargetType targetType)
    {
        if (targetType == TargetType.Fast)
        {
            return fastTargetPool.GetTarget();
        }

        if (targetType == TargetType.Strong)
        {
            return strongTargetPool.GetTarget();
        }

        return normalTargetPool.GetTarget();
    }
}