//////////////////////////////////////////////
//Assignment/Lab/Project: BasicAI_Treek
//Name: Ahmed Treek
//Section: SGD.213.0021
//Instructor: Aurore Locklear
//Date: 4/11/2024
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    enum AIState { patroling, chasing } 
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform[] patrolPoints;
    private AIState currentState;
    private GameObject player;
    private float health = 100f;

    float distanceToPlayer;
    Player playerScript;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); //sets up and finds the objects/scripts on awake
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = GameObject.FindAnyObjectByType<Player>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        
    }
    void Start()
    {
        healthBar.UpdateHealthBar(health, 100f); //sets the health bar
        currentState = AIState.patroling; //sets the enemys enum state to patrol on start
        StartCoroutine("DetermineStates"); //starts the coroutine for the determine states method
    }

    IEnumerator DetermineStates()
    {
        while (gameObject.activeInHierarchy) //while enemy is active 
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); //calculates the distance to the player 

            if (distanceToPlayer < 10f) //if the distance to the player is less than 10, starts the chasing state
            {
                currentState = AIState.chasing;
            }
            else
            {
                currentState = AIState.patroling; //else enemy patrols
            }
            HandleStates(); //calls the handle states method
            yield return new WaitForSeconds(0.2f); //waits for 0.2 to start again
        }
    }

    void HandleStates()
    {
        switch(currentState) //switch statement for the current state
        {
            case AIState.patroling: //patrolling state
                agent.speed = 2.5f; //enemy moves slower
                if(!agent.pathPending && agent.remainingDistance < 1.0f) //if the enemy is close to the set patrol point path
                {
                    agent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Length)].transform.position); //finds and sets destatnation for nearest patrol point
                }
                    break;
            case AIState.chasing: //chasing state
                agent.speed = 5f; //enemy moves faster
                agent.SetDestination(player.transform.position); //sets the desination to the players position
                break;
            default:
                Debug.LogError("The agent component has not been added to the enemy"); // deafult checks for errors
                break;
        }    
    }

    public void TakeDamage(float damage)
    {
        health -= damage; //enemy takes damage
        healthBar.UpdateHealthBar(health, 100f); //sends the updated health to the health bar script
        if (health <= 0) //if the enemy dies
        {
            playerScript.enemiesDefeated -= 1; //lowers enemy defeated count
            playerScript.enemiesLeft.text = "Enemies Left: " + playerScript.enemiesDefeated; //updates UI
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("hit");
            TakeDamage(25); //if enemy is hit, take 25 damage
            Destroy(other.gameObject);
        }
    }
}
