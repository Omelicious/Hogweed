using UnityEngine;

public class Scythe : Weapon
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        audioSource.generator = activeAudio;
        active = false;
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
            audioSource.Play();
        }
    }

    // Uses parent class' methods:
    //
    // CancelAttack
    // UpdateAnimator
    // SpawnAttackTrigger
}
