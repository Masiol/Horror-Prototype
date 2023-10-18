using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private CameraController camera;
    private GameOver gameOver;
    // Start is called before the first frame update
    void Start()
    {
        gameOver = FindObjectOfType<GameOver>();
        camera = GetComponentInChildren<CameraController>(); 
    }

    // Update is called once per frame
   public void Die()
    {
        StartCoroutine(Died());
    }
    public IEnumerator Died()
    {
        yield return new WaitForSeconds(1.5f);
        camera.gameObject.AddComponent<Rigidbody>();
        camera.gameObject.GetComponent<BoxCollider>().enabled = true;
        camera.gameObject.transform.parent = null;
        yield return new WaitForSeconds(1);
        GameObject.FindObjectOfType<GameOver>().DimScreen();
        Destroy(camera.GetComponent<Rigidbody>());
    }
}
