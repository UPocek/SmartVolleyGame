using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    public float energyLevel;

    [SerializeField]
    private Slider energySlider;
    [HideInInspector]
    public readonly float maxEnergyLevel = 100f;

    private readonly float energyRegenerationSpeed = 2f;

    public float walkCost = 2f;
    public float jumpCost = 10f;
    public float spikeCost = 20f;

    // Start is called before the first frame update
    void Start()
    {
        energySlider.maxValue = maxEnergyLevel;
        energySlider.value = energyLevel;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        energyLevel += energyRegenerationSpeed * Time.deltaTime;
        energyLevel = Mathf.Min(energyLevel, maxEnergyLevel);

        energySlider.value = energyLevel;
    }

    public void DecreaseEnergyWalk()
    {
        energyLevel -= walkCost * Time.deltaTime;
    }

    public void DecreaseEnergyJump()
    {
        energyLevel -= jumpCost;
    }

    public void DecreaseEnergySpike()
    {
        energyLevel -= spikeCost;
    }
}
