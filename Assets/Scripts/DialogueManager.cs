using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueBox;
    public float sentenceDelay = 1.0f;

    private Queue<string> sentences;
    private bool isTyping = false;
    private bool isDialogueActive = false;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.transform.DOScale(1, 0.5f).SetEase(Ease.OutCubic);


        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
        isDialogueActive = true;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        yield return new WaitForSeconds(sentenceDelay);
        isTyping = false;
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // Jeœli tekst jest w trakcie pisania, zignoruj naciœniêcie spacji
                return;
            }

            DisplayNextSentence();
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");
        isDialogueActive = false;
        dialogueBox.transform.DOScale(0, 0.5f).SetEase(Ease.OutCubic);
    }
}