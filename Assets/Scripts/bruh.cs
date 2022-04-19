using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bruh : MonoBehaviour
{
    public Transform Goal;
    public GameObject robot;
    public Vector3 oj;

    public Text TimerText;
    public Text BestText;

    private float counter;
    private float best;

    // Start is called before the first frame update
    void Start()
    {
        oj = robot.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;
        TimerText.text = counter.ToString();
        if ((Goal.position - robot.transform.position).magnitude < 4) {
            best = counter;
            counter = 0;
            BestText.text = best.ToString();
            robot.transform.position = new Vector3(oj.x,oj.y,oj.z);
        }
    }
}
