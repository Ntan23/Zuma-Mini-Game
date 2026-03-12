using System.Collections;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public enum BallState 
{ 
    Projectile, OnPath 
}

public class Ball : MonoBehaviour
{
    private Transform[] path;
    private int targetIndex;
    [SerializeField] private SpriteRenderer ballRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer trailRenderer;
    private Color currentColor;
    private BallState currentState;
    LevelData levelData;
    GameManager gameManager;
    BallManager ballManager;
    BallPoolManager ballPoolManager;

    public void Init(LevelData levelData, bool isProjectile)
    {
        gameManager = GameManager.instance;
        ballManager = BallManager.instance;
        ballPoolManager = BallPoolManager.instance;

        this.levelData = levelData;

        int colorCount = levelData.availableColors.Length;
        int randomIndex = Random.Range(0, colorCount);

        currentColor = levelData.availableColors[randomIndex];
        ballRenderer.material.color = currentColor;

        targetIndex = 0;
        trailRenderer.enabled = false;

        if(isProjectile)
        {
            currentState = BallState.Projectile;
            rb.simulated = true;
            
            trailRenderer.startColor = currentColor;
        }
        else
        {
            path = ballManager.GetWaypoints();
            
            if (path != null && path.Length > 0) 
            {
                transform.position = path[0].position;
            }

            currentState = BallState.OnPath;
            rb.simulated = false;
        }
    }

    void FixedUpdate()
    {
        if(currentState == BallState.Projectile && !gameManager.isComplete)
        {
            CheckHitBall();
        }
    }

    public void Move()
    {
        if (targetIndex >= path.Length) return;

        Transform target = path[targetIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            levelData.ballSpeed * Time.fixedDeltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            targetIndex++;

            if (targetIndex == path.Length)
            {
                gameManager.GameOver();
            }
        }
    }

    void CheckHitBall()
    {
        foreach (Ball ball in ballManager.activeBalls)
        {
            float distance = Vector3.Distance(transform.position, ball.transform.position);

            if(distance < levelData.ballSpacing)
            {
                HitBall(ball);
                return;
            }
        }
    }

    void HitBall(Ball hitBall)
    {
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        trailRenderer.enabled = false;

        transform.SetParent(ballPoolManager.GetParent());

        path = ballManager.GetWaypoints();

        ballManager.InsertBall(this, hitBall);

        currentState = BallState.OnPath;
    }

    public void EnableTrailRenderer()
    {
        trailRenderer.enabled = true;
    }

    public int GetCurrentWaypointIndex()
    {
        return targetIndex;
    }

    public void SetWaypointIndex(int index)
    {
        targetIndex = index;
    }

    public Color GetCurrentBallColor()
    {
        return currentColor;
    }

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }
}
