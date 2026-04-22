using UnityEngine;

public class BuyMenu : MonoBehaviour
{
    /*FIELDS*/
    [SerializeField] private Animator charaAnim;
    [SerializeField] private Animator scytheAnim;
    [SerializeField] private Animator trimmerAnim;
        // UI
    [SerializeField] private TMPro.TextMeshProUGUI pointsText;
        // Value
    [SerializeField] private TMPro.TextMeshProUGUI movementSpeedValueText;
    [SerializeField] private TMPro.TextMeshProUGUI attackSpeedValueText;
    [SerializeField] private TMPro.TextMeshProUGUI attackAreaValueText;
        // Increase value
    [SerializeField] private TMPro.TextMeshProUGUI movementSpeedIncrementText;
    [SerializeField] private TMPro.TextMeshProUGUI attackSpeedIncrementText;
    [SerializeField] private TMPro.TextMeshProUGUI attackAreaIncrementText;
        // Price
    [SerializeField] private TMPro.TextMeshProUGUI movementSpeedPriceText;
    [SerializeField] private TMPro.TextMeshProUGUI attackSpeedPriceText;
    [SerializeField] private TMPro.TextMeshProUGUI attackAreaPriceText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyWalkSpeed()
    {
        PointsSystem.Instance.BuyWalkSpeed();

        UpdateAnimators();
        SetBuyMenuText();
    }

    public void BuyAttackSpeed()
    {
        PointsSystem.Instance.BuyAttackSpeed();

        UpdateAnimators();
        SetBuyMenuText();
    }

    public void BuyAttackArea()
    {
        PointsSystem.Instance.BuyAttackArea();

        SetBuyMenuText();
    }

    private void UpdateAnimators()
    {
        charaAnim.SetFloat("MoveSpeed", PointsSystem.Instance.movementSpeedValue);
        charaAnim.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedValue);
        scytheAnim.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedValue);
        trimmerAnim.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedValue);
    }
    
    private void SetBuyMenuText()
    {
        pointsText.text = "Очки: " + PointsSystem.Instance.pointsTotal;

        // Movement Speed
        movementSpeedValueText.text = "" + PointsSystem.Instance.movementSpeedValue;
        movementSpeedIncrementText.text = "+ " + PointsSystem.Instance.movementSpeedIncrement;
        movementSpeedPriceText.text = PointsSystem.Instance.movementSpeedPrice + " очков";

        if (PointsSystem.Instance.movementSpeedValue >= PointsSystem.MOVEMENT_SPEED_MAX)
        {
            movementSpeedValueText.text = "УСЁ";
            movementSpeedIncrementText.text = "";
            movementSpeedPriceText.text = "";
        }

        // Attack Speed
        attackSpeedValueText.text = "" + PointsSystem.Instance.attackSpeedValue;
        attackSpeedPriceText.text = PointsSystem.Instance.attackSpeedPrice + " очков";
        attackSpeedIncrementText.text = "+ " + PointsSystem.Instance.attackSpeedIncrement;

        if (PointsSystem.Instance.attackSpeedValue >= PointsSystem.ATTACK_SPEED_MAX)
        {
            attackSpeedValueText.text = "УСЁ";
            attackSpeedPriceText.text = "";
            attackSpeedIncrementText.text = "";
        }

        // Attack Area
        attackAreaValueText.text = "" + PointsSystem.Instance.attackAreaValue;
        attackAreaIncrementText.text = "+ " + PointsSystem.Instance.attackAreaIncrement;
        attackAreaPriceText.text = PointsSystem.Instance.attackAreaPrice + " очков";

        if (PointsSystem.Instance.attackAreaValue >= PointsSystem.ATTACK_AREA_MAX)
        {
            attackAreaValueText.text = "УСЁ";
            attackAreaIncrementText.text = "";
            attackAreaPriceText.text = "";
        }
    }
}
