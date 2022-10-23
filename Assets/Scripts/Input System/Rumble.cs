using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public enum RumblePattern
{
    Constant,
    Pulse,
    Linear
}

public class Rumble : MonoBehaviour
{
    private PlayerInput _playerInput;
    private RumblePattern rumblePattern;

    private float rumbleDuration;
    private float pulseDuration;

    private float lowA;
    private float lowStep;
    private float highA;
    private float highStep;
    private float rumbleStep;

    private bool isMotorActive = false;


    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        StopRumble();
    }

    private void Update()
    {
        if (Time.time > rumbleDuration)
        {
            StopRumble();
            return;
        }

        var gamepad = GetGamepad();
        if (gamepad == null) return;

        switch (rumblePattern)
        {
            case RumblePattern.Constant:
                gamepad.SetMotorSpeeds(lowA, highA);
                break;

            case RumblePattern.Pulse:

                if (Time.time > pulseDuration)
                {
                    isMotorActive = !isMotorActive;
                    pulseDuration = Time.time + rumbleStep;

                    if (!isMotorActive) gamepad.SetMotorSpeeds(0, 0);
                    else gamepad.SetMotorSpeeds(lowA, highA);
                }
                break;

            case RumblePattern.Linear:
                gamepad.SetMotorSpeeds(lowA, highA);
                lowA += (lowStep * Time.deltaTime);
                highA += (highStep * Time.deltaTime);
                break;

            default:
                break;
        }
    }

    public void RumbleConstant(float low, float high, float duration)
    {
        rumblePattern = RumblePattern.Constant;
        lowA = low;
        highA = high;
        rumbleDuration = Time.time + duration;
    }

    public void RumblePulse(float low, float high, float burstTime, float durration)
    {
        rumblePattern = RumblePattern.Pulse;
        lowA = low;
        highA = high;
        rumbleStep = burstTime;
        pulseDuration = Time.time + burstTime;
        rumbleDuration = Time.time + durration;
        isMotorActive = true;
        var g = GetGamepad();
        g?.SetMotorSpeeds(lowA, highA);
    }

    public void RumbleLinear(float lowStart, float lowEnd, float highStart, float highEnd, float durration)
    {
        rumblePattern = RumblePattern.Linear;
        lowA = lowStart;
        highA = highStart;
        lowStep = (lowEnd - lowStart) / durration;
        highStep = (highEnd - highStart) / durration;
        rumbleDuration = Time.time + durration;
    }

    public void StopRumble()
    {
        var gamepad = GetGamepad();
        if (gamepad != null) gamepad.SetMotorSpeeds(0, 0);
    }


    private Gamepad GetGamepad()
    {
        return Gamepad.all.FirstOrDefault(g => _playerInput.devices.Any(d => d.deviceId == g.deviceId));
    }
}