using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that worked on this script: 
 * Will Belden Brown
**/


//[System.Serializable]
public class Enchantment : MonoBehaviour{
    public string name;
    public int healthBoost = 0;
    public int speedBoost = 0;
    public int damageBoost = 0;
    public int damageResistance = 0;
    public int valueInGold = 0;

    public string toString() {
        if (healthBoost != 0)
            return name + " +" + healthBoost.ToString();

        else if (speedBoost != 0)
            return name + " +" + speedBoost.ToString();

        else if (damageBoost != 0)
            return name + " +" + damageBoost.ToString();

        else
            return name + " +" + damageResistance.ToString();
    }
}
