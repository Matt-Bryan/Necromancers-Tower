using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * People that worked on this script: 
 * Will Belden Brown
**/

public class Shop : MonoBehaviour {
    private GameObject player;
    private List<GameObject> inventory;
    private GameManager GameManager;

    private float playerHealth;
    private float playerMaxHealth;
    private float playerSpeed;
    private float playerDamage;
    private int playerGold;

    private int priceDamageBoost;
    private int priceHealthBoost;
    private int priceSpeedBoost;
    private int priceMinorHeal = 50;
    private int priceModerateHeal = 125;
    private int priceMajorHeal = 250;

    public Text merchantText;
    public Text playerGoldText;
    public Text playerHealthText;
    public Text playerDmgText;
    public Text playerSpeedText;
    public Text minorHealPriceText;
    public Text moderateHealPriceText;
    public Text majorHealPriceText;
    public Text healthBoostPriceText;
    public Text speedBoostPriceText;
    public Text damageBoostPriceText;
	
    // Use this for initialization
	void Start () {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameManager.player;
        playerMaxHealth = player.GetComponent<PlayerController>().maxHealthPoints;
        playerHealth = player.GetComponent<PlayerController>().healthPoints;
        playerSpeed = player.GetComponent<PlayerController>().moveSpeed;
        playerDamage = player.GetComponent<PlayerController>().attackDamage;
        playerGold = player.GetComponent<PlayerController>().getPlayerGold();

        minorHealPriceText.text = 50.ToString();
        moderateHealPriceText.text = 125.ToString();
        majorHealPriceText.text = 250.ToString();
        
        merchantText.text = "Welcome to my humble shop";

        setInventory();
	}

    public void setInventory() {
        playerMaxHealth = player.GetComponent<PlayerController>().maxHealthPoints;
        playerHealth = player.GetComponent<PlayerController>().healthPoints;
        playerSpeed = player.GetComponent<PlayerController>().moveSpeed;
        playerDamage = player.GetComponent<PlayerController>().attackDamage;
        playerGold = player.GetComponent<PlayerController>().getPlayerGold();

        priceDamageBoost = (int)playerDamage * 75;
        priceHealthBoost = ((int)playerMaxHealth - 90) * 10;
        priceSpeedBoost = (int)playerSpeed * 75;

        damageBoostPriceText.text = priceDamageBoost.ToString();
        speedBoostPriceText.text = priceSpeedBoost.ToString();
        healthBoostPriceText.text = priceHealthBoost.ToString();

        playerGoldText.text = playerGold.ToString();
        playerHealthText.text = playerHealth.ToString() + " / " + playerMaxHealth.ToString();
        playerDmgText.text = playerDamage.ToString();
        playerSpeedText.text = playerSpeed.ToString();
    }

    public bool buy(int price) {
        if ((playerGold - price) < 0) {
            merchantText.text = "You'll need more than a measly sum to buy that";
            return false;
        }

        merchantText.text = "Thank you for your patronage";
        playerGold -= price;
        player.GetComponent<PlayerController>().setPlayerGold(playerGold);
        return true;
    }

    public void buyDamageUpgradeButton() {
        if(buy(priceDamageBoost)) {
            player.GetComponent<PlayerController>().upgradeStats(1, "Damage");
            setInventory();
        }
    }

    public void buySpeedUpgradeButton() {
        if(buy(priceSpeedBoost)) {
            player.GetComponent<PlayerController>().upgradeStats(0.5f, "Speed");
            setInventory();
        }
    }

    public void buyHealthUpgradeButton() {
        if(buy(priceHealthBoost)) {
            player.GetComponent<PlayerController>().upgradeStats(10, "Health");
            setInventory();
        }
    }

    public void buyMinorHealButton() {
        if(buy(priceMinorHeal)) {
            player.GetComponent<PlayerController>().upgradeStats(15, "Heal");
            setInventory();
        }
    }

    public void buyModerateHealButton() {
        if(buy(priceModerateHeal)) {
            player.GetComponent<PlayerController>().upgradeStats(45, "Heal");
            setInventory();
        }
    }

    public void buyMajorHealButton() {
        if(buy(priceMajorHeal)) {
            player.GetComponent<PlayerController>().upgradeStats(80, "Heal");
            setInventory();
        }
    }

    public void nextLevelButton() {
        GameManager.SendMessage("nextLevel");
    }
}
