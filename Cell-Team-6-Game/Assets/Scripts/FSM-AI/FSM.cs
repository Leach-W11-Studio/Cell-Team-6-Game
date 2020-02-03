using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

/// <summary>
/// Contains all fms states to be implimented in any FSM of this project
/// </summary>
public enum FSMStateID
{
    none = 0,
    Idle,
    Shoot,
    Dead,
    Chase,
    Passive,
    Patrol,
    Retreat,
}

public enum FSMTransitions
{
    none = 0,
    SawPlayer,
    PlayerOutOfRange,
    OutOfHealth,
    Awoken,
    PlayerTooClose,
    CloserDistanceReached,
}

public abstract class FSM : MonoBehaviour
{
    public Transform playerTransform;

    //Leaving following block empty for implimentation of navmesh once that is present
    //[Navmesh Implimentation Here]

    protected abstract void Initalize();
    protected abstract void FSMUpdate();
    protected abstract void FSMFixedUpdate();

    public PolyNavAgent navAgent;

    /// <summary>
    /// Whether or not the enemy is active and executing their relevent functions
    /// </summary>
    public bool Active;

    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    private List<FSMState> FSMStates = new List<FSMState>();

    /// <summary>
    /// Adds an FSM state to the list of currently avalable states. First added state is treated as inital state as well
    /// </summary>
    /// <param name="state">State which is to be added</param>
    public void AddFSMState(FSMState state)
    {
        foreach (var item in FSMStates)
        {
            if (item.StateID == state.StateID)
            {
                Debug.LogError("Error: You can not add a state that is already present in this FSM");
                return;
            }
        }

        //First state added is treated as inital starting state for purposes of this FSM
        if (FSMStates.Count == 0)
        {
            FSMStates.Add(state);
            currentState = state;
        }
        else
        {
            FSMStates.Add(state);
        }
    }

    /// <summary>
    /// Removes FSMState from list of currently avalable FSM States
    /// </summary>
    /// <param name="stateID">State which is to be removed</param>
    public void RemoveFSMState(FSMStateID stateID)
    {
        foreach (var item in FSMStates)
        {
            if (item.StateID == stateID)
            {
                FSMStates.Remove(item);
                return;
            }
        }
        Debug.LogError("Error: " + stateID.ToString() + " Is not present within FSMStates");
    }

    /// <summary>
    /// Sets new state based off input transition
    /// </summary>
    /// <param name="trans">Transition to be set to given state</param>
    public void SetTransition(FSMTransitions trans)
    {
        if (trans == FSMTransitions.none)
        {
            Debug.LogError("Null transition is not allowed");
            return;
        }
        FSMStateID newID = currentState.CheckTransition(trans);
        if (newID == FSMStateID.none)
        {
            Debug.LogError("There is no state bound to " + trans.ToString());
            return;
        }
        foreach (var state in FSMStates)
        {
            if (newID == state.StateID)
            {
                Debug.Log("State changed to " + newID.ToString());
                currentState.OnStateExit(playerTransform, gameObject);

                currentState = state;
                currentState.OnStateEnter(playerTransform, gameObject);
                return;
            }
        }
        Debug.LogError("Unidentified error with state transition");
    }

    // Start is called before the first frame update
    void Start()
    {
        Active = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = gameObject.GetComponent<PolyNavAgent>();
        Initalize();
        currentState.OnStateEnter(playerTransform, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            currentState.Reason(playerTransform, gameObject);
            FSMUpdate();
        }
    }

    void FixedUpdate()
    {
        if (Active)
        {
            currentState.Act(playerTransform, gameObject);
            FSMFixedUpdate();
        }
    }
}
