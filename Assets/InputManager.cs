using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    [SerializeField] private float amplitude = 2;

    [Header("Colors")]
    [SerializeField] private Color up;
    [SerializeField] private Color down;
    [SerializeField] private Color left;
    [SerializeField] private Color right;

    private SpriteRenderer sprite;
    private Manette inputActions;

    private Vector2 movement = Vector2.zero;
    private float buttonUp;
    private float buttonDown;
    private float buttonLeft;
    private float buttonRight;

    // Start is called before the first frame update
    void Start()
    {
        sprite = circle.GetComponent<SpriteRenderer>();
        inputActions = new Manette();
        inputActions.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        movement = inputActions.Player.Move.ReadValue<Vector2>();
        circle.transform.position = amplitude * movement.normalized;

        buttonUp = inputActions.Player.Buttonup.ReadValue<float>();
        buttonDown = inputActions.Player.Buttondown.ReadValue<float>();
        buttonLeft = inputActions.Player.Buttonleft.ReadValue<float>();
        buttonRight = inputActions.Player.Buttonright.ReadValue<float>();

        if (buttonUp != 0) sprite.color = up;
        else if (buttonDown != 0) sprite.color = down;
        else if (buttonLeft != 0) sprite.color = left;
        else if (buttonRight != 0) sprite.color = right;
        else sprite.color = Color.white;
    }
}
