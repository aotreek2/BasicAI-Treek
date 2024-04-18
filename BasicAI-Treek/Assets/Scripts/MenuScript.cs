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
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
   public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("MainGame"); //loads scene 
    }
    public void OnQuitButtonClick()
    {
       Application.Quit();
    }
}
