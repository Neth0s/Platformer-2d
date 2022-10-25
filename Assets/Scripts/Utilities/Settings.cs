using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings")]
public class Settings : ScriptableObject
{
    [field: SerializeField] public bool Particles { get; set; } = true;
    [field: SerializeField] public bool Animations { get; set; } = true;
    [field: SerializeField] public bool Vibrations { get; set; } = true;

    [field: SerializeField] public bool RetryMode { get; set; } = false;
}
