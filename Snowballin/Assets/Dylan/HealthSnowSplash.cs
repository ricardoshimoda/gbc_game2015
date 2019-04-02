using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSnowSplash : MonoBehaviour
{
    [Range(0, 100)]
    public float health = 100;
    public float hitTimer;
    public Image one, two, three;
    private float a1, a2, a3;
    private float healthMissing;
    private float hitTime = 4;
    private bool regenerate;

    
    void Update()
    {
        if (health == 100)
        {
            one.gameObject.SetActive(false);
            two.gameObject.SetActive(false);
            three.gameObject.SetActive(false);
        }
        else if (health <= 100 && health > 66) // one third 
        {
            one.gameObject.SetActive(true);
            healthMissing = 100 - health;
            a1 = healthMissing / 33;
            Color tempColor = one.color;
            tempColor.a = a1;
            one.color = tempColor;
            two.gameObject.SetActive(false);
        }
        else if (health <= 66 && health > 33) // two thirds
        {
            two.gameObject.SetActive(true);
            healthMissing = 66 - health;
            a2 = healthMissing / 33;
            Color tempColor = two.color;
            tempColor.a = a2;
            two.color = tempColor;
            three.gameObject.SetActive(false);
        }
        else if (health <= 33) // three thirds
        {
            three.gameObject.SetActive(true);
            healthMissing = 33 - health;
            a3 = healthMissing / 33;
            Color tempColor = three.color;
            tempColor.a = a3;
            three.color = tempColor;
        }

        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }
        if (hitTimer <= 0)
        {
            regenerate = true;
            hitTimer = -1f;
        }
        if (regenerate && health < 100)
            health++;

        if(health <=0)
        {
            health = 100;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy Snowball"))
        {
            regenerate = false;
            health -= 15;
            hitTimer = hitTime;
        }
    }
}
