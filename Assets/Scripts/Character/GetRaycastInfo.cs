using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRaycastInfo : MonoBehaviour
{
    //gameobject references
    public GameObject offsetCheckOne;
    public GameObject offsetCheckTwo;
    public GameObject leftFootRef;
    public GameObject rightFootRef;
    public GameObject leftFootParent;
    public GameObject rightFootParent;

    //referenced for raycasts
    public RaycastHit groundInfo;

    //iterates at start to get cross product reference
    public Vector3 perpendicularCheck;

    //data collection to update states of animation
    public Vector3 slopeOne;
    public Vector3 slopeTwo;
    public Quaternion footRotationOne;
    public Quaternion footRotationTwo;
    public float distanceOne;
    public float distanceTwo;
    public float maxDistance = 5f;

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
            //update variables and draw debug
            Debug.DrawRay(offsetCheckOne.transform.position, Vector3.down * groundInfo.distance, Color.red);
            distanceOne = Vector3.Distance(groundInfo.point, leftFootRef.transform.position);
            slopeOne = Vector3.Cross(groundInfo.normal, perpendicularCheck).normalized;
            
            //something something update foot rotation
            /*footRotationOne = Quaternion.LookRotation(slopeOne, Vector3.up);
            leftFootRef.transform.rotation = footRotationOne;*/

            //move position of leg if far enough away from raycast
            if(distanceOne >= maxDistance)
            {
                Vector3.MoveTowards(leftFootParent.transform.position, groundInfo.point, 0.5f);
            }
        }

        if(Physics.Raycast(offsetCheckTwo.transform.position, Vector3.down, out groundInfo, Mathf.Infinity))
        {
            Debug.DrawRay(offsetCheckTwo.transform.position, Vector3.down * groundInfo.distance, Color.red);
            distanceTwo = Vector3.Distance(groundInfo.point, leftFootRef.transform.position);
            slopeOne = Vector3.Cross(groundInfo.normal, perpendicularCheck).normalized;

            /*footRotationTwo = Quaternion.LookRotation(slopeTwo, Vector3.up);
            rightFootRef.transform.rotation = footRotationTwo;*/

            if(distanceTwo >= maxDistance)
            {
                Vector3.MoveTowards(rightFootParent.transform.position, groundInfo.point, 0.5f);
            }
        }
    }
}
