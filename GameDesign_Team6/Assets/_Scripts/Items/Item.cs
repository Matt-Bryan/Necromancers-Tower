using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that worked on this script:
 * Will Belden Brown
 * Matthew Stewart
**/

public class Item : MonoBehaviour {
	public Sprite itemSprite;
    new public string name;
    public string type;
    private int valueInGold = 0;
    private int healing;
	//private List<Enchantment> enchantments = new List<Enchantment>();
	private List<GameObject> enchantments = new List<GameObject>();

	public void enchant(GameObject enchantment) {
        enchantments.Add(enchantment);

        valueInGold += enchantment.GetComponent<Enchantment>().valueInGold;
    }

    public void setName(int itemLevel) {
        if (itemLevel == 1)
            name += "Minor " + type + " of " + enchantments[0].GetComponent("Enchantment").name;

        else if (itemLevel == 2)
            name += "Moderate " + type + " of " + enchantments[0].GetComponent("Enchantment").name + ", " + enchantments[1].GetComponent("Enchantment").name;

        else if ( itemLevel == 3)
            name += "Major " + type + " of " + enchantments[0].GetComponent("Enchantment").name + ", " + enchantments[1].GetComponent("Enchantment").name + ", and " + enchantments[2].GetComponent("Enchantment").name;
    }

    public void setGold(int addGold) {
        valueInGold += addGold;
    }

    public int getGold() {
        return valueInGold;
    }

    public List<GameObject> getEnchantments() {
        return enchantments;
    }

    public void setHealing(int addHealing) {
		healing = addHealing;
		Debug.Log ("setting healing " + healing.ToString ());
    }

    public int getHealing() {
		Debug.Log ("returning healing " + healing.ToString ());
        return healing;
    }
}
