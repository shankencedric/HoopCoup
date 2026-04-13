using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugSceneManager : MonoBehaviour
{
    public static DebugSceneManager Instance;

    [SerializeField] private InputAction sceneManagerInput;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DebugLogger.Log($"[{name}] Cannot have 2 instances of the same singleton component.", gameObject);
    }

    private void OnEnable()
    {
        sceneManagerInput.Enable();
    }

    private void OnDisable()
    {
        sceneManagerInput.Disable();
    }

    private void Update()
    {
        if (sceneManagerInput.WasPressedThisFrame())
            ReloadScene();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
