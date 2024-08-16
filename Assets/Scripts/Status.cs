using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Status : MonoBehaviour
{
    PlayerController playerController;

    [Header("Status")]
    //about player's max running time
    [SerializeField] float maxStamina = 10f;
    [SerializeField] float staminaDecreaseRate = 2f;
    [SerializeField] float staminaCoolTime = 2f;

    //about player's vision size
    [SerializeField] float mentalityDecreaseRate = 0.1f;
    [SerializeField] PostProcessVolume postProcessVolume;
    Vignette vignette;

    float currentStamina;
    float elapsedTime = 0f;

    // Can't running when stamina becomes 0
    public bool CanRunning { get; private set; }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        currentStamina = maxStamina;
        CanRunning = true;

        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DecreaseSightByTime();
        ChangeStamina();
    }

    void DecreaseSightByTime()
    {
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Clamp(vignette.intensity.value + mentalityDecreaseRate * Time.deltaTime, 0f, 1f);
        }
    }

    public void IncreaseSight()
    {
        if (vignette != null)
        {
            vignette.intensity.value = 0f;
        }
    }

    public void ChangeStamina()
    {
        if(CanRunning && playerController.isRunning) // if player can && want running
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            if (currentStamina <= 0f)
            {
                elapsedTime = 0f;
                CanRunning = false;
            }
        }
        else if (!playerController.isRunning) // if player don't running
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= staminaCoolTime)
            {
                CanRunning = true;
                currentStamina += staminaDecreaseRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            }
        }
    }
}
