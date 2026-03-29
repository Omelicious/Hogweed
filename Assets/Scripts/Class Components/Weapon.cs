using UnityEngine;
using UnityEngine.Audio;

public class Weapon : MonoBehaviour
{
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

        // audioSource.generator = activeAudio; // OR I can do 2 sources and change their volumes?
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack ()
    {
        if (knifeTransform.Find("Attack Effect(Clone)") == false) // If there's no attacks present
        {
            GameObject attackInstance = Instantiate(attackEffect, knifeTransform.position, knifeTransform.rotation, knifeTransform);
            attackInstance.transform.localScale /= 50 / PointsSystem.Instance.attackAreaValue; // Was too big when spawned on knives, so I shrinked it
            
            
            Destroy(attackInstance, 0.5f);
        }
    }

    public void UpdateAnimator()
    {
        animator.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedValue);
    }
}
