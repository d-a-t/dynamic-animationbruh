using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FootPlantingPriority {
    Closest, Farthest
}
public enum FootUpdatingPriority {
    Always, OnlyWhenUnreachable
}

public class FootPlanting : MonoBehaviour
{
    [Range(1, 180)]
    public float ConeFreedomDegrees = 90;

    public Vector3 ConeDirection = Vector3.right;

    public float RayDirection;
    public float RayLength = 3;

    [Range(2, 5)]
    public int RaycastStepInterval = 2;

    public int RaycastFrameUpdateInterval = 2;
    public bool InterpolatePosition = true;
    public FootPlantingPriority Priority = FootPlantingPriority.Closest;
    public FootUpdatingPriority UpdatePriority = FootUpdatingPriority.Always;


    public Transform TrackingObject;

    private RaycastHit BestRaycastInfo;
    private RaycastHit RaycastInfo;

    public Maid Maid = new Maid();

    public void Start()
    {
        Maid.GiveTask(
            Runservice.RunEvery(0, (RaycastFrameUpdateInterval / 50F),
                (float dt) =>
                {
                    if (UpdatePriority == FootUpdatingPriority.OnlyWhenUnreachable) {
                        if ((transform.position - TrackingObject.position).magnitude < RayLength) {
                            return true;
                        }
                    }
                    RayCastUpdate();
                    
                    return true;
                }
            )
        );
    }

    public void OnDestroy()
    {
        Maid.Dispose();
    }

    public void RayCastUpdate()
    {
        List<RaycastHit> bestFootPlacements = new List<RaycastHit>();

        float halfFOV = ConeFreedomDegrees / 2.0f;
        float coneDirection = 360;

        CFrame baseCFrame = transform.GetCFrame() * new CFrame(ConeDirection.normalized * RayLength);
        for (float i = 0; i < coneDirection; i += RaycastStepInterval / 2)
        {
            CFrame aroundCFrame = baseCFrame * CFrame.FromAxisAngle(ConeDirection.normalized, i * Mathf.Deg2Rad) * new CFrame(Mathf.Tan(ConeFreedomDegrees * Mathf.Deg2Rad) * RayLength);
            Vector3 rayDirection = (aroundCFrame.p - transform.position).normalized;
            if (Physics.Raycast(transform.position, rayDirection, out RaycastInfo, RayLength))
            {
                Debug.DrawRay(transform.position, rayDirection, Color.yellow);
                bestFootPlacements.Add(RaycastInfo);
            }
        }

        for (int b = RaycastStepInterval; b < ConeFreedomDegrees; b += RaycastStepInterval)
        {
            float fovInterval = halfFOV - b;
            for (int i = 0; i < coneDirection; i += RaycastStepInterval)
            {
                CFrame aroundCFrame = baseCFrame * CFrame.FromAxisAngle(ConeDirection.normalized, i * Mathf.Deg2Rad) * new CFrame(Mathf.Tan(b * Mathf.Deg2Rad) * RayLength);
                Vector3 rayDirection = (aroundCFrame.p - transform.position).normalized;
                if (Physics.Raycast(transform.position, rayDirection, out RaycastInfo, RayLength))
                {
                    Debug.DrawRay(transform.position, rayDirection, Color.blue);
                    bestFootPlacements.Add(RaycastInfo);
                }
            }
        }

        if (TrackingObject && bestFootPlacements.Count > 0 && bestFootPlacements[0].normal.magnitude != 0)
        {
            RaycastHit bestPlace = bestFootPlacements[0];
            for (int i = 1; i < bestFootPlacements.Count; i++)
            {
                RaycastHit hit = bestFootPlacements[i];

                if (Priority == FootPlantingPriority.Closest) {
                    if ((transform.position - hit.point).magnitude < (transform.position - bestPlace.point).magnitude)
                    {
                        bestPlace = hit;
                    }
                } else if (Priority == FootPlantingPriority.Farthest) {
                    if ((transform.position - hit.point).magnitude > (transform.position - bestPlace.point).magnitude)
                    {
                        bestPlace = hit;
                    }
                }
            }
            Debug.DrawLine(transform.position, bestPlace.point, Color.yellow);

            BestRaycastInfo = bestPlace;
        }
    }

