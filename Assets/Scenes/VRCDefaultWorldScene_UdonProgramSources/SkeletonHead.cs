
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SkeletonHead : UdonSharpBehaviour
{
    public GameObject m_CustomDialogue;
    public bool m_IsBlessed = false;

    void Start()
    {
        this.DisableInteractive = true;
    }

    public void Bless()
    {
        this.m_IsBlessed = true;
    }

    private float m_LastUpdate = 0.0f;
    private readonly float UPDATE_RATE = 1.0f / 30.0f;

    private void Update()
    {

        this.m_LastUpdate += Time.deltaTime;
        if (!(this.m_LastUpdate > UPDATE_RATE)) return;

        this.m_LastUpdate = 0.0f;

        if (this.m_CustomDialogue != null)
        {
            this.m_CustomDialogue.transform.position = this.transform.position;
        }
    }
}
