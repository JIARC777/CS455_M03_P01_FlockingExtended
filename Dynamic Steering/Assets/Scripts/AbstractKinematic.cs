using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractKinematic : MonoBehaviour
{
    public Vector3 linearVelocity;
    public float angularVelocity; // in degrees
    public GameObject target;
    public float maxSpeed = 40f;
    protected SteeringOutput mySteering;
    //public dynamicSteering control;
   // public bool DistanceActivation;
   // public string movementType;
  //  public string rotationType;
  //  public bool avoidObstacles;

    // Only For Seperation or path following
    public AbstractKinematic[] seperationObstacles;
    public AbstractKinematic[] collisionAvoidance;
    public virtual void Update()
    {
        transform.position += linearVelocity * Time.deltaTime;
        // adding angular velocity to current transform rotation y component
        if (float.IsNaN(angularVelocity))
            angularVelocity = 0;
        transform.eulerAngles += new Vector3(0, angularVelocity * Time.deltaTime, 0);
        if (mySteering != null)
        {
            linearVelocity += mySteering.linear * Time.deltaTime;
            if (linearVelocity.magnitude > maxSpeed)
            {
                linearVelocity.Normalize();
                linearVelocity *= maxSpeed;
            }
            angularVelocity += mySteering.angular * Time.deltaTime;

        }
        
    }
}
