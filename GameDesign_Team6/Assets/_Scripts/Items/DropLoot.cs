using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that worked on this script:
 * Will Belden Brown
 * Matthew Stewart
**/

public class DropLoot : MonoBehaviour {

    public static DropLoot instance = null;
    public GameObject[] rings;
    public GameObject[] amulets;
    public GameObject[] gems;
    public GameObject[] bracers;
    public GameObject[] boots;
	//public Enchantment[] enchantments;
	public GameObject[] enchantments;
    public GameObject[] potions;
    public GameObject[] food;
    public GameObject gold;
    
    private GameObject lootDropper;

    private void Awake() {
        if (instance == null)
            instance = this;

        else
			Destroy(gameObject);

		DontDestroyOnLoad (gameObject);
    }

    public void dropLoot(int enemyTier, GameObject actor) {
        int randomItem = Random.Range(1,101);
        lootDropper = actor;

        if (randomItem <= 5) {
            spawnHealingItem(5);
        }
        else if (randomItem <= 10) {
            spawnHealingItem(4);
        }
        else if (randomItem <= 15) {
            spawnHealingItem(3);
        }
        else if (randomItem <= 20) {
            spawnHealingItem(2);
        }
        else if (randomItem <= 25) {
            spawnHealingItem(1);
        }
        else {
            spawnGold(1);
        }

        //minor magic item (1), medium magic item (2), major magic item (3)
        //cases 1-5 for enemy drops based on enemy level, case 6 for random items in shop
        /*switch (enemyTier) {
            case 1:
                if (randomItem >= 75 && randomItem <= 94)
                    generateLoot(1);

                else if (randomItem >= 95)
                    generateLoot(2);

			    else if (randomItem >= 65 && randomItem < 75)
                    spawnHealingItem(1);

                else 
                    spawnGold(1);

                break;

            case 2:
                if (randomItem >= 65 && randomItem <= 85)
                    generateLoot(1);

                else if (randomItem >= 86 && randomItem <= 99)
                    generateLoot(2);

                else if (randomItem == 100)
                    generateLoot(3);

                else if (randomItem >= 50 && randomItem < 65)
                    spawnHealingItem(2);

                else 
                    spawnGold(2);

                break;

            case 3:
                if (randomItem >= 56 && randomItem <= 65)
                    generateLoot(1);

                else if (randomItem >= 66 && randomItem <= 95)
                    generateLoot(2);

                else if (randomItem >= 96)
                    generateLoot(3);

                else if (randomItem >= 49 && randomItem < 56)
                    spawnHealingItem(3);

                else 
                    spawnGold(3);

                break;
            
            case 4:
                if (randomItem >= 56 && randomItem <= 65)
                    generateLoot(1);

                else if (randomItem >= 66 && randomItem <= 85)
                    generateLoot(2);

                else if (randomItem >= 86)
                    generateLoot(3);

                else if (randomItem >= 48 && randomItem < 56)
                    spawnHealingItem(4);

                else 
                    spawnGold(4);

                break;

            case 5:
                if (randomItem >= 56 && randomItem <= 65)
                    generateLoot(1);

                else if (randomItem >= 66 && randomItem <= 80)
                    generateLoot(2);

                else if (randomItem >= 81)
                    generateLoot(3);

                else if (randomItem >= 47 && randomItem < 56)
                    spawnHealingItem(5);

                else 
                    spawnGold(5);

                break;

           case 6:
                if (randomItem >= 1 && randomItem <= 25)
                    generateLoot(1);

                else if (randomItem >= 26 && randomItem <= 50)
                    generateLoot(2);

                else if (randomItem >= 76)
                    generateLoot(3);

                else if (randomItem >= 51 && randomItem < 75)
                    spawnHealingItem(Random.Range(1,6));

                break;
        }
        */
    }

    void generateLoot(int itemLevel) {
        GameObject item;
        GameObject enchantment;
        int itemType = Random.Range(1, 6);
        if (itemType == 1)
            item = rings[Random.Range(0, rings.Length)];

        else if (itemType == 2)
            item = amulets[Random.Range(0, amulets.Length)];

        else if (itemType == 3)
            item = gems[Random.Range(0, gems.Length)];

        else if (itemType == 4)
            item = bracers[Random.Range(0, bracers.Length)];

        else
            item = boots[Random.Range(0, boots.Length)];

		GameObject instance = Instantiate(item, lootDropper.transform.position, Quaternion.identity) as GameObject;

        for(int i=1; i<=itemLevel; i++) {
            enchantment = enchantments[Random.Range(0, enchantments.Length)];
			instance.GetComponent<Item>().enchant(enchantment);
        }

		instance.GetComponent<Item>().setName(itemLevel);
    }

    void spawnGold(int multiplier) {
        int goldAmount = Random.Range(1, 100) * multiplier;

        //instantiate gold drop
        GameObject instance = Instantiate(gold, lootDropper.transform.position, Quaternion.identity) as GameObject;

		instance.GetComponent<Item>().setGold(goldAmount);
    }

    void spawnHealingItem(int multiplier) {
        GameObject item;
        int healingAmount = Random.Range(1, 15) * multiplier;

        if(healingAmount >= 38)
            item = potions[Random.Range(0, potions.Length)];
		else
            item = food[Random.Range(0, food.Length)];

        item = Instantiate(item, lootDropper.transform.position, Quaternion.identity) as GameObject;

		item.GetComponent<Item>().setHealing(healingAmount);
		item.GetComponent<Item>().setGold(healingAmount * 5);
    }
}