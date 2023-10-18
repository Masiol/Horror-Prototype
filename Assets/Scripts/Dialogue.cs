using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;

    private DialogueManager dialogueManager;
    private bool startDialogue;
    private Transform player;
    [SerializeField] private float minDist;
    private float distance;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
      /*  float distance = Vector3.Distance(player.position, this.transform.position);

        if (distance <= minDist)
        {
            Debug.Log("Gracz jest blisko");
        }
        else
        {
            Debug.Log("Gracz jest daleko ");
        }*/

        if (Input.GetKeyDown(KeyCode.O) && !startDialogue)
        {
            TriggerDialogue();
            startDialogue = true;
        }
    }

    public void TriggerDialogue()
    {       
        dialogueManager.StartDialogue(this);
    }
}