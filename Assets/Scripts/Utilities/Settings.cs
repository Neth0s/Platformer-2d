using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings")]
public class Settings : ScriptableObject
{
    [field: SerializeField] public bool DashEffects { get; set; }
    [field: SerializeField] public bool DeathEffects { get; set; }
    [field: SerializeField] public bool MovementParticles { get; set; }
}
