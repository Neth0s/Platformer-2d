using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSettingPanel : MonoBehaviour
{
    [SerializeField] Settings settings;

    [SerializeField] Toggle dashEffects;
    [SerializeField] Toggle deathEffects;
    [SerializeField] Toggle movementParticles;

    private void OnEnable()
    {
        dashEffects.isOn = settings.DashEffects;
        deathEffects.isOn = settings.DeathEffects;
        movementParticles.isOn = settings.MovementParticles;
    }
}
