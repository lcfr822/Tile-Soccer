using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    private float sprintRechargeStartTime = 0.0f;
    private float sprintRechargeStartValue = 0.0f;
    private float sprintStartTime = 0.0f;
    private float sprintStartValue = 0.0f;
    private const float MAX_STAMINA = 10.0f;
    private const float MAX_STAMINA_RECHARGE_TIME = 1.5f;
    private const float MAX_SPRINT_TIME = 1.5f;

    protected bool isRechargingSprint = false;
    protected bool isSprinting = false;
    protected bool inPossession = false;
    protected bool canAttemptSteal = false;
    protected float stamina = 10.0f;
    protected const float MAX_VELOCITY = 10.0f;
    protected Rigidbody2D controllerBody = null;

    public float moveSpeed = 1.0f;
    public float sprintMultiplier = 1.5f;
    public float calculatedSprintTime = 1.5f;
    public float calculatedSprintRechargeTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // Sprint depleting stamina
        if (isSprinting && !isRechargingSprint)
        {
            float timeSinceStarted = Time.time - sprintStartTime;
            float percentComplete = timeSinceStarted / calculatedSprintTime;
            stamina = Mathf.Lerp(sprintStartValue, 0.0f, percentComplete);
            if (percentComplete >= 1.0f) { isSprinting = false; }
        }
        // Recharging stamina
        else if (!isSprinting && isRechargingSprint)
        {
            float timeSinceStarted = Time.time - sprintRechargeStartTime;
            float percentComplete = timeSinceStarted / calculatedSprintRechargeTime;
            stamina = Mathf.Lerp(sprintRechargeStartValue, MAX_STAMINA, percentComplete);
            if (percentComplete >= 1.0f) { isRechargingSprint = false; }
        }
        ActionControlUpdate();
    }

    protected void ActionControlUpdate()
    {
        canAttemptSteal = !inPossession && !isSprinting && controllerBody.velocity.magnitude > 0.0f;
        if (inPossession || isSprinting) { canAttemptSteal = false; }
    }

    protected void RechargeSprint()
    {
        if(stamina >= MAX_STAMINA) { stamina = MAX_STAMINA; return; }
        calculatedSprintRechargeTime = (stamina / MAX_STAMINA).Map(0.0f, MAX_STAMINA, 1.0f, 0.0f) * MAX_STAMINA_RECHARGE_TIME;
        sprintRechargeStartTime = Time.time;
        sprintRechargeStartValue = stamina;
        isRechargingSprint = true;
    }

    protected void Sprint()
    {
        if(stamina < MAX_STAMINA / 2.0f) { return; }
        if (isRechargingSprint) { isRechargingSprint = false; }
        calculatedSprintTime = (stamina / MAX_STAMINA) * MAX_SPRINT_TIME;
        sprintStartTime = Time.time;
        sprintStartValue = stamina;
        isSprinting = true;
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, "Stamina: " + stamina.ToString());
    }
}
