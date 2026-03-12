using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public LevelData levelData;
    [SerializeField] private WinLoseUI winLoseUI;
    [SerializeField] private ParticleSystem winLoseParticleEffect;
    [HideInInspector] public bool isComplete;
    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
    }

    public void GameOver()
    {
        audioManager.PlaySFX("GameOver");

        var main = winLoseParticleEffect.main;
        main.startColor = Color.red;

        winLoseParticleEffect.Play();

        isComplete = true;
        winLoseUI.ShowUI(false);
    }

    public void Victory()
    {
        audioManager.PlaySFX("Victory");
        
        var main = winLoseParticleEffect.main;
        main.startColor = Color.yellow;

        winLoseParticleEffect.Play();

        isComplete = true;
        winLoseUI.ShowUI(true);
    }

    public void RestartGame()
    {
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        winLoseUI.CloseUI();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
