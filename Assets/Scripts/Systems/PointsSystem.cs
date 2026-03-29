using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PointsSystem : MonoBehaviour
{
    public static PointsSystem Instance;
    public int pointsTotal { get; private set; }
    public const int MOVEMENT_SPEED_MAX = 10;
    public const int ATTACK_SPEED_MAX = 5;
    public const int ATTACK_AREA_MAX = 10;

    public int movementSpeedIncrement { get; private set; } = 1;
    public int attackSpeedIncrement { get; private set; } = 1;
    public int attackAreaIncrement { get; private set; } = 1;

    // stays here
    public int movementSpeedPrice { get; private set; } = 10;
    public int attackSpeedPrice { get; private set; } = 10;
    public int attackAreaPrice { get; private set; } = 10;
    
    // goes outside
    public int movementSpeedValue { get; private set; } = 1;
    public int attackSpeedValue { get; private set; } = 1;
    public int attackAreaValue { get; private set; } = 1;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy (gameObject);
        
        LoadPlayerPrefs();

        DontDestroyOnLoad(Instance); // Keeps when loading new scene
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyWalkSpeed()
    {
        if (movementSpeedValue >= MOVEMENT_SPEED_MAX)
            return;
        
        if(pointsTotal >= movementSpeedPrice)
        {
            pointsTotal -= movementSpeedPrice;
            movementSpeedValue += movementSpeedIncrement;
            movementSpeedPrice += movementSpeedPrice;
        }
        
        UpdatePlayerPrefs();
    }
    public void BuyAttackSpeed()
    {
        if (attackSpeedValue >= ATTACK_SPEED_MAX)
            return;
        
        if(pointsTotal >= attackSpeedPrice)
        {
            pointsTotal -= attackSpeedPrice;
            attackSpeedValue += attackSpeedIncrement;
            attackSpeedPrice += attackSpeedPrice;
        }
        
        UpdatePlayerPrefs();
    }
    public void BuyAttackArea()
    {
        if (attackAreaValue >= ATTACK_AREA_MAX)
            return;
        
        if(pointsTotal >= attackAreaPrice)
        {
            pointsTotal -= attackAreaPrice;
            attackAreaValue += attackAreaIncrement;
            attackAreaPrice += attackAreaPrice;
        }

        UpdatePlayerPrefs();
    }

    public void PointsTotalIncrease()
    {
        pointsTotal++;
    }
 
    public void LoadPlayerPrefs()
    {
        pointsTotal = PlayerPrefs.GetInt("Points total", 0);
        movementSpeedPrice = PlayerPrefs.GetInt("Movement price", 10);
        attackSpeedPrice = PlayerPrefs.GetInt("Attack speed price", 10);
        attackAreaPrice = PlayerPrefs.GetInt("Attack area price", 10);
        movementSpeedValue = PlayerPrefs.GetInt("Movement", 1);
        attackSpeedValue = PlayerPrefs.GetInt("Attack speed", 1);
        attackAreaValue = PlayerPrefs.GetInt("Attack area", 1);

        Debug.Log("Loaded points prefs!");
    }
    public void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetInt("Points total", pointsTotal);
        PlayerPrefs.SetInt("Movement price", movementSpeedPrice);
        PlayerPrefs.SetInt("Attack speed price", attackSpeedPrice);
        PlayerPrefs.SetInt("Attack area price", attackAreaPrice);
        PlayerPrefs.SetInt("Movement", movementSpeedValue);
        PlayerPrefs.SetInt("Attack speed", attackSpeedValue);
        PlayerPrefs.SetInt("Attack area", attackAreaValue);
    }
}
