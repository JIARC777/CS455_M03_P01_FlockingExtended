using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance
{
    public Kinematic ai;
    //float maxAcceleration = 10f;
    public AbstractKinematic[] targets;
    float radius = 10f; //collision radius

    public SteeringOutput GetSteering()
    {
        //check to see if collision is coming
        float shortestTime = float.PositiveInfinity;
        Kinematic firstTarget = null;
        float firstMinSeperation = float.PositiveInfinity;
        float firstDistance = float.PositiveInfinity;
        Vector3 firstRelativePos = Vector3.zero;
        Vector3 firstRelativeVel = Vector3.zero;

        Vector3 relativePos;
        foreach (Kinematic target in targets)
        {
            // calculate time to collision (time of closest approach
            relativePos = target.transform.position - ai.transform.position;
            Vector3 relativeVel = ai.linearVelocity - target.linearVelocity;
            float relativeSpeed = relativeVel.magnitude;
            float timeToCollision = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
            // is it close enough to care
            float distance = relativePos.magnitude;
            float minSeperation = distance - relativeSpeed * timeToCollision;
            if (minSeperation > 2 * radius)
                continue;
            //check if its the shortest
            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;
                firstTarget = target;
                firstMinSeperation = minSeperation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }
        if (firstTarget == null)
        {
           // Debug.Log("No Collisions Detected");
            return null;
        }
        
        // Do we need this??? 
        if (firstMinSeperation <= 0 || firstDistance < 2 * radius)
        {
             relativePos = firstTarget.transform.position - ai.transform.position;
        } else
        {
             relativePos = firstRelativePos + firstRelativeVel * shortestTime;
        }
        relativePos.Normalize();


        SteeringOutput result = new SteeringOutput();
        //result.linear = relativePos;

        Vector3 aiVel = ai.linearVelocity;
        aiVel.Normalize();
        Vector3 targetVel = firstTarget.linearVelocity;
        targetVel.Normalize();

        float dotResult = Vector3.Dot(aiVel, targetVel);
        if (dotResult < -0.9)
            result.linear = -firstTarget.transform.right;
        else
        {
            result.linear = -firstTarget.linearVelocity;
        }
            
        return result;


    }
}


public class ObstacleAvoidance: Seek
{
    float avoidDistance = 10;
    float lookAhead = 15;
    
    //float maxAvoidAcceleration = 4f;

    public override SteeringOutput GetSteering()
    {
        RaycastHit hit;
        SteeringOutput result = new SteeringOutput();
        if (Physics.Raycast(ai.transform.position, ai.linearVelocity.normalized, out hit, lookAhead)) {
            Debug.Log("Impending Collision Detected");
            target = new GameObject();
            Debug.DrawRay(ai.transform.position, ai.linearVelocity.normalized * hit.distance);
            target.transform.position = hit.point + (hit.normal * avoidDistance);
            Debug.DrawRay(hit.point, (hit.normal * avoidDistance));
            return base.GetSteering();
        }
        else
        {
            return null;
        }
    }

}