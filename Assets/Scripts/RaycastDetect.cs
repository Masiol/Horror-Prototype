using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDetect : MonoBehaviour
{
    public Transform raycastObject;
    public Collider targetCollider;

    private float raycastDistance = 10f;
    private Color rayColor = Color.red;
    public GameObject currentTarget;
    public GameObject lastTarget;


    public bool canInteract;
    public horrorFlashlightBasic horror;
    [SerializeField] private float coneAngle = 10f;

    private void Start()
    {
        horror = GameObject.FindObjectOfType<horrorFlashlightBasic>();
    }
    void Update()
    {
        Vector3 direction = raycastObject.forward;
        Vector3 coneOrigin = raycastObject.position;
        Quaternion coneRotation = Quaternion.LookRotation(direction);

        //Debug.DrawRay(coneOrigin, direction * raycastDistance, rayColor);

        float coneRadius = Mathf.Tan(coneAngle * Mathf.Deg2Rad) * raycastDistance;
        Vector3[] coneDirections = {
        coneRotation * Quaternion.Euler(0f, coneAngle, 0f) * Vector3.forward,
        coneRotation * Quaternion.Euler(0f, -coneAngle, 0f) * Vector3.forward,
        coneRotation * Quaternion.Euler(coneAngle, 0f, 0f) * Vector3.forward,
        coneRotation * Quaternion.Euler(-coneAngle, 0f, 0f) * Vector3.forward
    };

        foreach (Vector3 coneDirection in coneDirections)
        {
           // Debug.DrawRay(coneOrigin, coneDirection * coneRadius, rayColor);
            Ray ray = new Ray(coneOrigin, coneDirection);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance) && hit.collider == targetCollider)
            {
                //canInteract = true;
                //break;
            }
            else
            {
                //canInteract = false;
            }
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Hit an enemy!");
                currentTarget = hit.collider.gameObject;
                Enemy enemyScript = currentTarget.GetComponent<Enemy>();
                if (enemyScript != null && horror.turnedOn)
                {
                    enemyScript.canBlindEnemy = true;
                }
                break;
            }
            else
            {
                if (currentTarget != null)
                {
                    Enemy enemyScript = currentTarget.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        //enemyScript.isFlashed = false;
                    }
                    currentTarget = null;
                }
            }
        }
    }
}