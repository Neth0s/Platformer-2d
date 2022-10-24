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

    private float rumbleEnd;
    private float pulseDuration;

    private float lowAmplitude;
    private float highAmplitude;

    private float lowStep;
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
        if (Time.time > rumbleEnd)
        {
            StopRumble();
            return;
        }

        var gamepad = GetGamepad();
        if (gamepad == null) return;

        switch (rumblePattern)
        {
            case RumblePattern.Constant:
                gamepad.SetMotorSpeeds(lowAmplitude, highAmplitude);
                break;

            case RumblePattern.Pulse:

                if (Time.time > pulseDuration)
                {
                    isMotorActive = !isMotorActive;
                    pulseDuration = Time.time + rumbleStep;

                    if (!isMotorActive) gamepad.SetMotorSpeeds(0, 0);
                    else gamepad.SetMotorSpeeds(lowAmplitude, highAmplitude);
                }
                break;

            case RumblePattern.Linear:
                gamepad.SetMotorSpeeds(lowAmplitude, highAmplitude);
                lowAmplitude += (lowStep * Time.deltaTime);
                highAmplitude += (highStep * Time.deltaTime);
                break;

            default:
                break;
        }
    }

    public void RumbleConstant(float low, float high, float duration)
    {
        rumblePattern = RumblePattern.Constant;
        lowAmplitude = low;
        highAmplitude = high;
        rumbleEnd = Time.time + duration;
    }

    public void RumblePulse(float low, float high, float burstTime, float duration)
    {
        rumblePattern = RumblePattern.Pulse;

        lowAmplitude = low;
        highAmplitude = high;
        rumbleStep = burstTime;

        pulseDuration = Time.time + burstTime;
        rumbleEnd = Time.time + duration;

        isMotorActive = true;

        var g = GetGamepad();
        g?.SetMotorSpeeds(lowAmplitude, highAmplitude);
    }

    public void RumbleLinear(float lowStart, float lowEnd, float highStart, float highEnd, float duration)
    {
        rumblePattern = RumblePattern.Linear;
        lowAmplitude = lowStart;
        highAmplitude = highStart;

        lowStep = (lowEnd - lowStart) / duration;
        highStep = (highEnd - highStart) / duration;

        rumbleEnd = Time.time + duration;
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