using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateRaycastInformation : MonoBehaviour
{
    [Range(1, 180)]
    public float ConeFreedomDegrees = 90;

    public Vector3 ConeDirection = Vector3.right;

    public float RayDirection;
    public float RayLength = 3;
    public float coneRadius;
    public int RaycastStepInterval = 2;

    public Transform TrackingObject;

    private Vector3 BestRaycastInfo;
    private RaycastHit RaycastInfo;

    public Maid Maid = new Maid();
    // Start is called before the first frame update
    void Start()
    {
        Maid.GiveTask(
            Runservice.RunEvery(0, (5/60F), 
                (float dt)  => {
                    updatePos();
                    RayCastUpdate();
                    return true;
                }
            )
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RayCastUpdate()
    {
         List<RaycastHit> bestFootPlacements = new List<RaycastHit>();

        float halfFOV = ConeFreedomDegrees / 2.0f;
        float coneDirection = 180;
        for (int b = 0; b < halfFOV; b += RaycastStepInterval) {
            for (int i = 0; i < coneDirection; i += RaycastStepInterval)
            {
                    Quaternion upRayRotation = Quaternion.AngleAxis(-b+ coneDirection, Quaternion.Euler(ConeDirection * i) * Vector3.forward);
                    Quaternion downRayRotation = Quaternion.AngleAxis(b + coneDirection, Quaternion.Euler(ConeDirection * i) * Vector3.forward);

                    Vector3 upRayDirection = upRayRotation * ConeDirection * RayLength;
                    Vector3 downRayDirection = downRayRotation * ConeDirection * RayLength;

                    if (Physics.Raycast(transform.position, upRayDirection, out RaycastInfo, RayLength))
                    {
                        //Debug.DrawRay(transform.position, upRayDirection, Color.yellow);
                        bestFootPlacements.Add(RaycastInfo);
                    }
                    if (Physics.Raycast(transform.position, downRayDirection, out RaycastInfo, RayLength))
                    {
                        //Debug.DrawRay(transform.position, downRayDirection, Color.yellow);
                        bestFootPlacements.Add(RaycastInfo);
                    }
              }
        }

        if (TrackingObject && bestFootPlacements[0].normal.magnitude != 0) {
            RaycastHit bestPlace = bestFootPlacements[0]; 
            for (int i = 1; i < bestFootPlacements.Count; i++) {
                RaycastHit hit = bestFootPlacements[i];

                if ((transform.position-hit.point).magnitude < (transform.position-bestPlace.point).magnitude) {
                    bestPlace = hit;
                }
            }
            Debug.DrawLine(transform.position, bestPlace.point, Color.yellow);
          TrackingObject.transform.UpdateFromCFrame(new CFrame(bestPlace.point, bestPlace.point + bestPlace.normal));
        }
    }
    public void OnDrawGizmos()
    {
        float halfFOV = ConeFreedomDegrees / 2.0f;
        float coneDirection = 180;

        for (float i = 0; i < coneDirection; i += .1F)
        {
            Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Quaternion.Euler(ConeDirection * i) * Vector3.forward);
            Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Quaternion.Euler(ConeDirection * i) * Vector3.forward);

            Vector3 upRayDirection = upRayRotation * ConeDirection * RayLength;
            Vector3 downRayDirection = downRayRotation * ConeDirection * RayLength;

            Gizmos.color = new Color(1, 0, 0, .05F);

            Gizmos.DrawRay(transform.position, upRayDirection);
            Gizmos.DrawRay(transform.position, downRayDirection);
            Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
        }

        for (int b = 0; b < halfFOV; b += RaycastStepInterval) {
            float fovInterval = halfFOV - b;
            for (int i = 0; i < coneDirection; i += RaycastStepInterval)
            {
                    Quaternion upRayRotation = Quaternion.AngleAxis(-fovInterval + coneDirection, Quaternion.Euler(ConeDirection * i) * Vector3.forward);
                    Quaternion downRayRotation = Quaternion.AngleAxis(fovInterval + coneDirection, Quaternion.Euler(ConeDirection * i) * Vector3.forward);

                    Vector3 upRayDirection = upRayRotation * ConeDirection * RayLength;
                    Vector3 downRayDirection = downRayRotation * ConeDirection * RayLength;

                    Gizmos.color = new Color(0, 1, 0, .03F);

                    Gizmos.DrawRay(transform.position, upRayDirection);
                    Gizmos.DrawRay(transform.position, downRayDirection);
                    Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
              }
        }
    }
    public void updatePos()
    {
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }
}
