using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movespeed = 1f;
    [SerializeField] private float rotationSpeed = 10f; // Adjust rotation speed for smoothing
    [SerializeField] private Transform lightSource; // Drag the light object here in the Inspector
    public Item Weapon;
    private PlayerControlls playerController;
    private Vector2 movement;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerController = new PlayerControlls();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerController.Enable();
    }

    private void UseWeapon()
    {
        // can i solve that null using get_component<Shotgun>?? 
        if(Input.GetButtonDown("Fire1") && Weapon != null){
            Debug.Log("Weapon used: " + Weapon.name);
            Weapon.UseItem(GetComponent<PlayerStats>());
        }
    }

    private void Update()
    {
        PlayerInput();
        RotatePlayerToMouse(); // Rotate based on mouse position
        UseWeapon();

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        movement = playerController.Movement.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        rb.velocity = movement * movespeed;
    }

    private void RotatePlayerToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        // Calculate the desired angle
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Smooth the rotation
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        // Rotate the light source with the player
        if (lightSource != null)
        {
            lightSource.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
    }
}
