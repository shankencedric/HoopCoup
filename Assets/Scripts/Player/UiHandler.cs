using TMPro;
using UnityEngine;

public class UiHandler : MonoBehaviour
{
    public static UiHandler Instance;

    [SerializeField] private TextMeshProUGUI scoretext;
    [SerializeField] private string scoretextPrefix = "SCORE: ";
    [SerializeField] private string scoretextSuffix = "";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DebugLogger.Log($"[{name}] Cannot have 2 instances of the same singleton component.", gameObject);
    }

    private void Start() 
    {
        ShowCursor(false);
    }

    private void ShowCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
    
    public void ChangeScore(float newScore)
    {
        scoretext.text = scoretextPrefix + newScore.ToString() + scoretextSuffix; 
    }
}