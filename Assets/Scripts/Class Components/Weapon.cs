using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform knifeTransform;
    [SerializeField] private GameObject attackEffect;

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
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
            attackInstance.transform.localScale /= 50 / PointsSystem.Instance.attackAreaCurrent; // Was too big when spawned on knives, so I shrinked it
            
            Destroy(attackInstance, 0.5f);
        }
    }

    public void UpdateAnimator()
    {
        animator.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedCurrent);
    }
}
