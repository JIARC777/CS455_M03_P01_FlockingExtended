using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorAndWeight
{
    public dynamicSteering behavior;
    public float weight;
}

public class BlendedSteering
{
    public float maxAcceleration = 1000f;
    public float maxAngularAcceleration = 5f;
    // for flocking blend seperate, arrive, and LWG
    public BehaviorAndWeight[] behaviors;
    public SteeringOutput GetSteering()
    {
        SteeringOutput result = new SteeringOutput();
        
        foreach(BehaviorAndWeight b in behaviors)
        {
            SteeringOutput s = b.behavior.GetSteering();
            //Debug.Log(s.linear);
            if (s != null)
            {
               // Debug.Log(b.behavior.GetType())
                result.linear += b.weight * s.linear;
                Debug.Log(result.linear);
                result.angular += b.weight * s.angular;
            }
        }

        result.linear = result.linear.normalized * Mathf.Min(maxAcceleration, result.linear.magnitude);
        float angularAcceleration = Mathf.Abs(result.angular);
        if ( angularAcceleration > maxAngularAcceleration)
        {
            result.angular /= angularAcceleration;
            result.angular *= maxAngularAcceleration;
        }
        return result;
    }
}


//Notes for creating a client class to apply blended
/*
 * create parent class with standard update
 * create a protected steering output steeringupdate
 * children change its value - steeringupdate.linear = myMoveType.getSteering().linear
 *  base.update()
 *  
 *  
 *  class flocker: kinematic
 *  blendedSteering mySteering
 *  
 *  set up arrive, look where going, seperate
 *  
 *  create a blended steering
 *  behaviors = new BehaviorAndWeight[5]
 *  
 *  steeringUpdate = mySteering.GetSteering()
 *  
 *  initialize array of behavior
 *  
 *  seperation - seperate from all the flockers - need alot of flockers so you dont want to manually assign target
 *  GameObject[] birds = findgameobjectswithtag(bird)
 *  problem is one is going to be yourself
 *  
 *  convert array of gameobject into an array of kinematic
 *  
 *  kBirds = new Kinematic[birds.Length -1] 
 *  int j = 0
 *  for (int i = 0; i < bird.length-1; i++
 *      if (bird[i] == this
 *          continue
 *       kBirds[j++] = Birds[i].GetComponent<Kinematic>();
 * seperate.target = kBirds;
 */