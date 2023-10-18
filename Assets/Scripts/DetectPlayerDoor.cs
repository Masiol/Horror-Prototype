using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerDoor : MonoBehaviour
{
    [SerializeField] private Doors doors;

    private void Start()
    {
        //doors = GetComponentInParent<Doors>();
    }
    public enum Trigger
    {
        Front,
        Back
    }
    public Trigger trigger;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // Debug.Log("Player entered " + trigger.ToString() + " trigger");
            doors.Action();
        }
    }
}
