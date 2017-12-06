using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Manages the canvas object that holds the help dialogue's text and text box
 */
public class HelpDialogueManager : MonoBehaviour {

    public GameObject dialogueBox;
    public Text dialogueText;
    public bool dialogActive;

    void Start() {
        dialogActive = false;
    }

    private void Update() {

    }

    public void ShowHelp(string dialogue) {
        // status that object is actively showing
        dialogActive = true;

        // canvas object box is now showing
        dialogueBox.SetActive(true);

        // canvas object text is now stored
        dialogueText.text = dialogue;
    }

    public void HideHelp() {
        // status that object is actively showing
        dialogActive = false;

        // canvas object box is now showing
        dialogueBox.SetActive(false);
    }
}
