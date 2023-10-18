using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Furniture : MonoBehaviour
{

    private bool isOpening, isClosing, isOpen;


    public float openAngle = 90f;
    public float duration = 1f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private FurnitureInteract furnitureInteract;

    private Transform TableOpen;

    public List<GameObject> itemsInsideFurniture = new List<GameObject>();

    private bool isPressing = false;
    public float pressTime = 0f;
    public float longPressTime = 1f;

    public bool canInteract;

    void Start()
    {
        
        furnitureInteract = GetComponentInChildren<FurnitureInteract>();
        //raycastDetect = FindObjectOfType<RaycastDetect>();

        TableOpen = transform.GetChild(0);
        closedRotation = TableOpen.transform.localRotation;
        openRotation = Quaternion.Euler(closedRotation.eulerAngles.x, closedRotation.eulerAngles.y, closedRotation.eulerAngles.z - openAngle);
    }

    private bool isReferenceFound = false;

    // Update is called once per frame
    void Update()
    {
       /* if (AddressableLoader.loadedAssets == true && !isReferenceFound)
        {
            raycastDetect = FindObjectOfType<RaycastDetect>();
            
            if (raycastDetect != null)
            {
                Debug.Log("Referencja zosta³a znaleziona");
                isReferenceFound = true;
            }
        }*/
        if (canInteract /*&& raycastDetect.canInteract*/)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isPressing = true;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                isPressing = false;

                if (!isOpen && pressTime < 1f)
                    OpenDoor();
                else if (pressTime >= longPressTime)
                    CanCollectItemsFromFurniture();
                else if (pressTime <= 0.3)
                    CloseDoor();

                pressTime = 0f;
            }

            if (isPressing)
            {
                pressTime += Time.deltaTime;
            }
        }
    }

    private void OpenDoor()
    {
        isOpening = true;

        TableOpen.DOLocalRotate(openRotation.eulerAngles, duration).OnComplete(() => {
            isOpening = false;
            isClosing = false;

            isOpen = true;

        });

        // rigidBody.constraints = RigidbodyConstraints.None;
    }

    private void CloseDoor()
    {
        isClosing = true;

        TableOpen.DOLocalRotate(closedRotation.eulerAngles, duration).OnComplete(() => {
            isClosing = false;
            isOpen = false;
        });

        // rigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    string itemName;
    int itemAmount;
    ItemType itemType;
    Sprite itemImage;

    void CanCollectItemsFromFurniture()
    {
        PlayerItems playerItems = FindObjectOfType<PlayerItems>();

        if (isOpen)
        {
            for (int i = 0; i < itemsInsideFurniture.Count; i++)
            {
                itemName = itemsInsideFurniture[i].GetComponent<ItemCollect>().nameItem;
                itemAmount = itemsInsideFurniture[i].GetComponent<ItemCollect>().itemAmount;
                itemType = itemsInsideFurniture[i].GetComponent<ItemCollect>().itemType;
                itemImage = itemsInsideFurniture[i].GetComponent<ItemCollect>().itemImage;

                string itemTypeString = itemType.ToString(); // Konwertujemy wartoœæ enum na string

                switch (itemType)
                {
                    case ItemType.Battery:
                        playerItems.AddBattery(itemAmount);
                        break;
                    case ItemType.List:
                        playerItems.AddList(itemAmount, itemName, itemImage);
                        break;
                    case ItemType.Item2:
                        // Do something if the item type is Item2
                        break;
                    case ItemType.Item3:
                        // Do something if the item type is Item3
                        break;
                    case ItemType.Item4:
                        // Do something if the item type is Item4
                        break;
                    default:
                        // Handle any unexpected item types
                        break;
                }

                Destroy(itemsInsideFurniture[i], 0.1f);
                Debug.Log(itemName + itemAmount + " - ItemType: " + itemTypeString); // Wyœwietlamy wartoœæ itemType jako string
            }
        }
    }
}
