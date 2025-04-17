using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueSystem : UiBase
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public GameObject[] optionButtons;
    public string[] dialogues;

    public GameObject interactPrompt; // “按E对话”提示
    public KeyCode interactKey = KeyCode.E;

    private bool playerInRange = false;
    private int dialogueIndex = 0;
    private bool inDialogue = false;

    void Update()
    {
        if (playerInRange && !inDialogue)
        {
            interactPrompt.SetActive(true);
            if (Input.GetKeyDown(interactKey))
            {
                StartDialogue();
            }
        }

        if (inDialogue && Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceDialogue();
        }
    }

    void StartDialogue()
    {
        inDialogue = true;
        dialoguePanel.SetActive(true);
        dialogueIndex = 0;
        ShowCurrentDialogue();
        interactPrompt.SetActive(false);
    }

    void ShowCurrentDialogue()
    {
        if (dialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[dialogueIndex];
        }
        else
        {
            ShowOptions();
        }
    }

    void AdvanceDialogue()
    {
        dialogueIndex++;
        ShowCurrentDialogue();
    }

    void ShowOptions()
    {
        foreach (GameObject btn in optionButtons)
            btn.SetActive(true);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        foreach (GameObject btn in optionButtons)
            btn.SetActive(false);
        inDialogue = false;
    }

    // 玩家进入触发区域
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // 玩家离开触发区域
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactPrompt.SetActive(false);
        }
    }
}
