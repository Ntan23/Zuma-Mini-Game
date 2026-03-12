using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Singleton
    public static BallManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion
    [SerializeField] private Transform[] waypoints;
    private BallPoolManager ballPoolManager; 
    private LevelData levelData;
    public List<Ball> activeBalls;
    [SerializeField] private ParticleSystem particleEffect;
    GameManager gameManager;
    AudioManager audioManager;

    void Start()
    {
        gameManager = GameManager.instance;
        audioManager = AudioManager.instance;

        activeBalls = new List<Ball>();
        
        ballPoolManager = BallPoolManager.instance;
        levelData = GameManager.instance.levelData;

        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls()
    {
        int ballCount = levelData.initialBallCount;

        for(int i = 0; i < ballCount; i++)
        {
            Ball ball = ballPoolManager.GetBall();
            ball.Init(levelData, false);

            activeBalls.Add(ball);

            yield return new WaitForSeconds(levelData.ballSpawnInterval);
        }
    }

    void Update()
    {
        if(!gameManager.isComplete && !gameManager.isPaused) 
        {
            MoveBalls();
        }
    }

    void MoveBalls()
    {
        if (activeBalls.Count == 0) return;

        activeBalls[0].Move();

        for (int i = 1; i < activeBalls.Count; i++)
        {
            Ball front = activeBalls[i - 1];
            Ball current = activeBalls[i];

            float distance = Vector3.Distance(
                front.transform.position,
                current.transform.position
            );

            if (distance >= levelData.ballSpacing)
            {
                current.Move();
            }
        }
    }

    public void InsertBall(Ball newBall, Ball hitBall)
    {
        int hitIndex = activeBalls.IndexOf(hitBall);
        if (hitIndex < 0) return;

        int waypointIndex = hitBall.GetCurrentWaypointIndex();
        newBall.SetWaypointIndex(waypointIndex);

        Vector3 dir;

        if (hitIndex < activeBalls.Count - 1)
        {
            // If there is a ball in front of current ball
            Ball frontBall = activeBalls[hitIndex + 1];
            dir = (hitBall.transform.position - frontBall.transform.position).normalized;
        }
        else
        {
            // Hit the first ball
            Transform[] waypoints = GetWaypoints();
            dir = (waypoints[waypointIndex].position - hitBall.transform.position).normalized;
        }

        newBall.transform.position = hitBall.transform.position - dir * levelData.ballSpacing;

        activeBalls.Insert(hitIndex + 1, newBall);

        CheckMatch(hitIndex + 1);
    }

    void CheckMatch(int index)
    {
        if (index < 0 || index >= activeBalls.Count) return;

        Color targetColor = activeBalls[index].GetCurrentBallColor();

        List<int> matchIndexes = new List<int>();
        matchIndexes.Add(index);

        // Check Left
        int left = index - 1;
        while (left >= 0 && activeBalls[left].GetCurrentBallColor() == targetColor)
        {
            matchIndexes.Add(left);
            left--;
        }

        // Check Right
        int right = index + 1;
        while (right < activeBalls.Count && activeBalls[right].GetCurrentBallColor() == targetColor)
        {
            matchIndexes.Add(right);
            right++;
        }

        // If 3 or more destroy (Release) balls
        if (matchIndexes.Count >= 3)
        {
            StartCoroutine(DestroyMatch(matchIndexes));
        }
    }

    IEnumerator DestroyMatch(List<int> matchIndexes)
    {
        yield return new WaitForSeconds(0.05f);

        // Sort for safe remove
        matchIndexes.Sort();
        matchIndexes.Reverse();

        //Explode effect
        int middleBallIndex = Mathf.FloorToInt(matchIndexes.Count / 2);
        Ball currentBall = activeBalls[matchIndexes[middleBallIndex]];

        var main = particleEffect.main;
        main.startColor = currentBall.GetCurrentBallColor();

        particleEffect.transform.position = currentBall.transform.position;
        particleEffect.Play();

        //Release balls
        foreach(int i in matchIndexes)
        {
            Ball ball = activeBalls[i];
            activeBalls.RemoveAt(i);
            ballPoolManager.ReturnBall(ball);
        }

        audioManager.PlaySFX("Explode");
        particleEffect.Play();

        if(activeBalls.Count == 0)
        {
            gameManager.Victory();
        }
    }

    public Transform[] GetWaypoints()
    {
        return waypoints;
    }
}
