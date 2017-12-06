using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Future Improvements:
 *   (1) Fix bug where first line of dialogue is not being displayed.
 *   (1) Clean up code.
 */
public class DialogueManager : MonoBehaviour {

    public GameObject dBox;
    public Text dText;

    public bool dialogActive; // defaults to false

    public string[] dialogLines;
    public int currentLine;

    // Use this for initialization
    void Start () {
        currentLine = 0;
    }

    // Update is called once per frame
    void Update () {
        if (dialogActive && Input.GetKeyUp(KeyCode.Mouse0)) {
            currentLine++;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) || currentLine > dialogLines.Length - 1)　{
            dBox.SetActive(false);
            dialogActive = false;

            currentLine = 0;

            GameManager.instance.changeScene("MattTestScene");
        }

        dText.text = dialogLines[currentLine];
    }

    public void ShowBox(string dialogue) {
        dialogActive = true;
        dBox.SetActive(true);
        dText.text = dialogue;
    }

    public void ShowDialogue() {
        dialogActive = true;
        dBox.SetActive(true);
    }
 }
