using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : FSM
{
    [System.Serializable]
    public class LashRange {
        public Vector2 position;
        public float radius;
    }

    [SerializeField]
    public LashRange lashRange;
    public HealthScript healthScript;
    public float shootTime;
    public float shootInterval;

    public List<Animator> tentacles;
    protected override void Initalize()
    {
        //currentHealth = initalHealth;
        healthScript = GetComponent<HealthScript>();
        tentacles = new List<Animator>(transform.Find("Boss Body").GetComponentsInChildren<Animator>());
        BuildFSM();
    }
    protected virtual void BuildFSM()
    {
        BossIdleState bossIdle = new BossIdleState();
        //[State Name].AddTransitionState([State to transition to], [Transition Name]);

        GrappleLashState grappleLash = new GrappleLashState();

        LashState lashState = new LashState();

        ProjectileAttackState projectileAttack = new ProjectileAttackState();

        TrackRoundState trackRoundState = new TrackRoundState();

        WallSpawnState wallSpawnState = new WallSpawnState();

        DeadState dead = new DeadState();


        AddFSMState(bossIdle);
        AddFSMState(lashState);
        AddFSMState(grappleLash);
        AddFSMState(projectileAttack);
        AddFSMState(trackRoundState);
        AddFSMState(wallSpawnState);
        AddFSMState(dead);
    }

    public bool InLashRange (Transform other) {
        if (Vector2.Distance(other.position, transform.TransformPoint(lashRange.position)) < lashRange.radius) {
            return true;
        }
        else {return false;}
    }

    protected override void FSMUpdate()
    {
        //throw new System.NotImplementedException();
    }

    protected override void FSMFixedUpdate()
    {
        //throw new System.NotImplementedException();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.TransformPoint(lashRange.position), lashRange.radius);
    }
}
