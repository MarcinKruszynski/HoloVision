using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public static int health;


    Text text;


    void Awake ()
    {
        text = GetComponent <Text> ();
        health = 100;
    }


    void Update ()
    {
        text.text = "Health: " + health;        
    }    
}
