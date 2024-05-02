using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NPCController : MonoBehaviour
{
    public GameObject dialoguePanel; // UI Panel where everything is happening
    public Text dialogueText; // Text window where lines are going to be shown
    public string[] dialogue; // Lines that are going to be shown
    private int index; // Current line of dialogue

    public GameObject contButton; // Continue button

    public float textspeed; // Speed of showing text
    public bool playerIsClose; // Flag of Player beiing close to NPC

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose) // Checking if Player is starting conversation and he is close to the NPC
        {
            if (dialoguePanel.activeInHierarchy) // Checking if Panel active, if it is active, then by pressing E button, player wants to
                                                    // leave the conversation, so we are closing window
            {
                zeroText(); // close conversation window

            }
            else // If Panel is not active, then by pressing E button, player wants to
                 // enter the conversation, so we are opening window and starting typing process
            {
                dialoguePanel.SetActive(true); // open conversation window
                StartCoroutine(Typing()); // start typing
            }

        }

        if (dialogueText.text == dialogue[index]) // If we are finished typing the line that should be shown, then we 
                                               // give the player the opportunity to play next line by showing the button 
        {
            contButton.SetActive(true); // Show the button
        }
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray()) // dialogue[index] is current line that should be showing, so 
                                                            // we are doing foreach char in the current line
        {
            dialogueText.text += letter; // we are showing additional letter from current line
            yield return new WaitForSeconds(textspeed); // optional: it helps to show the line not in one frame
        }

    }

    public void zeroText()
    {
        dialogueText.text = ""; //Zeroing all of the text that is showing
        index = 0; //Zeroing current line number 
        dialoguePanel.SetActive(false); //Closing the panel with dialogue
        SceneManager.LoadScene("FinalScene", LoadSceneMode.Single);
    }

    public void NextLine() // Showing the next line
    {
        contButton.SetActive(false); // Because we started to show next line,
                                     // we dont give an opportunity to the Player to miss some information

        if (index < dialogue.Length - 1) // Checking if it is not the last line.
        {
            index++; // If it is not, we are changing number of line that should be showing 
            dialogueText.text = ""; // And delete the previous line
            StartCoroutine(Typing()); // Start typing of the new line
        }
        else
        {
            zeroText(); // If it is the last line, then closing window
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // could be done with collision.CompareTag("Player") 
        {
            playerIsClose = true; // Changing Flag to true if Player is close
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // could be done with collision.CompareTag("Player")
        {
            playerIsClose = false; // Changing Flag to false if Player is away
            zeroText(); // Closing the diologue window
        }
    }
}
