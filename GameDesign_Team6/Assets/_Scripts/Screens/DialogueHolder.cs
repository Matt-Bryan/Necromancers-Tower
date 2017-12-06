using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Future Improvements:
 *   (1) Must hold spacebar down to see dialogue. Maybe click
 *       once to activate and click again to deactivate.
 *   (2) Clean up code.
 */
public class DialogueHolder : MonoBehaviour {

    public string dialogue;
    private DialogueManager dMan;

    public string[] dialogueLines;

    // Use this for initialization
    void Start () {
        dMan = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update () {
        
    }

    private void OnTriggerStay2D(Collider2D collision) {
        Debug.Log("Tag:" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player") {
            if (Input.GetKeyUp(KeyCode.Space)) {
                // dMan.ShowBox(dialogue);

                if (!dMan.dialogActive) {
                    dMan.dialogLines = dialogueLines;
                    dMan.currentLine = 0;

                    dMan.ShowDialogue();
                }

                if (transform.parent.GetComponent<Lab4_IdleMotion>() != null) {
                    transform.parent.GetComponent<Lab4_IdleMotion>().canMove = false;
                }
            }
        }
    }
}
