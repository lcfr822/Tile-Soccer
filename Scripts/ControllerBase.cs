using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    private float sprintRechargeStartTime = 0.0f;
    private float sprintStartTime = 0.0f;
    private const float MAX_STAMINA = 10.0f;

    protected bool isRechargingSprint = false;
    protected bool isSprinting = false;
    protected bool inPossession = false;
    protected bool canAttemptSteal = false;
    protected float stamina = 10.0f;
    protected const float MAX_VELOCITY = 10.0f;
    protected Rigidbody2D controllerBody = null;

    public float moveSpeed = 1.0f;
    public float sprintMultiplier = 1.5f;
    public float sprintTime = 1.5f;
    public float sprintRechargeTime = 1.0f;

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
        if (isSprinting && !isRechargingSprint)
        {
            float timeSinceStarted = Time.time - sprintStartTime;
            float percentComplete = timeSinceStarted / sprintTime;
            stamina = Mathf.Lerp(MAX_STAMINA, 0.0f, percentComplete);
            if (percentComplete >= 1.0f) { isSprinting = false; }
        }
        else if (!isSprinting && isRechargingSprint)
        {
            float timeSinceStarted = Time.time - sprintRechargeStartTime;
            float percentComplete = timeSinceStarted / sprintRechargeTime;
            stamina = Mathf.Lerp(0.0f, MAX_STAMINA, percentComplete);
            if(percentComplete >= 1.0f) { isRechargingSprint = false; }
        }
        ActionControlUpdate();
    }

    protected void ActionControlUpdate()
    {
        canAttemptSteal = !inPossession && !isSprinting && controllerBody.velocity.magnitude > 0.0f;
        Debug.Log("canAttemptSteal: " + canAttemptSteal);
        if (inPossession || isSprinting) { canAttemptSteal = false; }
    }

    protected void RechargeSprint()
    {
        if(stamina > MAX_STAMINA) { stamina = MAX_STAMINA; return; }
        sprintRechargeStartTime = Time.time;
        isRechargingSprint = true;
    }

    protected void Sprint()
    {
        if (stamina < MAX_STAMINA) { return; }
        sprintStartTime = Time.time;
        isSprinting = true;
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, stamina.ToString());
    }
}
