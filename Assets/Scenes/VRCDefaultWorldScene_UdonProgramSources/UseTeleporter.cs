
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

    void Start()
    {
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
        foreach (GameObject obj in m_DeactivateObjects)
        {
            if (obj == null)
            {
                Debug.LogError("[UseTeleporter.cs] Interact: Deactivate object is null");
                continue;
            }

            obj.SetActive(false);
        }

        foreach (GameObject obj in m_ActivateObjects)
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
    }
}
