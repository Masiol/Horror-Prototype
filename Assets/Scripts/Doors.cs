using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Doors : MonoBehaviour
{
    public enum DoorDirection
    {
        Left,
        Right,
        Forward,
        Backward
    }

    public DoorDirection doorDirection;

    public float openAngle = 90f;
    public float duration = 1f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private bool isOpen = false;

    private bool canDoAcction;

    private NavMeshObstacle obstacle;

    //private Rigidbody rigidBody;


    private void Start()
    {
        obstacle = GetComponentInParent<NavMeshObstacle>();
        obstacle.enabled = true;
        closedRotation = transform.localRotation;
        switch (doorDirection)
        {
            case DoorDirection.Left:
                openRotation = Quaternion.Euler(closedRotation.eulerAngles.x, closedRotation.eulerAngles.y - openAngle, closedRotation.eulerAngles.z);
                break;
            case DoorDirection.Right:
                openRotation = Quaternion.Euler(closedRotation.eulerAngles.x, closedRotation.eulerAngles.y + openAngle, closedRotation.eulerAngles.z);
                break;
            case DoorDirection.Forward:
                openRotation = Quaternion.Euler(closedRotation.eulerAngles.x, closedRotation.eulerAngles.y, closedRotation.eulerAngles.z - openAngle);
                break;
            case DoorDirection.Backward:
                openRotation = Quaternion.Euler(closedRotation.eulerAngles.x + openAngle, closedRotation.eulerAngles.y, closedRotation.eulerAngles.z);
                break;
        }

    }
    // Update is called once per frame
    public bool isOpening;
    public bool isClosing;

    public void Action()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isOpening && !isClosing)
        {
            if (!isOpen)
                OpenDoor();
            else
                CloseDoor();
        }
    }
    private void OpenDoor()
    {
        isOpening = true;
        obstacle.enabled = false;

        transform.DOLocalRotate(openRotation.eulerAngles, duration).OnComplete(() => {
            isOpening = false;
            isClosing = false;
            isOpen = true;
        });

        // rigidBody.constraints = RigidbodyConstraints.None;
    }

    private void CloseDoor()
    {
        isClosing = true;
        obstacle.enabled = true;
        transform.DOLocalRotate(closedRotation.eulerAngles, duration).OnComplete(() => {
            isClosing = false;
            isOpen = false;
        });

        // rigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }
}






