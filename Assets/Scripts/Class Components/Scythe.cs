using UnityEngine;

public class Scythe : Weapon
{
    private bool active;

    [SerializeField] private Transform knifeTransform;
    [SerializeField] private GameObject attackEffect;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip idleAudio;
    [SerializeField] private AudioClip activeAudio;

    public Animator animator;

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

    public void Attack ()
    {
        SpawnAttackTrigger();

        if(!active)
        {
            active = true;
            audioSource.generator = activeAudio;
            audioSource.Play();
        }
    }

    public void CancelAttack()
    {
        active = false;
        audioSource.generator = idleAudio;
        audioSource.Play();
    }


    public void UpdateAnimator()
    {
        animator.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedValue);
    }

    private void SpawnAttackTrigger()
    {
        if (knifeTransform.Find("Attack Effect(Clone)") == false) // If there's no attacks present
        {
            GameObject attackInstance = Instantiate(attackEffect, knifeTransform.position, knifeTransform.rotation, knifeTransform);
            attackInstance.transform.localScale /= 50 / PointsSystem.Instance.attackAreaValue; // Was too big when spawned on knives, so I shrinked it
            
            Destroy(attackInstance, 0.5f);
        }
    }
}
