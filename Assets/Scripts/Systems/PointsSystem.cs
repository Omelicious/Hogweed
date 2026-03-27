using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PointsSystem : MonoBehaviour
{
    public static PointsSystem Instance;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {

        Instance = this;
        
        LoadPlayerPrefs();

        SetBuyMenuText();
    }

    [SerializeField] private Animator charaAnim;
    [SerializeField] private Animator scytheAnim;
    [SerializeField] private Animator trimmerAnim;

    [SerializeField] private int pointsTotal;
    [SerializeField] private const int MOVEMENT_SPEED_MAX = 10;
    [SerializeField] private const int ATTACK_SPEED_MAX = 5;
    [SerializeField] private const int ATTACK_AREA_MAX = 10;

    [SerializeField] private int movementSpeedIncreaseValue = 1;
    [SerializeField] private int attackSpeedIncreaseValue = 1;
    [SerializeField] private int attackAreaIncreaseValue = 1;

    // stays here
    [SerializeField] private int movementSpeedIncreasePrice = 10;
    [SerializeField] private int attackSpeedIncreasePrice = 10;
    [SerializeField] private int attackAreaIncreasePrice = 10;
    
    // goes outside
    [SerializeField] public int movementSpeedCurrent = 1;
    [SerializeField] public int attackSpeedCurrent = 1;
    [SerializeField] public int attackAreaCurrent = 1;


    // UI
    [SerializeField] private TMPro.TextMeshProUGUI buyWindowPointsText;
        // Current
    [SerializeField] private TMPro.TextMeshProUGUI movementSpeedCurrentText;
    [SerializeField] private TMPro.TextMeshProUGUI attackSpeedCurrentText;
    [SerializeField] private TMPro.TextMeshProUGUI attackAreaCurrentText;
        // Increase value
    [SerializeField] private TMPro.TextMeshProUGUI movementSpeedIncreaseValueText;
    [SerializeField] private TMPro.TextMeshProUGUI attackSpeedIncreaseValueText;
    [SerializeField] private TMPro.TextMeshProUGUI attackAreaIncreaseValueText;
        // Price
    [SerializeField] private TMPro.TextMeshProUGUI movementSpeedIncreasePriceText;
    [SerializeField] private TMPro.TextMeshProUGUI attackSpeedIncreasePriceText;
    [SerializeField] private TMPro.TextMeshProUGUI attackAreaIncreasePriceText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buyWindowPointsText.text = "Очки: " + pointsTotal;
        //Debug.Log("Movement speed is " + movementSpeedCurrent);
    }

    public void BuyWalkSpeed()
    {
        if (movementSpeedCurrent >= MOVEMENT_SPEED_MAX)
            return;
        
        if(pointsTotal >= movementSpeedIncreasePrice)
        {
            pointsTotal -= movementSpeedIncreasePrice;
            movementSpeedCurrent += movementSpeedIncreaseValue;
            movementSpeedIncreasePrice += movementSpeedIncreasePrice;

            movementSpeedCurrentText.text = "" + movementSpeedCurrent;
            movementSpeedIncreasePriceText.text = movementSpeedIncreasePrice + " очков";
        }
        
        UpdatePlayerPrefs();
        SetBuyMenuText();
    }

    public void BuyAttackSpeed()
    {
        if (attackSpeedCurrent >= ATTACK_SPEED_MAX)
            return;
        
        if(pointsTotal >= attackSpeedIncreasePrice)
        {
            pointsTotal -= attackSpeedIncreasePrice;
            attackSpeedCurrent += attackSpeedIncreaseValue;
            attackSpeedIncreasePrice += attackSpeedIncreasePrice;

            attackSpeedCurrentText.text = "" + attackSpeedCurrent;
            attackSpeedIncreasePriceText.text = attackSpeedIncreasePrice + " очков";
        }
        
        UpdatePlayerPrefs();
        SetBuyMenuText();
    }

    public void BuyAttackArea()
    {
        if (attackAreaCurrent >= ATTACK_AREA_MAX)
            return;
        
        if(pointsTotal >= attackAreaIncreasePrice)
        {
            pointsTotal -= attackAreaIncreasePrice;
            attackAreaCurrent += attackAreaIncreaseValue;
            attackAreaIncreasePrice += attackAreaIncreasePrice;

            attackAreaCurrentText.text = "" + attackAreaCurrent;
            attackAreaIncreasePriceText.text = attackAreaIncreasePrice + " очков";
        }
        UpdatePlayerPrefs();
        SetBuyMenuText();
    }

    public void PointsTotalIncrease()
    {
        pointsTotal++;
    }

    private void UpdateAnimators()
    {
        charaAnim.SetFloat("MoveSpeed", PointsSystem.Instance.movementSpeedCurrent);
        charaAnim.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedCurrent);
        scytheAnim.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedCurrent);
        trimmerAnim.SetFloat("AttackSpeed", PointsSystem.Instance.attackSpeedCurrent);
    }
    
    private void SetBuyMenuText()
    {
        movementSpeedCurrentText.text = "" + movementSpeedCurrent;
        if (movementSpeedCurrent >= MOVEMENT_SPEED_MAX)
            movementSpeedCurrentText.text = "MAX";

        attackSpeedCurrentText.text = "" + attackSpeedCurrent;
        if (attackSpeedCurrent >= ATTACK_SPEED_MAX)
            attackSpeedCurrentText.text = "MAX";

        attackAreaCurrentText.text = "" + attackAreaCurrent;
        if (attackAreaCurrent >= ATTACK_AREA_MAX)
            attackAreaCurrentText.text = "MAX";



        movementSpeedIncreaseValueText.text = "+ " + movementSpeedIncreaseValue;
        if (movementSpeedCurrent >= MOVEMENT_SPEED_MAX)
            movementSpeedIncreaseValueText.text = "MAX";

        attackSpeedIncreaseValueText.text = "+ " + attackSpeedIncreaseValue;
        if (attackSpeedCurrent >= ATTACK_SPEED_MAX)
            attackSpeedIncreaseValueText.text = "MAX";

        attackAreaIncreaseValueText.text = "+ " + attackAreaIncreaseValue;
        if (attackAreaCurrent >= ATTACK_AREA_MAX)
            attackAreaIncreaseValueText.text = "MAX";

        

        movementSpeedIncreasePriceText.text = movementSpeedIncreasePrice + " очков";
        if (movementSpeedCurrent >= MOVEMENT_SPEED_MAX)
            movementSpeedIncreasePriceText.text = "MAX";

        attackSpeedIncreasePriceText.text = attackSpeedIncreasePrice + " очков";
        if (attackSpeedCurrent >= ATTACK_SPEED_MAX)
            attackSpeedIncreasePriceText.text = "MAX";

        attackAreaIncreasePriceText.text = attackAreaIncreasePrice + " очков";
        if (attackAreaCurrent >= ATTACK_AREA_MAX)
            attackAreaIncreasePriceText.text = "MAX";
    }
    public void LoadPlayerPrefs()
    {
        pointsTotal = PlayerPrefs.GetInt("Points total", 0);
        movementSpeedIncreasePrice = PlayerPrefs.GetInt("Movement price", 10);
        attackSpeedIncreasePrice = PlayerPrefs.GetInt("Attack speed price", 10);
        attackAreaIncreasePrice = PlayerPrefs.GetInt("Attack area price", 10);
        movementSpeedCurrent = PlayerPrefs.GetInt("Movement", 1);
        attackSpeedCurrent = PlayerPrefs.GetInt("Attack speed", 1);
        attackAreaCurrent = PlayerPrefs.GetInt("Attack area", 1);

        Debug.Log("Loaded points prefs!");

        UpdateAnimators();
        SetBuyMenuText();
    }
    public void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetInt("Points total", pointsTotal);
        PlayerPrefs.SetInt("Movement price", movementSpeedIncreasePrice);
        PlayerPrefs.SetInt("Attack speed price", attackSpeedIncreasePrice);
        PlayerPrefs.SetInt("Attack area price", attackAreaIncreasePrice);
        PlayerPrefs.SetInt("Movement", movementSpeedCurrent);
        PlayerPrefs.SetInt("Attack speed", attackSpeedCurrent);
        PlayerPrefs.SetInt("Attack area", attackAreaCurrent);

        UpdateAnimators();
    }
}
