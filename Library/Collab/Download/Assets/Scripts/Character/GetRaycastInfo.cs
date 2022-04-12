using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRaycastInfo : MonoBehaviour
{
    public GameObject offsetCheckOne;
    public GameObject offsetCheckTwo;
    public GameObject leftFootRef;
    public GameObject rightFootRef;

    public RaycastHit groundInfo;

    public Vector3 perpendicularCheck;

    public Vector3 slopeOne;
    public Vector3 slopeTwo;
    public float distanceOne;
    public float distanceTwo;

    // Start is called before the first frame update
    void Start()
    {
        perpendicularCheck = Vector3.Normalize(offsetCheckOne.transform.position - offsetCheckTwo.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        CheckRaycast();
    }

    void CheckRaycast()
    {
        if(Physics.Raycast(offsetCheckOne.transform.position, Vector3.down, out groundInfo, Mathf.Infinity))
        {
            Debug.DrawRay(offsetCheckOne.transform.position, Vector3.down * groundInfo.distance, Color.red);
            distanceOne = Vector3.Distance(groundInfo.point, leftFootRef.transform.position);
            slopeOne = Vector3.Cross(groundInfo.normal, perpendicularCheck).normalized;

        }

        if(Physics.Raycast(offsetCheckTwo.transform.position, Vector3.down, out groundInfo, Mathf.Infinity))
        {
            Debug.DrawRay(offsetCheckTwo.transform.position, Vector3.down * groundInfo.distance, Color.red);
            distanceTwo = Vector3.Distance(groundInfo.point, leftFootRef.transform.position);
            slopeOne = Vector3.Cross(groundInfo.normal, perpendicularCheck).normalized;
        }
    }
}
