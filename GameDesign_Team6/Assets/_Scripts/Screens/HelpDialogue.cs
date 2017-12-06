using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script that displays "helpful" text when player enters the triggerTextZone.
 */
public class HelpDialogue : MonoBehaviour {

    public string dialogue;
    public HelpDialogueManager manager;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            manager.ShowHelp(dialogue);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            manager.HideHelp();
        }
    } 
}
