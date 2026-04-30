using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour
{
    public const int PLAYER_LIVES = 3;
    public const float PLAYER_RADIUS = 0.4F;

    private static Player instance;

    [SerializeField]
    private Transform bulletSpawnPoint;

    public int Score { get; set; }
    public int Lives { get; set; }

    public float HVal { get; private set; }

    public Transform BulletSpawnPoint => bulletSpawnPoint;

    private MovementCommand movementCommand;

    private ShootCommand shootCommand;
    private ICommand currentShootCommand;

    private Coroutine tripleShootCoroutine;

    public Action OnPlayerDied;
    public Action OnPlayerHit;

    public static Player Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Lives = PLAYER_LIVES;

        OnPlayerHit += PlayerHit;
        OnPlayerDied += PlayerDied;
        Target.onTargetDestroyed += AddScore;

        movementCommand = gameObject.GetComponent<MovementCommand>();
        shootCommand = gameObject.GetComponent<ShootCommand>();

        currentShootCommand = new NormalShootDecorator(shootCommand);
    }

    private void PlayerHit()
    {
        Lives -= 1;

        if (Lives <= 0 && OnPlayerDied != null)
        {
            OnPlayerDied();
        }
    }

    private void AddScore(int scoreAdd)
    {
        Score += scoreAdd;
    }

    private void PlayerDied()
    {
        OnPlayerDied = null;
        OnPlayerHit = null;
        Target.onTargetDestroyed -= AddScore;

        this.enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        HVal = Input.GetAxis("Horizontal");

        if (HVal != 0F)
        {
            movementCommand?.Execute();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            currentShootCommand?.Execute();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ActivateTripleShoot(5f);
        }
    }

    public void ActivateTripleShoot(float duration)
    {
        if (tripleShootCoroutine != null)
        {
            StopCoroutine(tripleShootCoroutine);
        }

        tripleShootCoroutine = StartCoroutine(TripleShootRoutine(duration));
    }

    private IEnumerator TripleShootRoutine(float duration)
    {
        currentShootCommand = new TripleShootDecorator(shootCommand, this);

        yield return new WaitForSeconds(duration);

        currentShootCommand = new NormalShootDecorator(shootCommand);

        tripleShootCoroutine = null;
    }
}