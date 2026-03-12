using UnityEngine;

public class BallShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer aimLine;
    private Ball activeBall;
    LevelData levelData;
    GameManager gameManager;
    BallPoolManager ballPoolManager;
    AudioManager audioManager;

    void Start()
    {
        gameManager = GameManager.instance;
        ballPoolManager = BallPoolManager.instance;
        audioManager = AudioManager.instance;

        levelData = gameManager.levelData;

        SpawnBallAmmo();
    }

    public void SpawnBallAmmo()
    {
        activeBall = ballPoolManager.GetBall();
        activeBall.transform.SetParent(firePoint);
        activeBall.transform.localPosition = Vector3.zero;

        activeBall.Init(levelData, true);

        aimLine.enabled = true;
        aimLine.startColor = activeBall.GetCurrentBallColor();
    }

    void Update()
    {
        if(!gameManager.isComplete)
        {
            RotateTowardsMouse();
            UpdateAimLine();

            if (Input.GetMouseButtonDown(0) && activeBall != null)
            {
                Shoot();
            } 
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void UpdateAimLine()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 shootDir = (mousePos - firePoint.position).normalized;

        // First Point Position
        aimLine.SetPosition(0, firePoint.position);
    
        // Last Point Position
        aimLine.SetPosition(1, firePoint.position + (shootDir * 10f));
    }

    void Shoot()
    {
        audioManager.PlaySFX("Shoot");

        Rigidbody2D rb = activeBall.GetRigidbody();
        activeBall.transform.SetParent(null);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = mousePos - transform.position;
        Vector2 shootDir = direction.normalized;
        rb.linearVelocity = shootDir * levelData.ballLaunchSpeed;

        activeBall.EnableTrailRenderer();
        activeBall = null;
        aimLine.enabled = false;

        Invoke("SpawnBallAmmo", 0.5f);
    }
}
