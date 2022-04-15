using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObs : MonoBehaviour
{
    public float step;
    public int movementType;
    public float minValue;
    public float maxValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(movementType == 0)
        {
            transform.position = new Vector3(transform.position.x + step, transform.position.y, transform.position.z);

            if((transform.position.x <= (minValue + 1f)) || (transform.position.x >= (maxValue + 1f)))
            {
                step = -step;
            }
        }
        else if(movementType == 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + step, transform.position.z);

            if((transform.position.y <= (minValue + 1f)) || (transform.position.y >= (maxValue + 1f)))
            {
                step = -step;
            }
        }
        else if(movementType == 2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + step);

            if((transform.position.z <= (minValue + 1f)) || (transform.position.z >= (maxValue + 1f)))
            {
                step = -step;
            }
        }
    }
}
