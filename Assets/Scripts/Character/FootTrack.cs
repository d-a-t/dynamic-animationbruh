using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class FootTrack : MonoBehaviour
{
    public Vector3 TrackToPoint = new Vector3();
    public FastIKFabric IK;

    public Transform DebugTransform;

    public void Awake() {

    }

    public void Start() {
        DebugTransform = (new GameObject()).transform;
        IK.Target = DebugTransform;
    }

    // Update is called once per frame
    void FixedUpdate() {
        //DebugTransform.transform.position = TrackToPoint;
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(DebugTransform.transform.position, .05F);
    }
}
