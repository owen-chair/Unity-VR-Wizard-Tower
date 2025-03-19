
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;
using Miner28.UdonUtils.Network;

public class magicbook : NetworkInterface
{
    public int m_ID = -1;

    public GameObject m_SpellEffect;
    public ParticleSystem m_SpellEffectParticleSystem;

    public VRC_Pickup m_Pickup;

    public VRCPlayerApi m_CastingPlayer;

    void Start()
    {
        if (this.m_SpellEffect == null)
        {
            Debug.LogError("[magicbook.cs] Start: m_SpellEffect is null");
            return;
        }

        if (this.m_SpellEffectParticleSystem == null)
        {
            Debug.LogError("[magicbook.cs] Start: m_SpellEffectParticleSystem is null");
            return;
        }

        if (this.m_Pickup == null)
        {
            Debug.LogError("[magicbook.cs] Start: m_Pickup is null");
            return;
        }

        this.m_SpellEffect.SetActive(false);
        this.m_SpellEffectParticleSystem.Stop();

        if (!Networking.LocalPlayer.IsUserInVR())
        {
            this.DisableInteractive = true;
            this.m_Pickup.pickupable = false;
        }
    }

    public override void OnPickup()
    {
        if (this.m_Pickup == null) { Debug.LogError("[magicbook.cs] OnPickup: m_Pickup is null"); return; }
        if (this.m_Pickup.currentPlayer == null) { Debug.LogError("[magicbook.cs] OnPickup: m_Pickup.currentPlayer is null"); return; }
        if (!this.m_Pickup.currentPlayer.IsValid()) { Debug.LogError("[magicbook.cs] OnPickup: m_Pickup.currentPlayer is not valid"); return; }
        if (!this.m_Pickup.currentPlayer.isLocal) { Debug.LogError("[magicbook.cs] OnPickup: m_Pickup.currentPlayer is not local"); return; }

        this.SendMethodNetworked(
            nameof(this.On_Received_SpellBookPickedUp),
            SyncTarget.All,
            new DataToken(Networking.LocalPlayer),
            new DataToken(this.m_ID)
        );
    }

    public override void OnDrop()
    {
        if (this.m_Pickup == null) { Debug.LogError("[magicbook.cs] OnDrop: m_Pickup is null"); return; }
        if (this.m_CastingPlayer == null) { Debug.LogError("[magicbook.cs] OnPickupUseDown: m_CastingPlayer.currentPlayer is null"); return; }
        if (!this.m_CastingPlayer.IsValid()) { Debug.LogError("[magicbook.cs] OnPickupUseDown: m_CastingPlayer.currentPlayer is not valid"); return; }
        if (!this.m_CastingPlayer.isLocal) { Debug.LogError("[magicbook.cs] OnPickupUseDown: m_CastingPlayer.currentPlayer not is local"); return; }

        this.SendMethodNetworked(
            nameof(this.On_Received_SpellBookDropped),
            SyncTarget.All,
            new DataToken(Networking.LocalPlayer),
            new DataToken(this.m_ID)
        );
    }

    public override void OnPickupUseDown()
    {
        if (this.m_Pickup == null) { Debug.LogError("[magicbook.cs] OnPickupUseDown: m_Pickup is null"); return; }
        if (this.m_Pickup.currentPlayer == null) { Debug.LogError("[magicbook.cs] OnPickupUseDown: m_Pickup.currentPlayer is null"); return; }
        if (!this.m_Pickup.currentPlayer.IsValid()) { Debug.LogError("[magicbook.cs] OnPickupUseDown: m_Pickup.currentPlayer is not valid"); return; }
        if (!this.m_Pickup.currentPlayer.isLocal) { Debug.LogError("[magicbook.cs] OnPickupUseDown: m_Pickup.currentPlayer not is local"); return; }

        this.SendMethodNetworked(
            nameof(this.On_Received_SpellBeginFire),
            SyncTarget.All,
            new DataToken(Networking.LocalPlayer),
            new DataToken(this.m_ID)
        );
    }

    public override void OnPickupUseUp()
    {
        if (this.m_Pickup == null) { Debug.LogError("[magicbook.cs] OnPickupUseUp: m_Pickup is null"); return; }
        if (this.m_Pickup.currentPlayer == null) { Debug.LogError("[magicbook.cs] OnPickupUseUp: m_Pickup.currentPlayer is null"); return; }
        if (!this.m_Pickup.currentPlayer.IsValid()) { Debug.LogError("[magicbook.cs] OnPickupUseUp: m_Pickup.currentPlayer is not valid"); return; }
        if (!this.m_Pickup.currentPlayer.isLocal) { Debug.LogError("[magicbook.cs] OnPickupUseUp: m_Pickup.currentPlayer is not local"); return; }

        this.SendMethodNetworked(
            nameof(this.On_Received_SpellEndFire),
            SyncTarget.All,
            new DataToken(Networking.LocalPlayer),
            new DataToken(this.m_ID)
        );
    }

