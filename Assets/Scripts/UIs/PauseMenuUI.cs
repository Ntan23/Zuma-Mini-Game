using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Animator animator;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;

        resumeButton.onClick.AddListener(() => gameManager.ResumeGame());
    }

    public void ShowUI()
    {
        animator.Play("ShowUI");
    }

    public void CloseUI()
    {
        animator.Play("CloseUI");
    }
}
