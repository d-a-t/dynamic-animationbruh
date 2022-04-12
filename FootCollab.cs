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

    public void RaycastAll(Vector3 direction) {
        RaycastLeftArm(direction);
        RaycastLeftFoot(direction);

        RaycastRightArm(direction);
        RaycastRightFoot(direction);
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

    // Update is called once per frame
    void Update()
    {
        if (Input.move == Vector2.zero) {
            RaycastAll(Vector3.down);

            if (StillFootPlantingUpdate.Destroyed) {
                StillFootPlantingUpdate = Runservice.RunEvery
                (
                    Global.RunservicePriority.RenderStep.Character, 1, 
                    (float dt) => {
                        RaycastAll(Vector3.down);
                        return true;
                    }
                );
            }
        } else {
            StillFootPlantingUpdate.Destroy();
        }
    }
}
