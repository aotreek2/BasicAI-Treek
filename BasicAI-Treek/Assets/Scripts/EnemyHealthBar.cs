//////////////////////////////////////////////
//Assignment/Lab/Project: BasicAI_Treek
//Name: Ahmed Treek
//Section: SGD.213.0021
//Instructor: Aurore Locklear
//Date: 4/11/2024
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider; 
    [SerializeField] private Camera mainCamera;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue; //sets the slider value 
        mainCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        transform.rotation = mainCamera.transform.rotation;
    }
}
