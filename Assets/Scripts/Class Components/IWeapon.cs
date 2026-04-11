using UnityEngine;
using UnityEngine.Audio;

public abstract class IWeapon : MonoBehaviour
{
    private bool active;

    [SerializeField] private Transform knifeTransform;
    [SerializeField] private GameObject attackEffect;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip activeAudio;

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start() // protected allows to reuse this code when overriding, buy accessing it through base.Start();
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        active = false;
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

    private virtual void SpawnAttackTrigger()
    {
        if (knifeTransform.Find("Attack Effect(Clone)") == false) // If there's no attacks present
        {
            GameObject attackInstance = Instantiate(attackEffect, knifeTransform.position, knifeTransform.rotation, knifeTransform);
            attackInstance.transform.localScale /= 50 / PointsSystem.Instance.attackAreaValue; // Was too big when spawned on knives, so I shrinked it
            
            Destroy(attackInstance, 0.5f);
        }
    }
}
