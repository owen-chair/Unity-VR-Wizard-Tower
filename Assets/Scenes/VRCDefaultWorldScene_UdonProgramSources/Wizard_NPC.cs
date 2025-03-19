using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Data;
using VRC.Udon;
using Miner28.UdonUtils.Network;
using TMPro;
using UnityEngine.UI;

public class Wizard_NPC : NetworkInterface
{
    public int m_ID = -1;

    public SkinnedMeshRenderer m_SkinnedMeshRenderer;
    public SkinnedMeshRenderer m_SkinnedMeshRenderer_Head;

    public GameObject m_Bounds_Min;
    public GameObject m_Bounds_Max;

    public Animator m_Animator; // Reference to the Animator component

    public Transform m_ArmaturePositionRotationTransform; //name = MasterControl
    public Transform m_HeadTransform;
    public readonly Quaternion m_HeadTransformRotationOffset = Quaternion.Euler(0, 270, 270);

    private Vector3 m_TargetPosition;
    private float m_TargetStopDistance = 0.1f;
    private bool m_IsMoving = false;
    private bool m_IsStopping = false;
    private float m_LastRandomMoveTime = 0.0f;

    private float currentWalkSpeed = 0.0f;
    public float walkSpeed;
    public float moveSpeed;
    public float rotationSpeed;

    private Ray m_MovementRay;
    private RaycastHit m_MovementRayHit;
    LayerMask m_LayerMask;

    public Canvas m_DialogueCanvas;
    public GameObject m_CustomDialogue;

    void Start()
    {
        this.m_LastRandomMoveTime = Time.time;
        this.m_TargetPosition = this.transform.position;

        this.m_LayerMask = LayerMask.GetMask("Environment", "Default");
        this.m_MovementRay = new Ray();

        this.m_IsMoving = false;
        this.m_IsStopping = false;

        currentWalkSpeed = 0.0f;
        if (this.m_Animator != null)
        {
            this.m_Animator.SetFloat("WalkSpeed", currentWalkSpeed);
        }
    }


    private float m_LastUpdate = 0.0f;
    private readonly float UPDATE_RATE = 1.0f / 30.0f;
    void Update()
    {
        this.m_LastUpdate += Time.deltaTime;
        if (!(this.m_LastUpdate > UPDATE_RATE)) return;


        if (this.m_IsMoving && !this.m_IsConversing)
        {
            this.HandleMove(this.m_LastUpdate);
            this.HandleRotation(this.m_LastUpdate);
        }
        else if (this.m_IsConversing)
        {
            this.LookAtLocalPlayer(this.m_LastUpdate);
        }

        this.UpdateWalkSpeed(this.m_LastUpdate);
        this.m_LastUpdate = 0.0f;

        if (!Networking.IsMaster) return;
        if (this.m_Bounds_Min == null) return;
        if (this.m_Bounds_Max == null) return;

        if (!this.m_IsMoving && Time.time - this.m_LastRandomMoveTime > 10.0f && !this.m_IsConversing)
        {
            this.PickRandomTargetPosition();
        }
    }

    public void PickRandomTargetPosition()
    {
        if (!Networking.IsMaster) return;
        if (this.m_Bounds_Min == null) return;
        if (this.m_Bounds_Max == null) return;

        Vector3 min = this.m_Bounds_Min.transform.position;
        Vector3 max = this.m_Bounds_Max.transform.position;

        float randomX = Random.Range(min.x, max.x);
        float randomZ = Random.Range(min.z, max.z);

        Vector3 newTargetPosition = new Vector3(randomX, this.m_Bounds_Max.transform.position.y, randomZ);
        if (Vector3.Distance(newTargetPosition, this.m_TargetPosition) < 2.0f) return;
        this.m_LastRandomMoveTime = Time.time;

        SendMethodNetworked(
            nameof(this.Notify_SetTargetPosition),
            SyncTarget.All,
            new DataToken(Networking.LocalPlayer),
            new DataToken(newTargetPosition),
            new DataToken(this.m_ID)
        );
    }

    [NetworkedMethod]
    public void Notify_SetTargetPosition(VRCPlayerApi requestingPlayer, Vector3 targetPosition, int id)
    {
        if (requestingPlayer == null) return;
        if (!requestingPlayer.IsValid()) return;
        if (!requestingPlayer.isMaster) return;
        if (id != this.m_ID) return;

        this.SetTargetPosition(targetPosition);
    }

