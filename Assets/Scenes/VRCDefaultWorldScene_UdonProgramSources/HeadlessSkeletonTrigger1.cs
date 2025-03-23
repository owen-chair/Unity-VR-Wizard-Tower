
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HeadlessSkeletonTrigger1 : UdonSharpBehaviour
{
    public QuestManager m_QuestManager;

    public SkeletonHead m_SkeletonHead;

    void Start()
    {

    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == null) return;
        if (!player.isLocal) return;

        if (this.m_QuestManager == null)
        {
            Debug.LogError("[HeadlessSkeletonTrigger1.cs] OnPlayerTriggerEnter: m_QuestManager is not set");
            return;
        }

        if (this.m_SkeletonHead == null)
        {
            Debug.LogError("[HeadlessSkeletonTrigger1.cs] OnPlayerTriggerEnter: m_SkeletonHead is not set");
            return;
        }

        if (this.m_SkeletonHead.gameObject == null)
        {
            Debug.LogError("[HeadlessSkeletonTrigger1.cs] OnPlayerTriggerEnter: m_SkeletonHead.gameObject is null");
            return;
        }

        if (!this.m_SkeletonHead.m_IsBlessed)
        {
            return;
        }

        this.m_QuestManager.OnHeadlessSkeletonTriggered();

        if (this.gameObject == null)
        {
            Debug.LogError("[HeadlessSkeletonTrigger1.cs] OnPlayerTriggerEnter: this.gameObject is null");
            return;
        }

        this.gameObject.SetActive(false);
    }
}
