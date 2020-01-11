using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ControllerBase
{
    void Start()
    {
        controllerBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Sprint") && stamina > 0.0f) { Sprint(); }
        else if (Input.GetButtonUp("Sprint")) { isSprinting = false; RechargeSprint(); }
        if ((controllerBody.velocity.magnitude < MAX_VELOCITY && !isSprinting) ||
            (controllerBody.velocity.magnitude < (MAX_VELOCITY * sprintMultiplier) && isSprinting)) { Move(); }
        MouseLook();
    }

    private void MouseLook()
    {
        Vector3 lookDifference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookDifference.Normalize();
        float zRotation = Mathf.Atan2(lookDifference.y, lookDifference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, zRotation);
    }

    private void Move()
    {
        Vector2 targetVelocity = new Vector2
        {
            x = isSprinting ? Input.GetAxis("Horizontal") * moveSpeed * sprintMultiplier : Input.GetAxis("Horizontal") * moveSpeed,
            y = isSprinting ? Input.GetAxis("Vertical") * moveSpeed * sprintMultiplier : Input.GetAxis("Vertical") * moveSpeed
        };

        controllerBody.velocity = Vector2.Lerp(controllerBody.velocity, targetVelocity, moveSpeed);
    }
}