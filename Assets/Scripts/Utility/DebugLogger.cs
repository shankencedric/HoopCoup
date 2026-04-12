using UnityEngine;

public static class DebugLogger
{
    public static void LogIfSelected(string message, GameObject gameObj)
    {
        #if UNITY_EDITOR
            if (UnityEditor.Selection.activeGameObject == gameObj)
                Log(message, gameObj);
        #else
            Log($"Skipping logging of {gameObj.name} debug details (not available for builds).");
        #endif
    }

    public static void LogPhysicsDetailsIfSelected(IMotor motor, GameObject gameObject)
    {
        Vector3 tv = motor.GetTargetVelocity();
        Vector3 cv = motor.GetVelocity();
        LogIfSelected($"{gameObject.name} target velocity: {tv}; current velocity: {cv}", gameObject);
    }

    public static void Log(string message, GameObject gameObj = null)
    {
        Debug.Log(message, gameObj);
    }
}
