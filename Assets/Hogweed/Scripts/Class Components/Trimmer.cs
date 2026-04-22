using UnityEngine;

public class Trimmer : Weapon
{
    [SerializeField] private AudioClip idleAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        active = false;
        audioSource.generator = idleAudio;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack ()
    {
        SpawnAttackTrigger();

        if(!active)
        {
            active = true;
            audioSource.generator = activeAudio;
            audioSource.Play();
        }
    }

    public override void CancelAttack()
    {
        active = false;
        audioSource.generator = idleAudio;
        audioSource.Play();
    }

    // Uses parent class' methods:
    //
    // UpdateAnimator
    // SpawnAttackTrigger
}
