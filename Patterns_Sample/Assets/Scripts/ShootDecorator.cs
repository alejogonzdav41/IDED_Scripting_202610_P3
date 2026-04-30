using System.Collections;
using UnityEngine;

public abstract class ShootDecorator : ICommand
{
    protected ICommand shootCommand;

    public ShootDecorator(ICommand shootCommand)
    {
        this.shootCommand = shootCommand;
    }

    public virtual void Execute()
    {
        shootCommand.Execute();
    }
}

public class NormalShootDecorator : ShootDecorator
{
    public NormalShootDecorator(ICommand shootCommand) : base(shootCommand)
    {
    }

    public override void Execute()
    {
        base.Execute();
    }
}

public class TripleShootDecorator : ShootDecorator
{
    private MonoBehaviour monoBehaviour;
    private float timeBetweenShots = 0.15f;

    public TripleShootDecorator(ICommand shootCommand, MonoBehaviour monoBehaviour) : base(shootCommand)
    {
        this.monoBehaviour = monoBehaviour;
    }

    public override void Execute()
    {
        monoBehaviour.StartCoroutine(TripleShoot());
    }

    private IEnumerator TripleShoot()
    {
        for (int i = 0; i < 3; i++)
        {
            base.Execute();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}