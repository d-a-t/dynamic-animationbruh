using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;
public class FootAnimating : MonoBehaviour
{
    private RaycastHit RaycastInfo;

    public FastIKFabric FootIK;
    public Transform TrackingObject;
    public Transform Root;
    public Transform CharacterRoot;

    private float HipHeight;

    private float SineWalk(float input) {
        return Mathf.Abs(Mathf.Sin(input));
    }

    // Start is called before the first frame update
    void Start()
    {
        HipHeight = Root.transform.position.y - TrackingObject.transform.position.y;
    }

    float x = 0;
    // Update is called once per frame
    void LateUpdate()
    {
        x += .01F;
        if (x > Mathf.Deg2Rad * 120) {
            x = Mathf.Deg2Rad * 30;
        }

        if(Physics.Raycast(Root.position, ((-CharacterRoot.transform.up *  Mathf.Sin(x)) + (CharacterRoot.transform.forward * Mathf.Cos(x))).normalized, out RaycastInfo, HipHeight)) {
            Debug.DrawRay(Root.position, ((-CharacterRoot.transform.up *  Mathf.Sin(x)) + (CharacterRoot.transform.forward * Mathf.Cos(x))).normalized * RaycastInfo.distance, Color.red);            
        } else {
            x = Mathf.Deg2Rad * 30;
            return;
        }

        

        if (RaycastInfo.normal.magnitude > 0) {
            Vector3 footpos = RaycastInfo.point;

            TrackingObject.transform.UpdateFromCFrame((new CFrame(footpos, RaycastInfo.point + Vector3.Cross(RaycastInfo.normal, CharacterRoot.right))));
        }
        
        
    }
}
