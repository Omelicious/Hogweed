using UnityEngine;
using UnityEngine.Audio;

public abstract class Weapon : MonoBehaviour
{
    protected bool active;

    [SerializeField] protected Transform knifeTransform;
    [SerializeField] protected GameObject attackEffect;

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip activeAudio;

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Attack (); // abstract forces to write daughter-specific code


    public virtual void CancelAttack() // virtual offers base behaviour
    {
        active = false;
        audioSource.Stop();
    }


    public virtual void UpdateAnimator()
    {
        animator.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedValue);
    }

    protected virtual void SpawnAttackTrigger()
    {
        if (knifeTransform.Find("Attack Effect(Clone)") == false) // If there's no attacks present
        {
            GameObject attackInstance = Instantiate(attackEffect, knifeTransform.position, knifeTransform.rotation, knifeTransform);
            attackInstance.transform.localScale /= 50 / PointsSystem.Instance.attackAreaValue; // Was too big when spawned on knives, so I shrinked it
            
            Destroy(attackInstance, 0.5f);
        }
    }
}
