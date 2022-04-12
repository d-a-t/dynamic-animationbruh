using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;
public class FootCollab : MonoBehaviour
{
    [Header("Arms")]
    public FootPlanting RightArmPlant;
    public FootPlanting LeftArmPlant;
    
    [Header("Legs")]
    public FootPlanting RightLegPlant;
    public FootPlanting LeftLegPlant;


    // Start is called before the first frame update
    void Start()
    {
        RightArmPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;
        RightLegPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;

        LeftArmPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;
        LeftLegPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;
    }


    public float step = 0;
    // Update is called once per frame
    void Update()
    {
        step += .05F;


    }
}
