using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ControllerBase
{
    // Start is called before the first frame update
    void Start()
    {
        controllerBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Sprint") && stamina > 0.0f) { Sprint(); }
        else if (Input.GetButtonUp("Sprint")) { isSprinting = false; RechargeSprint(); }
        if ((controllerBody.velocity.magnitude < MAX_VELOCITY && !isSprinting) || 
            (controllerBody.velocity.magnitude < MAX_VELOCITY * sprintMultiplier && isSprinting)) { Move(); }
    }

    private void Move()
    {
        Vector2 targetVelocity = new Vector2
        {
            x = isSprinting ? Input.GetAxis("Horizontal") * sprintMultiplier : Input.GetAxis("Horizontal"),
            y = isSprinting ? Input.GetAxis("Vertical") * sprintMultiplier : Input.GetAxis("Vertical")
        };

        controllerBody.velocity = Vector2.Lerp(controllerBody.velocity, targetVelocity, moveSpeed);
    }
}