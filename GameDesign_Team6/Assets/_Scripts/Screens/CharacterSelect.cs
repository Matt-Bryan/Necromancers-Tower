using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * People that touched this script:
 * Matt Bryan
 * Matthew Stewart
 * Lauren Kirk (Barely...)
**/

public class CharacterSelect : MonoBehaviour {

	public GameObject myBackground;
	public GameObject otherBackground1;
	public GameObject otherBackground2;
	public GameObject selectedChar;
	public GameObject myCharPrefab;
	public TextAsset descriptionFile;
	public TextAsset abilitiesFile;
    public GameObject BestScoreDisplay;

	private SpriteRenderer rend;
	private Color dim = new Color(0.8f,0.8f,0.8f,255);
	private Color lit = new Color(1f,1f,1f,255);

	void Start() {
		rend = GetComponent<SpriteRenderer>();
		rend.color = dim;
	}

	void OnMouseEnter() {
		rend.color = lit;
	}

	void OnMouseExit() {
		rend.color = dim;
	}

	void OnMouseDown() {
		changeBackground();
		changeSprites();
		changeTexts();
		GameManager.instance.selectedCharPrefab = myCharPrefab;
	}

	private void changeBackground() {
		otherBackground1.SetActive(false);
		otherBackground2.SetActive(false);
		myBackground.SetActive(true);
	}

	private void changeSprites() {
		SpriteRenderer otherRend = selectedChar.GetComponent<SpriteRenderer>();
		SpriteRenderer myRend = GetComponent<SpriteRenderer>();
		otherRend.sprite = myRend.sprite;

		selectedChar.transform.localScale = new Vector3(10, 10, 0);
	}

    private void changeTexts() {
        if (BestScoreDisplay.active == false) {
           BestScoreDisplay.SetActive(true);
        }

        Text desc = GameObject.Find("DescriptionText").GetComponent<Text>();
        Text abil = GameObject.Find("AbilitiesText").GetComponent<Text>();
        Text score = GameObject.Find("BestScoreText").GetComponent<Text>();

        desc.text = descriptionFile.text;
        abil.text = abilitiesFile.text;

        if (this.gameObject.name == "Peasant_Pete") {
            score.text = PlayerPrefs.GetInt("Pete Score").ToString();
        }
        else  if (this.gameObject.name == "Sorceress_Sera") {
            score.text = PlayerPrefs.GetInt("Sera Score").ToString();
        }
        else {
            score.text = "Error: Player not found.";
        }
	}
}
