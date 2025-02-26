using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptable Objects/GameInfo")]
public class GameInfo : ScriptableObject
{
    public float speed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float walkSpeed = 5.0f;
    public float mouseSensitivity = 1.0f;
    public float maxPitch = 80.0f;
    public float maxStamina = 100.0f;
    public float staminaDecrease = 0.5f;
    public float staminaIncrease = 0.5f;
    public float ttTime = 3f;
}
