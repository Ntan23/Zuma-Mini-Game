using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData/NewLevelData")]
public class LevelData : ScriptableObject
{
    public float ballSpeed;
    public int initialBallCount;
    public float ballSpawnInterval;
    public float ballSpacing;
    public Color[] availableColors;
    [Header("For Shoot")]
    public float ballLaunchSpeed;
}
