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
    [Tooltip("Setting this value sets the maximum Sprint Time.")]
    public float calculatedSprintTime = 1.5f;
    [Tooltip("Setting this value sets the maximum Stamina Recharge Time.")]
    public float calculatedSprintRechargeTime = 1.0f;
    public Team playerTeam;

    private void FixedUpdate()
    {
        // Lerps stamina value > 0 to 0 by fractions of a given length of time.
        // Initiates stamina recharging on stamina depletion.
        if (isSprinting && !isRechargingSprint)
        {
            float timeSinceStarted = Time.time - sprintStartTime;
            float percentComplete = timeSinceStarted / calculatedSprintTime;
            stamina = Mathf.Lerp(sprintStartValue, 0.0f, percentComplete);
            if (percentComplete >= 1.0f) { isSprinting = false; RechargeSprint(); }
        }
        // Lerps stamina value > 0 to MAX_STAMINA by fractions of a length of time.
        else if (!isSprinting && isRechargingSprint)
        {
            float timeSinceStarted = Time.time - sprintRechargeStartTime;
            float percentComplete = timeSinceStarted / calculatedSprintRechargeTime;
            stamina = Mathf.Lerp(sprintRechargeStartValue, MAX_STAMINA, percentComplete);
            if (percentComplete >= 1.0f) { isRechargingSprint = false; }
        }
        if (!FindObjectOfType<MatchManager>().playBoundary.Contains(transform.position))
        {
            FindObjectOfType<MatchManager>().OutOfBounds(this);
        }
        ActionControlUpdate();
    }

    protected void ActionControlUpdate()
    {
        canAttemptSteal = !inPossession && !isSprinting && controllerBody.velocity.magnitude > 0.0f;
        if (inPossession || isSprinting) { canAttemptSteal = false; }
    }

    /// <summary>
    /// Calculates and initiates stamina recharge.
    /// </summary>
    protected void RechargeSprint()
    {
        if(stamina >= MAX_STAMINA) { stamina = MAX_STAMINA; return; }
        calculatedSprintRechargeTime = (stamina / MAX_STAMINA).Map(0.0f, MAX_STAMINA, 1.0f, 0.0f) * MAX_STAMINA_RECHARGE_TIME;
        sprintRechargeStartTime = Time.time;
        sprintRechargeStartValue = stamina;
        isRechargingSprint = true;
    }

    /// <summary>
    /// Calculates and intitiates sprinting with stamina drain.
    /// </summary>
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
        GUIStyle teamAStyle = new GUIStyle();
        teamAStyle.normal.textColor = Color.red;
        teamAStyle.alignment = TextAnchor.MiddleCenter;
        Handles.Label(transform.position + (Vector3.up * 0.2f), "Name: " + gameObject.name, teamAStyle);
        Handles.Label(transform.position, "Team: " + playerTeam.name, teamAStyle);
        Handles.Label(transform.position + (Vector3.down * 0.2f), "Stamina: " + stamina.ToString(), teamAStyle);

        //GUIStyle teamBStyle = new GUIStyle();
        //teamBStyle.normal.textColor = Color.red;
        //teamBStyle.alignment = TextAnchor.MiddleCenter;
    }
}
