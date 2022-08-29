using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{

    public float EnergyLevel;

    private float maxEnergyLevel = 100f;
    private float energyRegenerationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnergyLevel = Mathf.Min(EnergyLevel * (1 + Time.deltaTime * energyRegenerationSpeed), maxEnergyLevel);
    }
}
