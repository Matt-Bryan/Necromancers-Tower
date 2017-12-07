using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * People that worked on this script: 
 * Jordan Barham
 *
 * This script has been deprecated, as there is no inventory system
**/

public class Inventory : MonoBehaviour {

	public const int numItemSlots = 8;
	public Image[] itemImages = new Image[numItemSlots];
	public Item[] items = new Item[numItemSlots];


	public void addItem(Item toAdd) {
		for (int i = 0; i < items.Length; i++) {
			if (items [i] == null) {
				items [i] = toAdd;
				itemImages [i].sprite = toAdd.itemSprite;
				itemImages [i].enabled = true;
				return;
			}
		}
	}

	public void removeItem(Item toRemove) {
		for (int i = 0; i < items.Length; i++) {
			if (items [i] == toRemove) {
				items [i] = null;
				itemImages [i].sprite = null;
				itemImages [i].enabled = false;
				return;
			}
		}
	}
}
