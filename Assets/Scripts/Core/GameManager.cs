using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private uint playerScore = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DebugLogger.Log($"[{name}] Cannot have 2 instances of the same singleton component.", gameObject);
    }

    private void Start() => UiHandler.Instance.ChangeScore(playerScore);

    public void Score()
    {
        playerScore++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI() => UiHandler.Instance.ChangeScore(playerScore);    

    public void ResetGame() => DebugSceneManager.Instance.ReloadScene();
}
