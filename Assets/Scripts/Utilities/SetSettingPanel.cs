using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSettingPanel : MonoBehaviour
{
    [SerializeField] Settings settings;

    [SerializeField] Toggle particles;
    [SerializeField] Toggle animations;
    [SerializeField] Toggle vibrations;

    private void OnEnable()
    {
        particles.isOn = settings.Particles;
        animations.isOn = settings.Animations;
        vibrations.isOn = settings.Vibrations;
    }
}
