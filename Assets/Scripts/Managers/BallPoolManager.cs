using System;
using UnityEngine;
using UnityEngine.Pool;

public class BallPoolManager : MonoBehaviour
{
    public static BallPoolManager instance;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform parent;
    private IObjectPool<Ball> pool;

    void Awake()
    {
        //Singelton
        if(instance == null)
        {
            instance = this;
        }

        //Init New Ball Pool
        pool = new ObjectPool<Ball>(
            createFunc: () => SpawnBall(),
            actionOnGet: (ball) => OnGetBall(ball),
            actionOnRelease: (ball) => OnReturnBall(ball),
            actionOnDestroy: (ball) => OnDestroyBall(ball),
            defaultCapacity: 20,
            maxSize: 50
        );
    }

    public Ball SpawnBall()
    {
        Ball ball = Instantiate(ballPrefab, parent);

        return ball;
    }

    public void OnGetBall(Ball ball)
    {
        ball.gameObject.SetActive(true);
    }

    public void OnReturnBall(Ball ball)
    {
        ball.gameObject.SetActive(false);
    }

    public void OnDestroyBall(Ball ball)
    {
        Destroy(ball.gameObject);
    }

    public Ball GetBall() 
    {
        return pool.Get();
    }

    public void ReturnBall(Ball ball) 
    {
        pool.Release(ball);
    }

    public Transform GetParent()
    {
        return parent;
    }
}
