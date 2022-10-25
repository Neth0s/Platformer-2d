using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rumble : MonoBehaviour
{
    [Header("Wall Slide")]
    [SerializeField, Range(0f, 1f)] private float lowS_slide;
    [SerializeField, Range(0f, 1f)] private float highS_slide;
    [SerializeField, Min(0)] private float slidePulse;
    [SerializeField, Min(0)] private float slideDuration;

    [Header("Dash")]
    [SerializeField, Range(0f, 1f)] private float lowS_dash;
    [SerializeField, Range(0f, 1f)] private float highS_dash;
    [SerializeField, Min(0)] private float durationDash;

    [Header("Death")]
    [SerializeField, Range(0f, 1f)] private float lowS_death;
    [SerializeField, Range(0f, 1f)] private float highS_death;
    [SerializeField, Min(0)] private float durationDeath;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        StopRumble();
    }

    //Rumble methods
    private void StartRumble(float lowSpeed, float highSpeed)
    {
        var gamepad = GetGamepad();
        gamepad?.SetMotorSpeeds(lowSpeed, highSpeed);
    }

    private void RumbleImpulse(float lowSpeed, float highSpeed, float duration)
    {
        StartRumble(lowSpeed, highSpeed);
        StartCoroutine(StopRumbleDelayed(duration));
    }

    private IEnumerator RumblePulsation(float lowSpeed, float highSpeed, float pulseLength, float duration)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            RumbleImpulse(lowSpeed, highSpeed, pulseLength);
            yield return new WaitForSeconds(2*pulseLength);
        }
    }

    private IEnumerator StopRumbleDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopRumble();
    }

    public void StopRumble()
    {
        var gamepad = GetGamepad();
        gamepad?.SetMotorSpeeds(0, 0);
    }

    //Rumble presets
    public void DeathRumble() => RumbleImpulse(lowS_death, highS_death, durationDeath);
    public void DashRumble() => RumbleImpulse(lowS_dash, highS_dash, durationDash);
    public void SlideRumble() => StartCoroutine(RumblePulsation(lowS_slide, highS_slide,slidePulse, slideDuration));


    private Gamepad GetGamepad()
    {
        return Gamepad.all.FirstOrDefault(g => _playerInput.devices.Any(d => d.deviceId == g.deviceId));
    }
}