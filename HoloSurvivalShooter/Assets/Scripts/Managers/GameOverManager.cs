using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;       
    public float restartDelay = 5f;

    
    float restartTimer;


    void Update()
    {
        
        if (playerHealth.currentHealth <= 0)
        {            
            restartTimer += Time.deltaTime;
            
            if (restartTimer >= restartDelay)
            {
                SceneManager.LoadScene(0);

                ScoreManager.score = 0;
                HealthManager.health = 100;
            }
        }
    }

}