    [NetworkedMethod]
    public void On_Received_SpellBookPickedUp(VRCPlayerApi castingPlayer, int bookId)
    {
        if (castingPlayer == null) return;
        if (!castingPlayer.IsValid()) return;
        if (bookId != this.m_ID) return;
        if (this.m_Pickup == null) return;

        this.m_CastingPlayer = castingPlayer;
        this.m_SpellEffect.SetActive(true);
        this.m_SpellEffectParticleSystem.Stop();
    }

    [NetworkedMethod]
    public void On_Received_SpellBookDropped(VRCPlayerApi castingPlayer, int bookId)
    {
        if (castingPlayer == null) return;
        if (!castingPlayer.IsValid()) return;
        if (bookId != this.m_ID) return;
        if (this.m_Pickup == null) return;

        this.m_CastingPlayer = null;
        this.m_SpellEffect.SetActive(false);
        this.m_SpellEffectParticleSystem.Stop();
    }

    [NetworkedMethod]
    public void On_Received_SpellBeginFire(VRCPlayerApi castingPlayer, int bookId)
    {
        if (castingPlayer == null) return;
        if (!castingPlayer.IsValid()) return;
        if (bookId != this.m_ID) return;
        if (this.m_Pickup == null) return;
        if (this.m_SpellEffectParticleSystem == null) return;

        this.m_SpellEffectParticleSystem.Play();
    }

    [NetworkedMethod]
    public void On_Received_SpellEndFire(VRCPlayerApi castingPlayer, int bookId)
    {
        if (castingPlayer == null) return;
        if (!castingPlayer.IsValid()) return;
        if (bookId != this.m_ID) return;
        if (this.m_Pickup == null) return;
        if (this.m_SpellEffectParticleSystem == null) return;

        this.m_SpellEffectParticleSystem.Stop();
    }

    /*Gets a struct called TrackingData, which contains separate Position and Rotation data. This is the suggested way to get position and rotation data for a Player's head and hands. This returns data from the TrackingManager for a Local Player (ie the data coming from their headset / trackers) and from the RightHand, LeftHand and Head bones for a Remote Player. Origin returns the center of the local VR user's playspace, while returning the player's position for local Desktop users and all remote users. AvatarRoot returns data for the root transform of the avatar (the same transform that the player capsule is attached to). For users in Fully-Body Tracking, AvatarRoot will not rotate with the head facing direction. If you need data reflecting the general forward facing direction of a Player, consider using GetRotation instead.*/

    private Quaternion rhPalmForwardRotationTransformation = Quaternion.Euler(90, 90, 0);
    private Quaternion lhPalmForwardRotationTransformation = Quaternion.Euler(-90, -90, 0);

    private Quaternion rhPalmForwardRemotePlayerOffset = Quaternion.Euler(0, 90, 0);
    private Quaternion lhPalmForwardRemotePlayerOffset = Quaternion.Euler(0, 90, 0);

    private void LateUpdate()
    {
        if (this.m_CastingPlayer == null) return;
        if (!this.m_CastingPlayer.IsValid()) return;
        if (this.m_SpellEffect == null) return;

        bool isVRUser = this.m_CastingPlayer.IsUserInVR();
        bool isLocalPlayer = this.m_CastingPlayer.isLocal;
        if (!isVRUser)
        {
            VRCPlayerApi.TrackingData headData = this.m_CastingPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

            this.m_SpellEffect.transform.position = headData.position;
            this.m_SpellEffect.transform.rotation = headData.rotation;

            return;
        }


        VRCPlayerApi.TrackingData lhData = this.m_CastingPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand);
        VRCPlayerApi.TrackingData rhData = this.m_CastingPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand);


        float lhBookDist = Vector3.Distance(lhData.position, this.transform.position);
        float rhBookDist = Vector3.Distance(rhData.position, this.transform.position);

        bool bookInLeftHand = lhBookDist < rhBookDist;
        bool bookInRightHand = rhBookDist < lhBookDist;

        if (bookInLeftHand)
        {
            this.m_SpellEffect.transform.position = rhData.position;
            this.m_SpellEffect.transform.rotation = isLocalPlayer ? rhData.rotation * rhPalmForwardRotationTransformation : rhData.rotation * rhPalmForwardRemotePlayerOffset;

            return;
        }


        if (bookInRightHand)
        {
            this.m_SpellEffect.transform.position = lhData.position;
            this.m_SpellEffect.transform.rotation = isLocalPlayer ? lhData.rotation * lhPalmForwardRotationTransformation : lhData.rotation * lhPalmForwardRemotePlayerOffset;

            return;
        }
    }
}
