using UnityEngine;

[CreateAssetMenu]
public class StaticData : ScriptableObject
{ 
    [Header("Player config")]
    public GameObject PlayerPrefab;
    public float PlayerSpeed;
    [Header("Camera config")]
    public float SmoothTime; 
    public Vector3 FollowOffset;
    [Header("Turret config")]
    public GameObject TurretPrefab;
}
