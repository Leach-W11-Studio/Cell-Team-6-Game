using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class FSMState
{
    protected FSMStateID stateID;
    public FSMStateID StateID { get { return stateID; } }
    //protected List<FSMStateID> transitionableStates = new List<FSMStateID>(); Depricated due to code update
    protected Dictionary<FSMTransitions, FSMStateID> transitions = new Dictionary<FSMTransitions, FSMStateID>();

    //[Reserved space for navmesh integration]

    /// <summary>
    /// Performs all actions related to the current state of the State Machine. Called in FixedUpdate.
    /// </summary>
    /// <param name="player">The transform assigned to the player object</param>
    /// <param name="self">The GameObject of the object this perticular state is assigned to</param>
    public abstract void Act(Transform player, GameObject self);

    /// <summary>
    /// Performs all checks and actions related to transitioning to new states from the current. Called in Update.
    /// </summary>
    /// <param name="player">The transform assigned to the player object</param>
    /// <param name="self">The GameObject of the object this perticular state is assigned to</param>
    public abstract void Reason(Transform player, GameObject self);

    /// <summary>
    /// Performs all single operations that are to be performed upon switching to a state. Called once upon entering state.
    /// </summary>
    /// <param name="player">The transform assigned to the player object</param>
    /// <param name="self">The GameObject of the object this perticular state is assigned to</param>
    public abstract void OnStateEnter(Transform player, GameObject self);

    /// <summary>
    /// Performs all single operations that are to be performed upon switching out of a state. Called once upon exiting state.
    /// </summary>
    /// <param name="player">The transform assigned to the player object</param>
    /// <param name="self">The GameObject of the object this perticular state is assigned to</param>
    public abstract void OnStateExit(Transform player, GameObject self);

    /// <summary>
    /// Checks if transition is avalable for use
    /// </summary>
    /// <param name="transition">Transition to be set</param>
    /// <returns>Returns transition to be transitioned to if legal, otherwise returns current state</returns>
    public FSMStateID CheckTransition(FSMTransitions transition)
    {
        if (transitions.ContainsKey(transition))
        {
            return transitions[transition];
        }
        else
        {
            Debug.LogError("Transition " + transition.ToString() + " is not present within allowed transitions for this state");
            return stateID;
        }
    }

    /// <summary>
    /// Adds transition to given state after checking
    /// </summary>
    /// <param name="transitionState">State that is to be transitioned to with transition</param>
    /// <param name="trans">transition selected to transition to above state</param>
    public void AddTransitionState(FSMStateID transitionState, FSMTransitions trans)
    {
        if (transitionState == FSMStateID.none)
        {
            Debug.LogError("Null Transition not allowed");
            return;
        }
        if (transitions.ContainsKey(trans))
        {
            Debug.LogError(trans.ToString() + " transition already listed as Transitionable");
            return;
        }
        transitions.Add(trans, transitionState);
        Debug.Log("Transition state " + transitionState.ToString() + " has been added as a transitionable state of " + stateID.ToString() + " using transition " + trans.ToString());
    }

    /// <summary>
    /// Removes transition state from legal transition states
    /// </summary>
    /// <param name="trans">Transition indented to be removed</param>
    public void RemoveTransitionState(FSMTransitions trans)
    {
        if (trans == FSMTransitions.none)
        {
            Debug.LogError("Null Transition not allowed");
            return;
        }
        if (transitions.ContainsKey(trans))
        {
            transitions.Remove(trans);
            Debug.Log("Transition " + trans.ToString() + " has been removed as a transitionable state of " + stateID.ToString());
            return;
        }
        Debug.LogError(trans.ToString() + " transition is not present within the transitionable states of " + stateID.ToString());
    }
}
