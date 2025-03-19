
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class candle_lod : UdonSharpBehaviour
{
    public GameObject m_LodFlame;
    public GameObject m_ParticleFlame;

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
        if (this.m_LodFlame == null)
        {
            Debug.LogError("candle_lod: m_LodFlame is not set");
        }

        if (this.m_ParticleFlame == null)
        {
            Debug.LogError("candle_lod: m_ParticleFlame is not set");
        }

        this.m_LodFlame.SetActive(true);
        this.m_ParticleFlame.SetActive(false);
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        base.OnPlayerTriggerEnter(player);

        this.m_LodFlame.SetActive(false);
        this.m_ParticleFlame.SetActive(true);
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        base.OnPlayerTriggerExit(player);

        this.m_LodFlame.SetActive(true);
        this.m_ParticleFlame.SetActive(false);
    }
}
