
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class UseTeleporter : UdonSharpBehaviour
{
    public GameObject m_TargetNode;

    public GameObject[] m_DeactivateObjects;
    public GameObject[] m_ActivateObjects;

    public QuestManager m_QuestManager;

    void Start()
    {
        if (this.m_QuestManager == null)
        {
            Debug.LogError("[UseTeleporter.cs] Start: QuestManager is not set");
            return;
        }

        if (this.m_TargetNode == null)
        {
            Debug.LogError("[UseTeleporter.cs] Start: Target node is not set");
            return;
        }

        if (this.m_DeactivateObjects == null)
        {
            Debug.LogError("[UseTeleporter.cs] Start: Deactivate objects is not set");
            return;
        }

        if (this.m_ActivateObjects == null)
        {
            Debug.LogError("[UseTeleporter.cs] Start: Activate objects is not set");
            return;
        }

        if (this.m_DeactivateObjects.Length == 0)
        {
            Debug.LogError("[UseTeleporter.cs] Start: Deactivate objects is empty");
        }
        else
        {
            foreach (GameObject obj in this.m_DeactivateObjects)
            {
                if (obj == null)
                {
                    Debug.LogError("[UseTeleporter.cs] Start: Deactivate object is null");
                    continue;
                }
            }
        }

        if (this.m_ActivateObjects.Length == 0)
        {
            Debug.LogError("[UseTeleporter.cs] Start: Activate objects is empty");
        }
        else
        {
            foreach (GameObject obj in this.m_ActivateObjects)
            {
                if (obj == null)
                {
                    Debug.LogError("[UseTeleporter.cs] Start: Deactivate object is null");
                    continue;
                }
            }
        }
    }

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

    public override void Interact()
    {
        foreach (GameObject obj in this.m_DeactivateObjects)
        {
            if (obj == null)
            {
                Debug.LogError("[UseTeleporter.cs] Interact: Deactivate object is null");
                continue;
            }

            obj.SetActive(false);
        }

        foreach (GameObject obj in this.m_ActivateObjects)
        {
            if (obj == null)
            {
                Debug.LogError("[UseTeleporter.cs] Interact: Deactivate object is null");
                continue;
            }

            obj.SetActive(true);
        }

        Transform respawnPoint = this.FindVRCWorldObjectInScene();
        if (respawnPoint != null)
        {
            respawnPoint.position = this.m_TargetNode.transform.position;
        }

        if (Networking.LocalPlayer == null || !Networking.LocalPlayer.IsValid())
        {
            Debug.LogError("[UseTeleporter.cs] Interact: Networking.LocalPlayer is null / invalid");
            return;
        }

        if (this.m_TargetNode == null)
        {
            Debug.LogError("[UseTeleporter.cs] Interact: Target node is null");
            return;
        }

        Networking.LocalPlayer.TeleportTo(
            this.m_TargetNode.transform.position,
            this.m_TargetNode.transform.rotation
        );

        if (this.m_QuestManager != null)
        {
            this.m_QuestManager.OnTransition(this.m_ActivateObjects[0]);
        }
    }
}
