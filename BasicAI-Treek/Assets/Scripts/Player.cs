//////////////////////////////////////////////
//Assignment/Lab/Project: BasicAI_Treek
//Name: Ahmed Treek
//Section: SGD.213.0021
//Instructor: Aurore Locklear
//Date: 4/11/2024
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement & Camera")]
    public Camera playerCamera;
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    [Header("Shooting")]
    private float shootDelay = 0.5f;
    private float lastShotTime;
    [SerializeField] private Transform shotLocation;
    [SerializeField] private GameObject bulletPrefab, resultPanel;
    public int enemiesDefeated = 4;

    [Header("UI & health Related")]
    [SerializeField] private TMP_Text healthTxt, resultTxt;
    public TMP_Text enemiesLeft;
    private int health = 100;


    [Header("Script Related")]
    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>(); //finds character controller component
        Cursor.lockState = CursorLockMode.Locked; //locks and hides cursor
        Cursor.visible = false; 
        resultPanel.SetActive(false);
        healthTxt.text = "Health: " + health;
        enemiesLeft.text = "Enemies Left: " + enemiesDefeated; //set up UI
    }

    void Update()
    {

        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion


        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

        #region Handles Shooting

        if (Time.time - lastShotTime >= shootDelay) // calculates the time between in game, the last time the player shot, and checks if its greater or equal to the shot delay
        {
            if (Input.GetKey(KeyCode.Space)) //when the player presses space shoots out a laser from the objecy pool
            {
                var bullets = Instantiate(bulletPrefab, shotLocation.position, shotLocation.rotation);
                bullets.GetComponent<Rigidbody>().velocity = transform.forward * 100f;
                Destroy(bullets, 4f);
                lastShotTime = Time.time; //sets the last shot time to when the player shot
            }
        }
        #endregion

        #region Handles Outcome And Mouse

        if (resultPanel.activeSelf == true)
        {
            Cursor.lockState = CursorLockMode.None; //if the result panel is active show and unlock cursor
            Cursor.visible = true;
            Time.timeScale = 0f; //pause game
            lookSpeed = 0f; //set camera sens to 0 to avoid camera moving
        }
        else
        {
            Time.timeScale = 1f; //unpause game
            lookSpeed = 2f;
        }

        if (enemiesDefeated <= 0)
        {
            resultPanel.SetActive(true); //if enemies are defeated the player wins
            resultTxt.text = "You Won!";
        }
        else if(health <= 0)
        {
            resultPanel.SetActive(true); //if the player dies the game is over
            resultTxt.text = "You Lose!";
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            health -= 20;
            health = Mathf.Clamp(health, 0, 100);
            healthTxt.text = "Health: " + health;
        }
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
