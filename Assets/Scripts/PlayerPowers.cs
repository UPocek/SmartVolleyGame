using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    [SerializeField]
    private float powerUpDuration;

    public void StartPowerUp(string name)
    {
        if (name.StartsWith("PowerUpEnergy"))
        {
            IncreaseEnergy();
        }
        else if (name.StartsWith("PowerUpSpike"))
        {
            IncreaseSpike();
        }
        else if (name.StartsWith("PowerUpSpeed"))
        {
            IncreaseSpeed();
        }
    }

    void IncreaseEnergy()
    {
        GetComponent<Energy>().EnergyLevel += 50f;
    }

    void IncreaseSpike()
    {
        GameObject.Find("Ball").GetComponent<BallMovement>().spikePower *= 1.5f;
        StartCoroutine(DecreaseSpike());
    }

    void IncreaseSpeed()
    {
        GetComponent<PlayerMovement>().speed *= 1.5f;
        StartCoroutine(DecreaseSpeed());
    }

    IEnumerator DecreaseSpike()
    {
        yield return new WaitForSeconds(powerUpDuration);
        GameObject.Find("Ball").GetComponent<BallMovement>().spikePower /= 1.5f;
    }

    IEnumerator DecreaseSpeed()
    {
        yield return new WaitForSeconds(powerUpDuration);
        GetComponent<PlayerMovement>().speed /= 1.5f;
    }
}
