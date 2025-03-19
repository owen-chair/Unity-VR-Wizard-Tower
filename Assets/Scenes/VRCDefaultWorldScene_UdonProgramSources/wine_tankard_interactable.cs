using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Miner28.UdonUtils.Network;
using VRC.SDK3.Components;
using VRC.SDK3.Data;

public class wine_tankard_interactable : NetworkInterface
{
    public int m_ID;

    private float m_FillLevel = 0.0f;
    private float m_SipAmount = 0.1f;

    public GameObject m_Liquid;
    public Transform m_BottomOfCup;
    public Transform m_TopOfCup;

    public AudioSource m_SwallowSound;
    public AudioSource m_PouringSound;

    public VRCPickup m_Pickup;

    void Start()
    {
        this.m_FillLevel = 0.0f;
        this.UpdateLiquidLevel();
    }

    private float m_LastRefillRequestTime = 0.0f;
    private const float REFILL_REQUEST_INTERVAL = 0.5f;
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other == null) return;
        if (other.gameObject == null) return;
        if (this.m_Pickup == null) return;
        if (!this.m_Pickup.IsHeld) return;
        if (this.m_Pickup.currentPlayer == null) return;
        if (!this.m_Pickup.currentPlayer.isLocal) return;
        if (this.m_FillLevel >= 1.0f) return;
        if (Time.time - this.m_LastRefillRequestTime < REFILL_REQUEST_INTERVAL) return;
        if (other.gameObject.name != "wine liquid") return;

        this.m_LastRefillRequestTime = Time.time;

        SendMethodNetworked(
            nameof(this.On_RefillRequestReceived),
            SyncTarget.All,
            new DataToken(this.m_ID)
        );
    }

    private float m_LastRefillRequestReceivedTime = 0.0f;
    private const float REFILL_REQUEST_RECEIVED_INTERVAL = 0.5f;
    [NetworkedMethod]
    public void On_RefillRequestReceived(int id)
    {
        if (id != this.m_ID) return;
        if (Time.time - this.m_LastRefillRequestReceivedTime < REFILL_REQUEST_RECEIVED_INTERVAL) return;

        this.m_LastRefillRequestReceivedTime = Time.time;

        this.m_FillLevel = 1.0f;
        this.UpdateLiquidLevel();

        this.PlayPouringSound();

        this.m_Liquid.SetActive(true);
    }

    private float m_LastPourRequestTime = 0.0f;
    private const float POUR_REQUEST_INTERVAL = 0.5f;
    public override void OnPickupUseDown()
    {
        base.OnPickupUseDown();

        if (this.m_Pickup == null) return;
        if (!this.m_Pickup.IsHeld) return;
        if (this.m_Pickup.currentPlayer == null) return;
        if (!this.m_Pickup.currentPlayer.isLocal) return;
        if (this.m_FillLevel <= 0.0f) return;
        if (Time.time - this.m_LastPourRequestTime < POUR_REQUEST_INTERVAL) return;

        this.m_LastPourRequestTime = Time.time;

        float currentFillLevel = this.m_FillLevel;
        float nextFillLevel = currentFillLevel - this.m_SipAmount;

        SendMethodNetworked(
            nameof(this.On_DrinkRequestReceived),
            SyncTarget.All,
            new DataToken(this.m_ID),
            new DataToken(nextFillLevel)
        );
    }

    [NetworkedMethod]
    public void On_DrinkRequestReceived(int id, float nextFillLevel)
    {
        if (id != this.m_ID) return;
        if (nextFillLevel < -0.5f) return;
        if (nextFillLevel > 1.1f) return;
        if (this.m_FillLevel <= 0.0f) return;

        this.m_FillLevel = nextFillLevel;
        this.UpdateLiquidLevel();
        this.PlaySwallowSound();
    }

    public void UpdateLiquidLevel()
    {
        float newY = Mathf.Lerp(this.m_BottomOfCup.localPosition.y, this.m_TopOfCup.localPosition.y, this.m_FillLevel);

        this.m_Liquid.transform.localPosition = new Vector3(
            this.m_Liquid.transform.localPosition.x,
            newY,
            this.m_Liquid.transform.localPosition.z
        );

        if (this.m_FillLevel <= 0.0f)
        {
            this.m_Liquid.SetActive(false);
            if (this.m_Pickup != null)
            {
                this.m_Pickup.InteractionText = "Empty Tankard";
            }
        }
        else
        {
            if (this.m_Pickup != null)
            {
                this.m_Pickup.InteractionText = "Wine";
            }
        }
    }

    private void PlaySwallowSound()
    {
        if (this.m_SwallowSound != null && !this.m_SwallowSound.isPlaying)
        {
            this.m_SwallowSound.Play();
        }
    }

    private void PlayPouringSound()
    {
        if (this.m_PouringSound != null && !this.m_PouringSound.isPlaying)
        {
            this.m_PouringSound.Play();
        }
    }
}
