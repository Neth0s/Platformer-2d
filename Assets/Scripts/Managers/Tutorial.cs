using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private bool isTutorial = false;
    [SerializeField] private GameObject keyboardInputs;
    [SerializeField] private GameObject gamepadInputs;

    private void OnEnable()
    {
        StartCoroutine(WaitForGamepadDetection());
    }

    public IEnumerator WaitForGamepadDetection()
    {
        yield return new WaitForSeconds(2);
        DisplayControls();
    }

    public void DisplayControls()
    {
        if (!isTutorial) return;

        var gamepad = GetGamepad();
        Debug.Log(gamepad);
        if (gamepad == null)
        {
            keyboardInputs.SetActive(true);
            gamepadInputs.SetActive(false);
        }
        else
        {
            gamepadInputs.SetActive(true);
            keyboardInputs.SetActive(false);
        }
    }

    private Gamepad GetGamepad()
    {
        return Gamepad.all.FirstOrDefault(g => _playerInput.devices.Any(d => d.deviceId == g.deviceId));
    }
}
