using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float MoveSpeed;
    public float RotationSpeed;
    public float JumpHeight;
}
