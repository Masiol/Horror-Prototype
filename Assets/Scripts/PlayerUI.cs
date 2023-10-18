using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerUI : MonoBehaviour, IPointerExitHandler
{
    public TextMeshProUGUI noteTitle;
    public Button[] buttons;
    public GameObject noteMenu;
    public GameObject imageView;
    public Button closeButton;
    public float hideDuration = 0.5f;
    public float clickDelay = 0.5f;
    private float lastClickTime = 0f;
    private bool noteMenuIsActive = false;
    public bool imageIsActive = false;
    public string currentImage = "";
    private PlayerItems playerItems;

    public TextMeshProUGUI batteryCountText;
    private horrorFlashlightBasic HorrorFlashlightBasic;

    private void Start()
    {
        Invoke("GetReference", 1f);
        playerItems = GameObject.FindObjectOfType<PlayerItems>();
        UpdateBatteryCount();
        noteTitle.text = "";
        noteMenu.transform.localScale = Vector3.zero;
        // Przypisz metodê OnButtonHover do ka¿dego przycisku
        foreach (Button button in buttons)
        {
            button.gameObject.AddComponent<EventTrigger>();
            EventTrigger trigger = button.GetComponent<EventTrigger>();

            // Dodaj zdarzenie dla najechania kursora myszy
            EventTrigger.Entry hoverEntry = new EventTrigger.Entry();
            hoverEntry.eventID = EventTriggerType.PointerEnter;
            hoverEntry.callback.AddListener((eventData) => { OnButtonHover(button); });
            trigger.triggers.Add(hoverEntry);

            // Dodaj zdarzenie dla klikniêcia myszy
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener((eventData) => { OnButtonClick(button); });
            trigger.triggers.Add(clickEntry);
        }

    }
    private void GetReference()
    {
        HorrorFlashlightBasic = GameObject.FindObjectOfType<horrorFlashlightBasic>();
    }
    public void UpdateBatteryCount()
    {
        batteryCountText.text = playerItems.batteryCount.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (imageIsActive)
            {
                CloseImage();
            }
        }

        if (Time.time - lastClickTime >= clickDelay)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                lastClickTime = Time.time;
                ToggleNoteMenu();
            }
        }
    }

    public void ToggleNoteMenu()
    {
        noteMenuIsActive = !noteMenuIsActive;

        if (noteMenuIsActive)
        {
            noteMenu.transform.DOScale(Vector3.one, hideDuration);
        }
        else
        {
            noteMenu.transform.DOScale(Vector3.zero, hideDuration);
        }
    }
    public void OnButtonHover(Button button)
    {
        foreach (Button b in buttons)
        {
            if (b.gameObject == button.gameObject)
            {
                if (b.name != "Empty")
                {
                    noteTitle.text = b.name;
                }
                break;
            }
        }
    }
    public void OnButtonClick(Button button)
    {
        foreach (Button b in buttons)
        {
            if (b.gameObject == button.gameObject)
            {
                currentImage = b.name;
                ShowImage();
                break;
            }
        }
    }

    private void ShowImage()
    {
        HorrorFlashlightBasic.TurnOffLight();
        imageView.transform.DOScale(Vector3.one, hideDuration);
        noteMenu.transform.DOScale(Vector3.zero, hideDuration);
        noteTitle.text = "";
        imageIsActive = true;
        imageView.SetActive(true);
        Sprite sprite = Resources.Load<Sprite>("Images/" + currentImage);
        imageView.GetComponent<Image>().sprite = sprite;
    }

    private void CloseImage()
    {
        HorrorFlashlightBasic.TurnOnLight();
        imageView.transform.DOScale(Vector3.zero, hideDuration);
        imageIsActive = false;
        imageView.SetActive(false);
        noteMenu.transform.DOScale(Vector3.one, hideDuration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        noteTitle.text = "";
    }
}
