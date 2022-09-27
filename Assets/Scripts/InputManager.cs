using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    [SerializeField] private float amplitude = 2;

    [Header("Colors")]
    [SerializeField] private Color jumpColor;

    private SpriteRenderer sprite;
    private Manette inputActions;

    private Vector2 movement = Vector2.zero;
    private float jump;

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

        jump = inputActions.Player.Jump.ReadValue<float>();

        if (jump != 0) sprite.color = jumpColor;
        else sprite.color = Color.white;
    }
}
