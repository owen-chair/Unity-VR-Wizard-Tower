
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SkullReturnQuestTrigger : UdonSharpBehaviour
{
    public SkeletonHead m_SkeletonHead;
    public QuestManager m_QuestManager;

    void Start()
    {
        if (this.m_SkeletonHead == null)
        {
            Debug.LogError("[SkullReturnQuestTrigger.cs] OnTriggerEnter: m_SkeletonHead is not set");
            return;
        }

        if (this.m_QuestManager == null)
        {
            Debug.LogError("[SkullReturnQuestTrigger.cs] OnTriggerEnter: m_QuestManager is not set");
            return;
        }
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other == null) return;
        if (other.gameObject == null) return;

        if (this.m_SkeletonHead == null)
        {
            Debug.LogError("[SkullReturnQuestTrigger.cs] OnTriggerEnter: m_SkeletonHead is not set");
            return;
        }

        if (this.m_SkeletonHead.gameObject == null)
        {
            Debug.LogError("[SkullReturnQuestTrigger.cs] OnTriggerEnter: m_SkeletonHead.gameObject is null");
            return;
        }

        if (this.m_QuestManager == null)
        {
            Debug.LogError("[SkullReturnQuestTrigger.cs] OnTriggerEnter: m_QuestManager is not set");
            return;
        }

        if (other.gameObject == this.m_SkeletonHead.gameObject)
        {
            if (this.m_SkeletonHead.m_IsBlessed)
            {
                this.m_QuestManager.OnSkullReturned();
            }
        }
    }
}
