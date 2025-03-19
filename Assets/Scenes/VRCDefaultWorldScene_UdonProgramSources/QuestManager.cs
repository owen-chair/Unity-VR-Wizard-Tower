
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class QuestManager : UdonSharpBehaviour
{
    public Transform m_TP_To_Jail_Teleporter_Cube;

    public GameObject m_WorldExterior;
    public GameObject m_GroundFloorInterior;
    public GameObject m_FirstFloorInterior;
    public GameObject m_SecondFloorInterior;
    public GameObject m_ThirdFloorInterior;
    public GameObject m_Bedroom1;
    public GameObject m_Bedroom2;
    public GameObject m_Bedroom3;
    public GameObject m_Bedroom4;
    public GameObject m_Bedroom5;
    public GameObject m_Jail;

    public Wizard_NPC m_DoorGuardWizard_NpcObject;
    public GameObject m_DoorGuardWizardGameObject;

    public GameObject m_JailWizard1GameObject;
    public Wizard_NPC m_JailWizard1_NpcObject;
    public GameObject m_JailExplosionParticleSystem;
    public GameObject m_JailBreakableWall;
    public GameObject m_JailWizardInvestigateNode;

    public AudioSource m_JailBreakableWallAudioSource;

    public GameObject m_JailSkeletonSpellBall;

    public AudioSource m_WhatTheAudioSource;
    public AudioSource m_OuchAudioSource;
    public AudioSource m_StopRightThereAudioSource;

    public GameObject m_FreePlayerNode;
    public GameObject m_FreePlayerTPNode;
    public AudioSource m_LuckyDayAudioSource;

    public GameObject m_JailEnterNode;

    public Transform FindVRCWorldObjectInScene()
    {
        GameObject respawnPoint = GameObject.Find("RespawnPoint");
        if (respawnPoint == null)
        {
            Debug.LogError("[UseTeleporter.cs] FindVRCWorldObjectInScene: VRCWorld not found in scene");
            return null;
        }

        return respawnPoint.transform;
    }

    private void TeleportTo(Transform transform)
    {
        if (transform == null)
        {
            Debug.LogError("[QuestManager.cs] TeleportTo: Transform is null");
            return;
        }

        Transform respawnPoint = this.FindVRCWorldObjectInScene();
        if (respawnPoint != null)
        {
            respawnPoint.position = transform.position;
        }

        if (Networking.LocalPlayer == null || !Networking.LocalPlayer.IsValid())
        {
            Debug.LogError("[QuestManager.cs] Interact: Networking.LocalPlayer is null / invalid");
            return;
        }

        Networking.LocalPlayer.TeleportTo(transform.position, transform.rotation);
    }

    public void OnTrigger(string token, float delay = 0.0f)
    {
        if (token == "TP_TO_JAIL")
        {
            SendCustomEventDelayedSeconds(nameof(this.TP_TO_JAIL), delay);
            return;
        }

        if (token == "WALL_SMASH")
        {
            SendCustomEventDelayedSeconds(nameof(this.WALL_SMASH), delay);
            return;
        }
    }

    public void TP_TO_JAIL()
    {
        if (this.m_ThirdFloorInterior != null)
        {
            this.m_ThirdFloorInterior.SetActive(false);
        }

        if (this.m_Jail != null)
        {
            this.m_Jail.SetActive(true);
        }

        if (this.m_JailWizard1GameObject != null)
        {
            this.m_JailWizard1GameObject.SetActive(true);
        }

        if (this.m_TP_To_Jail_Teleporter_Cube != null)
        {
            this.TeleportTo(this.m_TP_To_Jail_Teleporter_Cube);
        }
    }

    public void WALL_SMASH()
    {
        if (this.m_JailBreakableWall != null)
        {
            this.m_JailBreakableWall.SetActive(false);
        }

        if (this.m_JailBreakableWallAudioSource != null && !this.m_JailBreakableWallAudioSource.isPlaying)
        {
            this.m_JailBreakableWallAudioSource.Play();
        }

        if (this.m_JailExplosionParticleSystem != null)
        {
            this.m_JailExplosionParticleSystem.SetActive(true);
            SendCustomEventDelayedSeconds(nameof(this.DeactivateExpiredParticleSystemExplosionEffect), 3.0f);
        }

        if (this.m_JailWizard1GameObject != null && this.m_JailWizard1_NpcObject != null && this.m_JailWizardInvestigateNode != null)
        {
            this.m_JailWizard1_NpcObject.DisableInteractive = true;
            this.m_JailWizard1_NpcObject.SetTargetPosition(this.m_JailWizardInvestigateNode.transform.position);
        }

        if (this.m_WhatTheAudioSource != null && !this.m_WhatTheAudioSource.isPlaying)
        {
            this.m_WhatTheAudioSource.Play();
        }
    }

    public void DeactivateExpiredParticleSystemExplosionEffect()
    {
        if (this.m_JailExplosionParticleSystem != null)
        {
            this.m_JailExplosionParticleSystem.SetActive(false);
        }

        if (this.m_JailSkeletonSpellBall != null)
        {
            this.m_JailSkeletonSpellBall.SetActive(true);
        }
    }

    public void OnSpellBallExploded()
    {
        if (this.m_OuchAudioSource != null && !this.m_OuchAudioSource.isPlaying)
        {
            this.m_OuchAudioSource.Play();
        }

        SendCustomEventDelayedSeconds(nameof(this.StopRightThere), 1.0f);
    }

    public void StopRightThere()
    {
        if (this.m_StopRightThereAudioSource != null && !this.m_StopRightThereAudioSource.isPlaying)
        {
            this.m_StopRightThereAudioSource.Play();
        }

        float delay = this.m_StopRightThereAudioSource != null ? this.m_StopRightThereAudioSource.clip.length : 0.0f;
        SendCustomEventDelayedSeconds(nameof(this.FreePlayer), delay + 1.5f);
    }

    public void FreePlayer()
    {
        if (this.m_JailWizard1GameObject != null && this.m_JailWizard1_NpcObject != null && this.m_JailWizardInvestigateNode != null)
        {
            this.m_JailWizard1_NpcObject.SetTargetPosition(this.m_FreePlayerNode.transform.position);
        }

        if (this.m_FreePlayerTPNode != null)
        {
            this.TeleportTo(this.m_FreePlayerTPNode.transform);
        }

        if (this.m_LuckyDayAudioSource != null && !this.m_LuckyDayAudioSource.isPlaying)
        {
            this.m_LuckyDayAudioSource.Play();
        }

        float delay = this.m_LuckyDayAudioSource != null ? this.m_LuckyDayAudioSource.clip.length : 0.0f;
        SendCustomEventDelayedSeconds(nameof(this.JailWizardLeave), delay);
    }

    public void JailWizardLeave()
    {
        if (this.m_JailWizard1GameObject != null && this.m_JailWizard1_NpcObject != null && this.m_JailEnterNode != null)
        {
            this.m_JailWizard1_NpcObject.SetTargetPosition(this.m_JailEnterNode.transform.position);
            SendCustomEventDelayedSeconds(nameof(this.TestIfJailWizardReachedExit), 1.0f);
        }
    }

    private int m_MaxJailWizardLeaveTestAttempts = 15;
    private int m_CurrentJailWizardLeaveTestAttempts = 0;
    public void TestIfJailWizardReachedExit()
    {
        if (this.m_JailWizard1GameObject != null && this.m_JailWizard1_NpcObject != null && this.m_JailEnterNode != null)
        {
            float distance = Vector3.Distance(this.m_JailWizard1_NpcObject.transform.position, this.m_JailEnterNode.transform.position);
            if (distance < 1.5f || this.m_CurrentJailWizardLeaveTestAttempts >= this.m_MaxJailWizardLeaveTestAttempts)
            {
                this.m_JailWizard1GameObject.SetActive(false);
                SendCustomEventDelayedSeconds(nameof(this.CleanupJailQuestObjects), 1.0f);
            }
            else
            {
                SendCustomEventDelayedSeconds(nameof(this.TestIfJailWizardReachedExit), 1.0f);
            }
        }

        this.m_CurrentJailWizardLeaveTestAttempts++;
    }

    public void CleanupJailQuestObjects()
    {
        if (this.m_DoorGuardWizardGameObject != null)
        {
            Destroy(this.m_DoorGuardWizardGameObject);
        }

        if (this.m_JailWizard1GameObject != null)
        {
            Destroy(this.m_JailWizard1GameObject);
        }

        if (this.m_JailBreakableWall != null)
        {
            Destroy(this.m_JailBreakableWall);
        }

        if (this.m_JailExplosionParticleSystem != null)
        {
            Destroy(this.m_JailExplosionParticleSystem);
        }

        if (this.m_JailSkeletonSpellBall != null)
        {
            Destroy(this.m_JailSkeletonSpellBall);
        }
    }
}
