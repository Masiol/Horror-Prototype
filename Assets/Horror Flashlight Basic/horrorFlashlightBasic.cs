using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class horrorFlashlightBasic : MonoBehaviour
{
    private bool isWalking = false;
    public bool turnedOn = true;
    private float batteryLife = 100.0f; // Wytrzymałość latarki
    private float minBatteryLife = 2f; // Minimalna wytrzymałość latarki
    private float batteryDrainRate = 0.05f; // Tempo spadku wytrzymałości latarki
    private float nextBatteryDrainTime = 0.0f; // Czas następnego spadku wytrzymałości latarki

    public PlayerItems playerItems;
    private bool canUseFlashlight = true; // Czy można użyć latarki
    private bool reloadingFlashLight;
    private float reloadingTime = 3f;
    private float startReloadingTime;

    public GameObject light;
    public Image batteryBar; // Obrazek reprezentujący wytrzymałość latarki
    Animator anim;
    AudioSource audio;
    Light spotlight;
    private Light playerLight;

    private PlayerUI playerUI;
    private float startPlayerLightIntensity;
    private float startFlashLightIntensity;

    public bool enoughIntensityForBlindEnemy = true;

    // Use this for initialization
    void Start()
    {
        playerItems = GameObject.FindObjectOfType<PlayerItems>();
        playerUI = GameObject.FindObjectOfType<PlayerUI>();
        batteryBar = GameObject.Find("FlashlightPowerBar").GetComponent<Image>();
        playerLight = GameObject.Find("PlayerLight").GetComponent<Light>();
        anim = GetComponent<Animator>();
        spotlight = light.GetComponent<Light>();
        audio = GetComponent<AudioSource>();
        UpdateBatteryBar();

        startReloadingTime = reloadingTime;
        startFlashLightIntensity = spotlight.intensity;
        startPlayerLightIntensity = playerLight.intensity;

        canUseFlashlight = (playerItems.batteryCount > 0) ? true : false;


    }

    // Update is called once per frame
    private void Update()
    {

        if(canUseFlashlight && playerItems.batteryCount > 0 && !playerUI.imageIsActive)
        {
            if (Input.GetKeyDown(KeyCode.R) && !reloadingFlashLight)
            {
                canUseFlashlight = (playerItems.batteryCount > 0) ? true : false;
                playerItems.batteryCount--;
                playerUI.UpdateBatteryCount();
                reloadingFlashLight = true;
            }
        }
        if (reloadingFlashLight)
        {
            reloadingTime -= Time.deltaTime;

            if (turnedOn == true)
            {
                spotlight.enabled = false;
                anim.SetTrigger("on-off");
                turnedOn = false;
                audio.Play();
            }
            if (reloadingTime <= 0)
            {
                ReloadFlashLight();
                reloadingTime = startReloadingTime;
            }
        }

    


        //On-off controlls
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (turnedOn == true)
            {
                spotlight.enabled = false;
                anim.SetTrigger("on-off");
                turnedOn = false;
                audio.Play();
            }
            else
            {
                if (!reloadingFlashLight)
                {
                    spotlight.enabled = true;
                    anim.SetTrigger("on-off");
                    turnedOn = true;
                    audio.Play();
                }

            }
        }
        
        //On-off controlls end

        // WALK ANIMATION CONTROLS
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
        if (isWalking)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }

        // Spadek wytrzymałości latarki
        if (turnedOn && Time.time >= nextBatteryDrainTime)
        {
            nextBatteryDrainTime = Time.time / 3f;  //batteryDrainInterval;
            batteryLife -= batteryDrainRate;
            UpdateBatteryBar();

            // Wyłączenie latarki, gdy wytrzymałość spadnie poniżej 50%

            // Zatrzymanie wytrzymałości latarki na minimalnym poziomie
            if (batteryLife <= minBatteryLife)
            {
                batteryLife = minBatteryLife;
                UpdateBatteryBar();
            }
        }
    }
    // WALK ANIMATION CONTROLS END

    public void TurnOffLight()
    {
        if (turnedOn == true)
        {
            spotlight.enabled = false;
            anim.SetTrigger("on-off");
            turnedOn = false;
            audio.Play();
        }
    }
    public void TurnOnLight()
    {
        if (turnedOn == false)
        {
            spotlight.enabled = true;
            anim.SetTrigger("on-off");
            turnedOn = true;
            audio.Play();
        }
    }
    // Aktualizacja graficznego przedstawienia wytrzymałości latarki
    void UpdateBatteryBar()
    {
        float fillAmount = batteryLife / 100.0f;
        batteryBar.fillAmount = fillAmount;

        // Zmniejszanie jasności latarki wraz ze spadkiem wytrzymałości poniżej 50%
        if (batteryLife <= 50.0f && spotlight.enabled)
        {
            float intensity = Mathf.Lerp(0, 20, fillAmount *2); // Zmniejszanie jasności w zakresie od 0 do 8
            float intensityPlayerLight = Mathf.Lerp(0, 1, fillAmount * 2);
            playerLight.intensity = intensityPlayerLight;
            spotlight.intensity = intensity;
        }
        if (batteryLife <= 30)
        {
            enoughIntensityForBlindEnemy = false;
        }
        else
            enoughIntensityForBlindEnemy = true;
    }

    private void ReloadFlashLight()
    {
        playerLight.intensity = startPlayerLightIntensity;
        spotlight.intensity = startFlashLightIntensity;
        batteryLife = 100;
        batteryBar.fillAmount = 1;
        reloadingFlashLight = false;

        if (!turnedOn)
        {
            spotlight.enabled = true;
            anim.SetTrigger("on-off");
            turnedOn = true;
            audio.Play();
        }
    }
}