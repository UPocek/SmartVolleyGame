using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{

    public float EnergyLevel;

    [SerializeField]
    private Slider energySlider;
    private readonly float maxEnergyLevel = 100f;
    private readonly float energyRegenerationSpeed = 2f;

    public float walkCost = 2f;
    public float jumpCost = 10f;
    public float spikeCost = 20f;

    // Start is called before the first frame update
    void Start()
    {
        energySlider.maxValue = maxEnergyLevel;
        energySlider.value = EnergyLevel;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        EnergyLevel += Time.deltaTime * energyRegenerationSpeed;
        EnergyLevel = Mathf.Min(EnergyLevel, maxEnergyLevel);

        energySlider.value = EnergyLevel;
    }

    public void DecreaseEnergyWalk()
    {
        EnergyLevel -= walkCost * Time.deltaTime;
    }

    public void DecreaseEnergyJump()
    {
        EnergyLevel -= jumpCost;
    }

    public void DecreaseEnergySpike()
    {
        EnergyLevel -= spikeCost;
    }
}
