using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
    //Camera mainCamera;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;        
        playerHealth = player.GetComponent <PlayerHealth> ();
        //mainCamera = Camera.main;
        //playerHealth = mainCamera.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }


    void Update ()
    {
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            nav.SetDestination (player.position);

            //RaycastHit hit;
            //var headPosition = mainCamera.transform.position;
            //var gazeDirection = mainCamera.transform.forward;
            //var downDirection = new Vector3(0, -1, 0);

            //if (Physics.Raycast(headPosition, downDirection, out hit))
            //{
            //    nav.SetDestination(hit.point);
            //}
        }
        else
        {
            nav.enabled = false;
        }
    }
}