    private int frameCounter = 0;
    public void RaycastPointInterpolate(float dt)
    {
        TrackingObject.transform.UpdateFromCFrame(TrackingObject.transform.GetCFrame().Lerp(new CFrame(BestRaycastInfo.point, BestRaycastInfo.point + BestRaycastInfo.normal), dt * RaycastStepInterval));
    }

    public void Update()
    {
        if (InterpolatePosition) {
            RaycastPointInterpolate(Time.deltaTime);
        } else {
            TrackingObject.transform.UpdateFromCFrame(new CFrame(BestRaycastInfo.point, BestRaycastInfo.point + BestRaycastInfo.normal));
        }
    }

	public void OnDrawGizmos()
    {
        float halfFOV = ConeFreedomDegrees / 2.0f;
        float coneDirection = 360;

        CFrame baseCFrame = transform.GetCFrame() * new CFrame(ConeDirection.normalized  * RayLength);

		for (float i = 0; i < coneDirection; i += RaycastStepInterval / 2)
        {
            /*
            Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Quaternion.Euler(ConeDirection * i) * transform.forward);
            Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Quaternion.Euler(ConeDirection * i) * transform.forward);

            Vector3 upRayDirection = upRayRotation * ConeDirection * RayLength;
            Vector3 downRayDirection = downRayRotation * ConeDirection * RayLength;

            Gizmos.color = new Color(1, 0, 0, .05F);

            Gizmos.DrawRay(transform.position, upRayDirection);
            Gizmos.DrawRay(transform.position, downRayDirection);
            Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
            */
            Gizmos.color = new Color(1, 0, 0, .05F);
            CFrame aroundCFrame = baseCFrame * CFrame.FromAxisAngle(ConeDirection.normalized, i * Mathf.Deg2Rad) * new CFrame(Mathf.Tan(ConeFreedomDegrees * Mathf.Deg2Rad) * RayLength);
            Gizmos.DrawRay(transform.position, aroundCFrame.p - transform.position);
            // aroundCFrame = baseCFrame * CFrame.FromAxisAngle(Quaternion.Euler(ConeDirection * i) * baseCFrame.lookVector, halfFOV - coneDirection) * new CFrame(ConeDirection * RayLength);
            //Gizmos.DrawRay(transform.position, aroundCFrame.p - transform.position);
        }

        for (int b = RaycastStepInterval; b < ConeFreedomDegrees; b += RaycastStepInterval)
        {
            float fovInterval = halfFOV - b;
            for (int i = 0; i < coneDirection; i += RaycastStepInterval)
            {
                /*
                    Quaternion upRayRotation = Quaternion.AngleAxis(-fovInterval + coneDirection, Quaternion.Euler(ConeDirection * i) * Vector3.forward);
                    Quaternion downRayRotation = Quaternion.AngleAxis(fovInterval + coneDirection, Quaternion.Euler(ConeDirection * i) * Vector3.forward);

                    Vector3 upRayDirection = upRayRotation * ConeDirection * RayLength;
                    Vector3 downRayDirection = downRayRotation * ConeDirection * RayLength;

                    Gizmos.color = new Color(0, 1, 0, .03F);

                    Gizmos.DrawRay(transform.position, upRayDirection);
                    Gizmos.DrawRay(transform.position, downRayDirection);
                    Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
*/
                Gizmos.color = new Color(0, 1, 0, .1F);
                CFrame aroundCFrame = baseCFrame * CFrame.FromAxisAngle(ConeDirection.normalized, i * Mathf.Deg2Rad) * new CFrame(Mathf.Tan(b * Mathf.Deg2Rad) * RayLength);
                Gizmos.DrawRay(transform.position, aroundCFrame.p - transform.position);
            }
        }
    }
}