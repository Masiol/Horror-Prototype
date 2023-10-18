using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Image dimmerImage;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Ustaw pocz¹tkow¹ wartoœæ alfy na 0
        dimmerImage.color = new Color(0f, 0f, 0f, 0f);
    }

    public void DimScreen()
    {
        // P³ynne przejœcie z wartoœci alfy 0 do 1 w okreœlonym czasie
        dimmerImage.DOFade(1f, fadeDuration);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(1);
        }
    }
}