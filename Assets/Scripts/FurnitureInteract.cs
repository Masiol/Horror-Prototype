using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureInteract : MonoBehaviour
{
    public bool detectPlayer;
    private RaycastDetect raycastDetect;

    private bool isReferenceFound = false;

    private void Update()
    {
        if (AddressableLoader.loadedAssets == true && !isReferenceFound)
        {
            raycastDetect = FindObjectOfType<RaycastDetect>();
            if (raycastDetect != null)
            {
                Debug.Log("Referencja zosta³a znaleziona");
                isReferenceFound = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = true;
            raycastDetect.targetCollider = this.GetComponentInParent<Furniture>().transform.GetChild(1).GetComponent<Collider>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            detectPlayer = false;
        }
    }
}
