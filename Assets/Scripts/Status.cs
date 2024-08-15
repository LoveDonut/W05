using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Status")]
    //about player's max running time
    [SerializeField] float maxStamina;
    [SerializeField] float staminaDecreaseRate;

    //about player's vision size
    [SerializeField] float maxMentality;
    [SerializeField] float mentalityDecreaseRate;

    float currentStamina;
    float currentMentality;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DecreaseMentalityByTime();
    }

    void DecreaseMentalityByTime()
    {
        currentMentality -= mentalityDecreaseRate * Time.deltaTime;
        currentMentality = Mathf.Clamp(currentMentality, 0f, maxMentality);
    }

    public void StartDecreaseStamina()
    {
        StartCoroutine(DecreaseStamina());
    }

    public void StartRecoverStamina()
    {
//        StartCoroutine
    }

    IEnumerator DecreaseStamina()
    {
        while (currentStamina > 0f)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            yield return new WaitForEndOfFrame();
        }
    }
}
