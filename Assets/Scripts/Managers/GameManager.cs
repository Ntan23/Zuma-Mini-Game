using System;
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
    [SerializeField] private PauseMenuUI pauseMenuUI;
    [SerializeField] private ParticleSystem winLoseParticleEffect;
    [HideInInspector] public bool isComplete;
    [HideInInspector] public bool isPaused;
    private bool canPauseResume;
    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;

        canPauseResume = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && canPauseResume)
        {
            if(!isPaused)
            {
                PauseGame();
            }
            else if (isPaused)
            {
                ResumeGame();
            }
        }
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

    public void PauseGame()
    {
        StartCoroutine(Pause());
    }

    public void ResumeGame()
    {
        StartCoroutine(Resume());
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

    IEnumerator Pause()
    {
        canPauseResume = false;
        pauseMenuUI.ShowUI();
        yield return new WaitForSeconds(0.5f);
        isPaused = true;
        canPauseResume = true;
        Time.timeScale = 0;
    }

    IEnumerator Resume()
    {
        canPauseResume = false;
        Time.timeScale = 1;
        pauseMenuUI.CloseUI();
        yield return new WaitForSeconds(0.5f);
        isPaused = false;
        canPauseResume = true;
    }
}
