using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals
{
    public enum AnimalState
    {
        RunningTowardsFood,
        RunningTowardsWater,
        Drinking,
        Eating,
        Idle,
        Fleeing,
        ChasingPrey,
        LookingForMate,
        MakePopulation
    }
}