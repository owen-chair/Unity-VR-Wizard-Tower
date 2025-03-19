
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SkeletonSpellBall : UdonSharpBehaviour
{
    private float m_TimeCast = 0.0f;
    private bool m_Instantiated = false;

    public AudioSource m_FlyingAudioSource;
    public AudioSource m_HitAudioSourceSpatial;

    private bool m_Exploded = false;
    private bool m_SpellTimedOut = false;

    public readonly float m_MoveSpeed = 11.5f; // Units per second
    private float m_RotationAngle = 0f;
    public readonly float m_RotationSpeed = 720f; // Degrees per second (2 full rotations)

    public readonly float m_SpellTimeout = 1.0f;

    public GameObject m_Target;

    public QuestManager m_QuestManager;

    void Start()
    {
        if (this.m_Target == null) return;

        this.m_TimeCast = 0.0f;
        this.m_Instantiated = true;
        this.m_Exploded = false;

        if (this.m_FlyingAudioSource != null && !this.m_FlyingAudioSource.isPlaying)
        {
            this.m_FlyingAudioSource.Play();
        }
    }

    void Update()
    {
        if (!this.m_Instantiated) return;

        if (this.m_Target == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if (this.m_Exploded || this.m_SpellTimedOut)
        {
            return;
        }

        this.m_TimeCast += Time.deltaTime;
        this.m_SpellTimedOut = this.m_TimeCast > this.m_SpellTimeout;

        if (!m_SpellTimedOut)
        {
            Vector3 targetsFace = this.m_Target.transform.position + new Vector3(0, 1f, 0);
            Vector3 direction = (targetsFace - this.transform.position).normalized;

            // Spinning rotation around forward axis
            this.m_RotationAngle += this.m_RotationSpeed * Time.deltaTime;
            Quaternion newRot = Quaternion.Euler(0f, 0f, this.m_RotationAngle);

            // Constant movement speed
            Vector3 newPos = this.transform.position + direction * this.m_MoveSpeed * Time.deltaTime;

            this.transform.SetPositionAndRotation(newPos, newRot);

            if (Vector3.Distance(this.transform.position, targetsFace) < 0.5f)
            {
                this.On_Explode(true);
            }
        }
        else if (this.m_SpellTimedOut)
        {
            this.On_Explode(false);
        }
    }

    public void On_Explode(bool didHit)
    {
        if (this.m_Target == null) return;
        if (this.m_Exploded) return;
        if (this.m_FlyingAudioSource == null) return;
        if (this.m_HitAudioSourceSpatial == null) return;

        if (this.m_FlyingAudioSource != null && this.m_FlyingAudioSource.isPlaying)
        {
            this.m_FlyingAudioSource.Stop();
        }

        if (didHit)
        {
            if (this.m_HitAudioSourceSpatial != null && !this.m_HitAudioSourceSpatial.isPlaying)
            {
                this.m_HitAudioSourceSpatial.Play();
            }

           // this.OnHit();
        }

        this.m_Exploded = true;

        this.gameObject.SetActive(false);

        if (this.m_QuestManager != null)
        {
            this.m_QuestManager.OnSpellBallExploded();
        }
    }
}