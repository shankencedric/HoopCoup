using UnityEngine;

/// <summary>
/// Contract for any physics motor. Responsible for exposing velocity intent and application. <br/>
/// Concrete implementations determine <i>how</i> velocity is applied
/// (e.g., smoothed, impulse-based, or direct). <br/><br/>
/// Related: <seealso cref="RigidbodyMotor"/>
/// </summary>
public interface IMotor
{
    #region Setters
    /// <summary> Sets the intended velocity. Splits into horizontal and vertical internally. </summary>
    void SetTargetVelocity(Vector3 velocity);
    /// <summary> Sets only the horizontal (XZ) component of the intended velocity. </summary>
    void SetTargetVelocityXZ(Vector3 velocityXZ);
    /// <summary> Sets only the vertical (Y) component of the intended velocity. </summary>
    void SetTargetVelocityY(float velocityY);
    #endregion

    #region Getters.Intended
    /// <returns> The intended velocity (XZ + Y). </returns>
    Vector3 GetTargetVelocity();
    /// <returns> The intended horizontal (XZ) velocity. </returns>
    Vector3 GetTargetVelocityXZ();
    /// <returns> The intended vertical (Y) velocity. </returns>
    float GetTargetVelocityY();
    #endregion

    #region Getters.Actual
    /// <returns> The actual velocity from the physics body. </returns>
    Vector3 GetVelocity();
    /// <returns> The actual horizontal (XZ) velocity from the physics body. </returns>
    Vector3 GetVelocityXZ();
    /// <returns> The actual vertical (Y) velocity from the physics body. </returns>
    float GetVelocityY();
    #endregion

    #region Applications
    /// <summary>
    /// Applies <paramref name="targetVelocity"/> to the physics body. <br/>
    /// Behavior depends on <paramref name="forceMode"/> and the concrete implementation. <br/>
    /// <list type="bullet">
    ///     <item> <see cref="ForceMode.VelocityChange"/> — direct velocity set, ignores mass. Default for player. </item>
    ///     <item> <see cref="ForceMode.Impulse"/> — instantaneous force accounting for mass. Default for boulder throw. </item>
    /// </list>
    /// </summary>
    void ApplyVelocity(Vector3 targetVelocity, ForceMode forceMode = ForceMode.Force);
    #endregion
}