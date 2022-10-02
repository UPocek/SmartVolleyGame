using System.Collections;
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
        GetComponent<Energy>().energyLevel += 50f;
    }

    void IncreaseSpike()
    {
        GameObject.Find("Ball").GetComponent<BallMovement>().spikePower *= 1.25f;
        StartCoroutine(DecreaseSpike());
    }

    void IncreaseSpeed()
    {
        GetComponent<VolleyAIv1>().speed *= 1.5f;
        StartCoroutine(DecreaseSpeed());
    }

    IEnumerator DecreaseSpike()
    {
        yield return new WaitForSeconds(powerUpDuration);
        GameObject.Find("Ball").GetComponent<BallMovement>().spikePower /= 1.25f;
    }

    IEnumerator DecreaseSpeed()
    {
        yield return new WaitForSeconds(powerUpDuration);
        GetComponent<VolleyAIv1>().speed /= 1.5f;
    }
}
