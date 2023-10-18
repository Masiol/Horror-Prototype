using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Button> itemImages;
    private horrorFlashlightBasic horrorFlashlight;
    public int batteryCount = 3;
    PlayerUI playerUI;
    void Start()
    {
        playerUI = GameObject.FindObjectOfType<PlayerUI>();
        this.transform.name = "Player";
        horrorFlashlight = GetComponentInChildren<horrorFlashlightBasic>();


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddBattery(int amount)
    {
        batteryCount += amount;
        playerUI.UpdateBatteryCount();
    }
    public void AddList(int amount, string name, Sprite image)
    {
        // ZnajdŸ pierwszy pusty slot w UI
        int emptyIndex = -1; // pocz¹tkowa wartoœæ
        for (int i = 0; i < itemImages.Count; i++)
        {
            if (itemImages[i].GetComponent<Image>().sprite.name == "nvl")
            {
                emptyIndex = i;
                break;
            }
        }

        // Jeœli znaleziono pusty slot, to dodaj przedmiot
        if (emptyIndex != -1)
        {
            itemImages[emptyIndex].GetComponent<Image>().sprite = image;
            itemImages[emptyIndex].interactable = true;
            // Ustaw nazwê przedmiotu jako tytu³ UI dla danego slotu
            itemImages[emptyIndex].transform.name = name;
        }
        else
        {
            Debug.Log("Nie znaleziono wolnego miejsca w UI!");
        }
    }
}
