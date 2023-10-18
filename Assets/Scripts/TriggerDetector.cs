using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public Collider currentCollider;
    public List<Furniture> Furniture = new List<Furniture>();

    private void Awake()
    {
        Furniture[] furniture = GameObject.FindObjectsOfType<Furniture>();
        for (int i = 0; i < furniture.Length; i++)
        {
            Furniture.Add(furniture[i]);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            currentCollider = other;
            currentCollider.GetComponentInParent<Furniture>().canInteract = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            currentCollider = null;     

            for (int i = 0; i < Furniture.Count; i++)
            {
                Furniture[i].canInteract = false;
            }
        }
    }
}