    public void SetTargetPosition(Vector3 targetPosition, float targetStopDistance = 0.2f)
    {
        if (Vector3.Distance(targetPosition, this.m_TargetPosition) > 15.0f) return;

        this.m_TargetPosition = targetPosition;
        this.m_TargetStopDistance = targetStopDistance;
        this.m_IsMoving = true;
        this.m_IsStopping = false;
    }

    public float GetHeight()
    {
        this.m_MovementRay.origin = this.transform.position + (Vector3.up * 1.0f);
        this.m_MovementRay.direction = Vector3.down;

        if (Physics.Raycast(this.m_MovementRay, out this.m_MovementRayHit, Mathf.Infinity, this.m_LayerMask))
        {
            return this.m_MovementRayHit.point.y;
        }

        return 1.0f;
    }

    private void HandleMove(float delta)
    {
        Vector3 currentPos = this.transform.position;
        float distanceToTarget = Vector3.Distance(currentPos, this.m_TargetPosition);

        if (distanceToTarget < this.m_TargetStopDistance + 0.15f)
        {
            if (!this.m_IsStopping)
            {
                this.m_IsStopping = true;
            }
        }
        if (distanceToTarget < this.m_TargetStopDistance)
        {
            this.m_IsMoving = false;

           // this.On_WalkTargetReached() 

            return;
        }
        //else
        //{
            //Debug.Log("[Wizard_NPC.cs][HandleMove] distanceToTarget: " + distanceToTarget);
        //}

        float y = GetHeight();
        Vector3 direction = (this.m_TargetPosition - currentPos).normalized;
        Vector3 newPos = currentPos + (direction * moveSpeed) * delta;

        newPos.y = y;

        this.transform.position = newPos;
        bool customDialogueTransformValid = this.m_CustomDialogue != null && this.m_CustomDialogue.transform != null;
        bool headTransformValid = this.m_HeadTransform != null;
        if (customDialogueTransformValid && headTransformValid)
        {
            this.m_CustomDialogue.transform.position = this.m_HeadTransform.position;
        }
    }

    private void HandleRotation(float delta)
    {
        if (!this.m_IsMoving) return;
        if (this.m_IsStopping) return;

        Vector3 lookDirection = (this.m_TargetPosition - transform.position).normalized;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            // Apply an additional rotation offset to correct the orientation
            Quaternion rotationOffset = Quaternion.Euler(0, 0, 0); // Adjust this based on your model's initial orientation
            targetRotation *= rotationOffset;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * delta
            );
        }
    }

    private void LookAtLocalPlayer(float delta)
    {
        if (Networking.LocalPlayer == null) return;
        if (!Networking.LocalPlayer.IsValid()) return;

        Vector3 lookDirection = (Networking.LocalPlayer.GetPosition() - transform.position).normalized;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            // Apply an additional rotation offset to correct the orientation
            Quaternion rotationOffset = Quaternion.Euler(0, 0, 0); // Adjust this based on your model's initial orientation
            targetRotation *= rotationOffset;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * delta
            );
        }
    }

    private float lastWalkSpeed = 0.0f;
    private void UpdateWalkSpeed(float delta)
    {
        float targetSpeed = (this.m_IsMoving && !this.m_IsStopping) ? walkSpeed : 0.0f;
        float t = (this.m_IsStopping ? 15.0f : 20.0f) * delta;

        currentWalkSpeed = Mathf.Lerp(currentWalkSpeed, targetSpeed, t);
        currentWalkSpeed = Mathf.Clamp(currentWalkSpeed, 0.0f, 1.0f);
        if (this.m_Animator != null)
        {
            if (lastWalkSpeed != currentWalkSpeed)
            {
                //Debug.Log("[Wizard_NPC.cs][UpdateWalkSpeed] currentWalkSpeed: " + currentWalkSpeed);
                this.m_Animator.SetFloat("WalkSpeed", currentWalkSpeed);
            }

            lastWalkSpeed = currentWalkSpeed;
        }
    }


    public DialogueManager dialogueManager;  // Assign in the Inspector
    public bool m_IsConversing = false;

    public override void Interact()
    {
        if (dialogueManager != null && !this.m_IsConversing)
        {
            dialogueManager.StartDialogue();
            this.m_IsConversing = true;
            this.SetTargetPosition(this.transform.position);
            this.m_IsMoving = false;
            this.m_IsStopping = true;
            this.DisableInteractive = true;
        }
        else
        {
            Debug.LogError("DialogueManager is not assigned.");
        }
    }

    public void OnDialogueEnded()
    {
        this.m_IsConversing = false;
        this.DisableInteractive = false;
    }
}
