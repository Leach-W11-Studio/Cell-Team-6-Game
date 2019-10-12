using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState : MonoBehaviour
{
    protected FSMStateID stateID;
    public FSMStateID StateID { get { return stateID; } }

    //[Reserved space for navmesh integration]

    /// <summary>
    /// Performs all actions related to the current state of the State Machine
    /// </summary>
    /// <param name="player">The transform assigned to the player object</param>
    /// <param name="self">The transform of the object this perticular state is assigned to</param>
    public abstract void Act(Transform player, Transform self);

    /// <summary>
    /// Performs all checks and actions related to transitioning to new states from the current
    /// </summary>
    /// <param name="player">The transform assigned to the player object</param>
    /// <param name="self">The transform of the object this perticular state is assigned to</param>
    public abstract void Transition(Transform player, Transform self);
}
