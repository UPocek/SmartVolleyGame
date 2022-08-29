using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    public void StartPowerUp(string name)
    {
        if (name == "PowerUpEnergy")
        {
            IncreaseEnergy();
        }
        else if (name == "PowerUpSpike")
        {
            IncreaseSpike();
        }
        else if (name == "PowerUpSpeed")
        {
            IncreaseSpeed();
        }
    }

    void IncreaseEnergy()
    {
        GetComponent<Energy>().EnergyLevel += 50;
    }

    void IncreaseSpike()
    {
        //GetComponent<PlayerMovement>().spikePower *= 2;
    }

    void IncreaseSpeed()
    {
        GetComponent<PlayerMovement>().speed *= 2;
    }
}
