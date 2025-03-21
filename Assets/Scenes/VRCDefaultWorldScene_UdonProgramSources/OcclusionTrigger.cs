
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class OcclusionTrigger : UdonSharpBehaviour
{
    public GameObject[] m_Occludees;

    void Start()
    {
        this.SetDefaultState();
    }

    void OnEnable()
    {
        this.SetDefaultState();
    }

    private void SetDefaultState()
    {
        if (this.m_Occludees == null)
        {
            Debug.LogError("KitchenClutterOcclusionTrigger: m_Occludees is not set");
            return;
        }

        this.DeactivateOccludees();
    }

    public void ActivateOccludees()
    {
        if (this.m_Occludees == null)
        {
            Debug.LogError("KitchenClutterOcclusionTrigger: m_Occludees is not set");
            return;
        }

        foreach (GameObject occludee in m_Occludees)
        {
            if (occludee != null)
            {
                occludee.SetActive(true);
            }
        }
    }

    public void DeactivateOccludees()
    {
        if (this.m_Occludees == null)
        {
            Debug.LogError("KitchenClutterOcclusionTrigger: m_Occludees is not set");
            return;
        }

        foreach (GameObject occludee in m_Occludees)
        {
            if (occludee != null)
            {
                occludee.SetActive(false);
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == null) return;
        if (!player.isLocal) return;

        base.OnPlayerTriggerEnter(player);
        this.ActivateOccludees();
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player == null) return;
        if (!player.isLocal) return;

        base.OnPlayerTriggerExit(player);
        this.DeactivateOccludees();
    }
}
