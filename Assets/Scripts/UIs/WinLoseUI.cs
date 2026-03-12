using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winLoseText;
    [SerializeField] private Animator animator;
    [SerializeField] private Button restartButton;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;

        restartButton.onClick.AddListener(() => gameManager.RestartGame());
    }

    public void ShowUI(bool isWin)
    {
        animator.Play("ShowUI");

        if(isWin)
        {
            winLoseText.text = "Victory";
            winLoseText.color = Color.yellow;
        }
        else if(!isWin)
        {
            winLoseText.text = "Game Over";
            winLoseText.color = Color.red;
        }
    }

    public void CloseUI()
    {
        animator.Play("CloseUI");
    }
}
