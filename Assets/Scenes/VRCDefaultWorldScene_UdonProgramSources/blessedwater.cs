
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class blessedwater : UdonSharpBehaviour
{
    public SkeletonHead m_SkeletonHead;
    public AudioSource m_WaterSplashAudioSource;
    void Start()
    {
        if (this.m_SkeletonHead == null)
        {
            Debug.LogError("[blessedwater.cs] Start: m_SkeletonHead is not set");
            return;
        }

        if (this.m_WaterSplashAudioSource == null)
        {
            Debug.LogError("[blessedwater.cs] Start: m_WaterSplashAudioSource is not set");
            return;
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other == null) return;
        if (other.gameObject == null) return;

        if (this.m_SkeletonHead == null)
        {
            Debug.LogError("[blessedwater.cs] OnTriggerEnter: m_SkeletonHead is not set");
            return;
        }

        if (this.m_SkeletonHead.gameObject == null)
        {
            Debug.LogError("[blessedwater.cs] OnTriggerEnter: m_SkeletonHead.gameObject is null");
            return;
        }

        if (other.gameObject != this.m_SkeletonHead.gameObject)
        {
            return;
        }

        if (this.m_WaterSplashAudioSource != null && !this.m_WaterSplashAudioSource.isPlaying)
        {
            this.m_WaterSplashAudioSource.Play();
        }

        this.m_SkeletonHead.Bless();
    }
}
