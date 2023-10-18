using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public float fadeDuration = 1f; // Czas trwania przyciemniania ekranu
    public Image fadeImage; // Obiekt typu Image do przyciemniania

    private bool isFading = false; // Flaga wskazuj¹ca, czy ekran jest przyciemniany
    private float fadeTimer = 0f; // Licznik czasu przyciemniania

    public void StartGame()
    {
        Debug.Log("Chuj");
        if (!isFading)
        {
            isFading = true;
            fadeTimer = 0f;
            StartCoroutine(FadeScreen());
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator FadeScreen()
    {
        Color originalColor = fadeImage.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f); // Docelowy kolor przyciemnienia

        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = fadeTimer / fadeDuration;
            fadeImage.color = Color.Lerp(originalColor, targetColor, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Level maze"); 
    }
}








