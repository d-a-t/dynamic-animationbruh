using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateOffsetPos : MonoBehaviour
{
    public GameObject shoulderRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (shoulderRef.transform.position) - (shoulderRef.transform.forward * 0.5f);
    }
}
