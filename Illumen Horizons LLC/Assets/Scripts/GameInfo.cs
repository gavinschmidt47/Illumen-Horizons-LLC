using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptable Objects/GameInfo")]
public class GameInfo : ScriptableObject
{
    //Move stats
    public float speed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float walkSpeed = 5.0f;

    //Mouse stats
    public float mouseSensitivity = 1.0f;
    public float maxPitch = 80.0f;

    //Stamina
    public float maxStamina = 100.0f;
    public float staminaDecrease = 0.5f;
    public float staminaIncrease = 0.5f;

    //Tool tip
    public float ttTime = 3f;

    //Start tracker
    public bool inStart = true;

    //Enemy stats
    public int ability;
}
