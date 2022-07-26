using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int max_health = 100;
    int current_health;
    void Start()
    {
        current_health = max_health;
    }

    public void take_damage (int damage) {
        current_health = current_health - damage;

        if(current_health <= 0) {
            die();
        }
    }

    void die() {
        print("DIE!");
    }
}
