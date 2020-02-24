using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker: AbstractKinematic
{
    //BlendedSteering flocking;
    float epsilon = 0.0001f;
    Arrive arrive;
    LookWhereGoing look;
    Seperation seperate;
    ObstacleAvoidance avoid;
    SteeringOutput blendedSteeringType;
    BlendedSteering[] flockGroups;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        arrive = new Arrive();
        look = new LookWhereGoing();
        seperate = new Seperation();
        avoid = new ObstacleAvoidance();
        flockGroups = new BlendedSteering[3];

        arrive.target = target;
        arrive.ai = this;
        look.target = target;
        look.ai = this;
        seperate.ai = this;
        avoid.ai = this;

        AbstractKinematic[] kBirds;
        GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
        kBirds = new AbstractKinematic[birds.Length - 1];
        int j = 0;
        for (int i = 0; i < birds.Length - 1; i++) {
            if (birds[i] == this)
               continue;
            kBirds[j++] = birds[i].GetComponent<AbstractKinematic>();
        }
        seperate.targets = kBirds;

        InitializePriorityOne();
        InitializePriorityTwo();
        InitializePriorityThree();

        /*
        flocking.behaviors = new BehaviorAndWeight[3];
        BehaviorAndWeight behavior1 = new BehaviorAndWeight();
        behavior1.behavior = arrive;
        behavior1.weight = 100f;
        flocking.behaviors[0] = behavior1;
        BehaviorAndWeight behavior2 = new BehaviorAndWeight();
        behavior2.behavior = look;
        behavior2.weight = 1f;
        flocking.behaviors[1] = behavior2;
        BehaviorAndWeight behavior3 = new BehaviorAndWeight();
        behavior3.behavior = seperate;
        behavior3.weight = 20f;
        flocking.behaviors[2] = behavior3;
        */
    }
    void InitializePriorityOne()
    {
        flockGroups[0] = new BlendedSteering();
        flockGroups[0].behaviors = new BehaviorAndWeight[2];
        flockGroups[0].behaviors[0] = new BehaviorAndWeight();
        flockGroups[0].behaviors[1] = new BehaviorAndWeight();

        flockGroups[0].behaviors[0].behavior = avoid;
        flockGroups[0].behaviors[0].weight = 10000f;
        flockGroups[0].behaviors[1].behavior = seperate;
        flockGroups[0].behaviors[1].weight = 1f;
    }

    void InitializePriorityTwo()
    {
        flockGroups[1] = new BlendedSteering();
        flockGroups[1].behaviors = new BehaviorAndWeight[1];
        flockGroups[1].behaviors[0] = new BehaviorAndWeight();
        flockGroups[1].behaviors[0].behavior = arrive;
        flockGroups[1].behaviors[0].weight = 1f;
    }

    void InitializePriorityThree()
    {
        flockGroups[2] = new BlendedSteering();
        flockGroups[2].behaviors = new BehaviorAndWeight[1];
        flockGroups[2].behaviors[0] = new BehaviorAndWeight();
        flockGroups[2].behaviors[0].behavior = look;
        flockGroups[2].behaviors[0].weight = 4f;
    }
    // Update is called once per frame
    public override void Update()
    {
        foreach(BlendedSteering priorityGroup in flockGroups)
        {
            mySteering = priorityGroup.GetSteering();
           // Debug.Log(priorityGroup.behaviors[0].behavior);
           // Debug.Log(mySteering.linear);
            if (mySteering.linear.magnitude > epsilon || Mathf.Abs(mySteering.angular) > epsilon)
                base.Update();
        }
        /*
        mySteering = flocking.GetSteering();
        base.Update()
        */
    }
}
