using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DitzelGames.FastIK;
public class FootCollab : MonoBehaviour
{
	public CharacterController Controller;
	public StarterAssets.StarterAssetsInputs Input;

    [Header("Arms")]
    public FootPlanting RightArmPlant;
    public FootPlanting LeftArmPlant;
    
    [Header("Legs")]
    public FootPlanting RightLegPlant;
    public FootPlanting LeftLegPlant;

    public void RotateDownUntilFound(FootPlanting foot) {
        Vector3 orgPos = foot.TrackingObject.transform.position;

        float step = 0;
        while (foot.TrackingObject.transform.position == orgPos) {
            step += .05F;
            foot.ConeDirection = (new Vector3(0,-Mathf.Cos(step), Mathf.Sin(step))).normalized;
            foot.RayCastUpdate();
        }
    }

    public void RaycastAll(Vector3 direction) {
        RaycastLeftArm(direction);
        RaycastLeftFoot(direction);

        RaycastRightArm(direction);
        RaycastRightFoot(direction);
    }

    public void ChangePlantingPriority(FootPlantingPriority priority) {
        RightArmPlant.PlantingPriority = priority;
        LeftArmPlant.PlantingPriority = priority;
        RightLegPlant.PlantingPriority = priority;
        LeftLegPlant.PlantingPriority = priority;   
    }

    public void ChangeUpdatePriority(FootUpdatingPriority priority) {
        RightArmPlant.UpdatePriority = priority;
        LeftArmPlant.UpdatePriority = priority;
        RightLegPlant.UpdatePriority = priority;
        LeftLegPlant.UpdatePriority = priority;   
    }

    public void RaycastLeftArm(Vector3 direction) {
        LeftArmPlant.ConeDirection = direction.normalized;
        LeftArmPlant.RayCastUpdate();
    }

    public void RaycastLeftFoot(Vector3 direction) {
        LeftLegPlant.ConeDirection = direction.normalized;
        LeftLegPlant.RayCastUpdate();
    }

    public void RaycastRightArm(Vector3 direction) {
        RightArmPlant.ConeDirection = direction.normalized;
        RightArmPlant.RayCastUpdate();
    }

    public void RaycastRightFoot(Vector3 direction) {
        RightLegPlant.ConeDirection = direction.normalized;
        RightLegPlant.RayCastUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        RightArmPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;
        RightLegPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;

        LeftArmPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;
        LeftLegPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;
    }

    private Listener<float> StillFootPlantingUpdate;
    private Listener<float> MoveFootPlantingUpdate;

    // Update is called once per frame
    void Update()
    {
        if (Input.move == Vector2.zero) {
            if (StillFootPlantingUpdate == null || StillFootPlantingUpdate.Destroyed) {
                MoveFootPlantingUpdate?.Destroy();

                StillFootPlantingUpdate = Runservice.RunEvery
                (
                    Global.RunservicePriority.RenderStep.Character, (5F/60F), 
                    (float dt) => {
                        RaycastAll(Vector3.down);
                        return true;
                    }
                );
            }
        } else {
            ChangeUpdatePriority(FootUpdatingPriority.Manual);

            RightArmPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;
            RightLegPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;

            LeftArmPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;
            LeftLegPlant.ConeDirection = (new Vector3(0, -1, 1)).normalized;

            if (MoveFootPlantingUpdate == null || MoveFootPlantingUpdate.Destroyed) {
                StillFootPlantingUpdate?.Destroy();
                RightArmPlant.RayCastUpdate();
                RightLegPlant.RayCastUpdate();
                MoveFootPlantingUpdate = Runservice.RunEvery(0, (RightArmPlant.RaycastFrameUpdateInterval / 50F),
                (float dt) =>
                    { 
                        if ((RightArmPlant.transform.position - RightArmPlant.TrackingObject.position).magnitude > RightArmPlant.RayLength) {
                            LeftArmPlant.RayCastUpdate();
                            LeftLegPlant.RayCastUpdate();
                        }
                        if ((LeftArmPlant.transform.position - LeftArmPlant.TrackingObject.position).magnitude > LeftArmPlant.RayLength) {
                            RightArmPlant.RayCastUpdate();
                            RightLegPlant.RayCastUpdate();
                        }
                        return true;
                    }
                );
            }
        }
    }
}
