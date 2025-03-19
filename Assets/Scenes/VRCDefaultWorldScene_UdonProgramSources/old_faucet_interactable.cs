using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Miner28.UdonUtils.Network;
using VRC.SDK3.Data;

public class old_faucet_interactable : NetworkInterface
{
    public int m_ID;

    public GameObject m_LiquidPool;
    public GameObject m_PouringLiquid;
    public Transform m_TapExit; // The point where the liquid exits the tap

    private float m_UpdateDelta = 0.0f;
    private const float UPDATE_INTERVAL = 1.0f / 10.0f;

    public AudioSource m_WineFlowSound;

    public float m_PoolSize;

    void Start()
    {
        // Ensure the liquid is initially inactive
        this.m_PouringLiquid.SetActive(false);
        this.m_LiquidPool.SetActive(false);

        this.m_PoolSize = 0.0f;
    }

    public override void Interact()
    {
        if (this.m_PouringLiquid.activeSelf)
        {
            this.RequestDeactivate();
        }
        else
        {
            this.RequestActivate();
        }
    }

    private float m_LastActivateRequestTime = 0.0f;
    private const float ACTIVATE_REQUEST_INTERVAL = 0.5f;
    public void RequestActivate()
    {
        if (Time.time - this.m_LastActivateRequestTime < ACTIVATE_REQUEST_INTERVAL) return;

        this.m_LastActivateRequestTime = Time.time;

        SendMethodNetworked(
            nameof(this.On_ActivateRequestReceived),
            SyncTarget.All,
            new DataToken(this.m_ID)
        );
    }

    private float m_LastDeactivateRequestTime = 0.0f;
    private const float DEACTIVATE_REQUEST_INTERVAL = 0.5f;
    public void RequestDeactivate()
    {
        if (Time.time - this.m_LastDeactivateRequestTime < DEACTIVATE_REQUEST_INTERVAL) return;

        this.m_LastDeactivateRequestTime = Time.time;

        SendMethodNetworked(
            nameof(this.On_DeactivateRequestReceived),
            SyncTarget.All,
            new DataToken(this.m_ID)
        );
    }


    private float m_LastActivateRequestReceivedTime = 0.0f;
    private const float ACTIVATE_REQUEST_RECEIVED_INTERVAL = 0.5f;
    [NetworkedMethod]
    public void On_ActivateRequestReceived(int id)
    {
        if (id != this.m_ID) return;
        if (Time.time - this.m_LastActivateRequestReceivedTime < ACTIVATE_REQUEST_RECEIVED_INTERVAL) return;

        this.m_LastActivateRequestReceivedTime = Time.time;

        this.Activate();
    }

    private float m_LastDeactivateRequestReceivedTime = 0.0f;
    private const float DEACTIVATE_REQUEST_RECEIVED_INTERVAL = 0.5f;
    [NetworkedMethod]
    public void On_DeactivateRequestReceived(int id)
    {
        if (id != this.m_ID) return;
        if (Time.time - this.m_LastDeactivateRequestReceivedTime < DEACTIVATE_REQUEST_RECEIVED_INTERVAL) return;

        this.m_LastDeactivateRequestReceivedTime = Time.time;

        this.Deactivate();
    }

    public void Activate()
    {
        this.m_PouringLiquid.SetActive(true);
        this.m_LiquidPool.SetActive(true);

        this.UpdateLiquid();
        this.PlayPouringSound();
    }

    public void Deactivate()
    {
        this.m_PouringLiquid.SetActive(false);
        this.StopPouringSound();
    }

    void Update()
    {
        this.m_UpdateDelta += Time.deltaTime;
        if (this.m_UpdateDelta <= UPDATE_INTERVAL) return;

        if (this.m_PouringLiquid.activeSelf)
        {
            this.UpdateLiquid();
        }
        else if (this.m_PoolSize > 0.0f)
        {
            this.SetLiquidPoolScale(this.m_PoolSize - 0.025f);
        }
        else if (this.m_LiquidPool.activeSelf)
        {
            this.m_LiquidPool.SetActive(false);
        }

        this.m_UpdateDelta = 0.0f;
    }

    private void UpdateLiquid()
    {
        // Perform a raycast downwards from the tap exit
        RaycastHit hit;
        if (Physics.Raycast(this.m_TapExit.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Default", "Pickup")))
        {
            // Calculate the distance from the tap exit to the hit point
            float distance = Vector3.Distance(this.m_TapExit.position, hit.point);

            // Update the scale of the liquid object
            Vector3 newScale = this.m_PouringLiquid.transform.localScale;
            newScale.y = distance / 2.0f; // Set the Y scale to the distance
            this.m_PouringLiquid.transform.localScale = newScale;

            // Update the position of the liquid object
            Vector3 newPosition = m_TapExit.position;
            newPosition.y -= distance / 2.0f; // Move the liquid down by half the distance
            this.m_PouringLiquid.transform.position = newPosition;

            bool colliderValid = hit.collider != null && hit.collider.gameObject != null;
            if (colliderValid && (hit.collider.gameObject.layer != LayerMask.NameToLayer("Pickup")))
            {
                if (this.m_PoolSize < 1.0f)
                {
                    this.SetLiquidPoolScale(this.m_PoolSize + 0.025f);
                }

                this.m_LiquidPool.transform.position = hit.point;
            }
            else
            {
                if(this.m_PoolSize > 0.0f)
                {
                    this.SetLiquidPoolScale(this.m_PoolSize - 0.025f);
                }
            }
        }
        else
        {
            // If the raycast doesn't hit anything, deactivate the liquid
            this.m_PouringLiquid.SetActive(false);
            this.StopPouringSound();
        }
    }

    public void PlayPouringSound()
    {
        if (this.m_WineFlowSound != null)
        {
            this.m_WineFlowSound.Play();
        }
    }

    public void StopPouringSound()
    {
        if (this.m_WineFlowSound != null)
        {
            this.m_WineFlowSound.Stop();
        }
    }

    public void SetLiquidPoolScale(float scale)
    {
        this.m_PoolSize = scale;
        this.m_LiquidPool.transform.localScale = new Vector3(
            this.m_PoolSize,
            this.m_PoolSize,
            this.m_PoolSize
        );
    }
}

