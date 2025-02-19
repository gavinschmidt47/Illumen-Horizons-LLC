using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptable Objects/GameInfo")]
public class GameInfo : ScriptableObject
{
    public float speed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float walkSpeed = 5.0f;
    public float yawSpeed = 200.0f;
    public float pitchSpeed = 200.0f;
    public float maxPitch = 80.0f;
}
