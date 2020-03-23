using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Radius
{
    Rad1,
    Rad2,
    Rad3,
}

public class BossEnemy : FSM
{
    [System.Serializable]
    public class RadRanges
    {
        public Vector2 position;
        public float rad1;
        public float rad2;
    }

    [SerializeField]
    public RadRanges radRange;
    public HealthScript healthScript;
    public float shootTime;
    public float shootInterval;

    public List<Animator> tentacles;
    public float Phase2Threshold;
    public float lashDistance;
    public float projectileDistance;
    protected override void Initalize()
    {
        //currentHealth = initalHealth;
        healthScript = GetComponent<HealthScript>();
        tentacles = new List<Animator>(transform.Find("Boss Body").GetComponentsInChildren<Animator>());
        Phase2Threshold = 200;
        BuildFSM();
    }
    protected virtual void BuildFSM()
    {
        BossIdleState bossIdle = new BossIdleState();
        bossIdle.AddTransitionState(FSMStateID.Lash, FSMTransitions.InLashRange);
        bossIdle.AddTransitionState(FSMStateID.GrappleLash, FSMTransitions.Phase2LashRange);
        bossIdle.AddTransitionState(FSMStateID.Lunge, FSMTransitions.InLungeRange);
        bossIdle.AddTransitionState(FSMStateID.Projectile, FSMTransitions.InProjectileRange);
        bossIdle.AddTransitionState(FSMStateID.Tracking, FSMTransitions.Phase2ProjectileRange);
        //---------------------------------
        bossIdle.AddTransitionState(FSMStateID.WallSpawn, FSMTransitions.WallTime);
        bossIdle.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        GrappleLashState grappleLash = new GrappleLashState();
        grappleLash.AddTransitionState(FSMStateID.Lunge, FSMTransitions.PlayerOutOfRange);
        //---------------------------------
        grappleLash.AddTransitionState(FSMStateID.WallSpawn, FSMTransitions.WallTime);
        grappleLash.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.none);
        grappleLash.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        LashState lashState = new LashState();
        lashState.AddTransitionState(FSMStateID.Lunge, FSMTransitions.PlayerOutOfRange);
        lashState.AddTransitionState(FSMStateID.GrappleLash, FSMTransitions.Phase2LashRange);
        //---------------------------------
        lashState.AddTransitionState(FSMStateID.WallSpawn, FSMTransitions.WallTime);
        lashState.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.none);
        lashState.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);


        LungeState lungeState = new LungeState();
        lungeState.AddTransitionState(FSMStateID.Lash, FSMTransitions.PlayerTooClose);
        lungeState.AddTransitionState(FSMStateID.Projectile, FSMTransitions.PlayerOutOfRange);
        //-----------------------------------
        lungeState.AddTransitionState(FSMStateID.WallSpawn, FSMTransitions.WallTime);
        lungeState.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.none);
        lungeState.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        ProjectileAttackState projectileAttack = new ProjectileAttackState();
        projectileAttack.AddTransitionState(FSMStateID.Lunge, FSMTransitions.PlayerTooClose);
        projectileAttack.AddTransitionState(FSMStateID.Tracking, FSMTransitions.Phase2ProjectileRange);
        //----------------------------------
        projectileAttack.AddTransitionState(FSMStateID.WallSpawn, FSMTransitions.WallTime);
        projectileAttack.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.none);
        projectileAttack.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        TrackRoundState trackRoundState = new TrackRoundState();
        trackRoundState.AddTransitionState(FSMStateID.Lunge, FSMTransitions.PlayerTooClose);
        //-------------------------------------
        trackRoundState.AddTransitionState(FSMStateID.WallSpawn, FSMTransitions.WallTime);
        trackRoundState.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.none);
        trackRoundState.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        WallSpawnState wallSpawnState = new WallSpawnState();
        wallSpawnState.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.none);
        wallSpawnState.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        DeadState dead = new DeadState();


        AddFSMState(bossIdle);
        AddFSMState(lashState);
        AddFSMState(lungeState);
        AddFSMState(grappleLash);
        AddFSMState(projectileAttack);
        AddFSMState(trackRoundState);
        AddFSMState(wallSpawnState);
        AddFSMState(dead);
    }

    public Radius RadRangeCheck(Transform other) //According to the C# documentation, this suposedly works.
    {                                            //If anyone has any issues, Just mention and I'll switch it to if/elseif statements
        float distance = Vector2.Distance(other.position, transform.TransformPoint(radRange.position));

        switch (distance)
        {
            case float dist when (dist <= radRange.rad1):
                return Radius.Rad1;
            case float dist when (dist <= radRange.rad2):
                return Radius.Rad2;
            case float dist when (dist > radRange.rad2):
                return Radius.Rad3;
            default:
                return Radius.Rad3;

        }
    }

    protected override void FSMUpdate()
    {
        //throw new System.NotImplementedException();
    }

    protected override void FSMFixedUpdate()
    {
        //throw new System.NotImplementedException();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.TransformPoint(radRange.position), radRange.rad1);
        Gizmos.DrawWireSphere(transform.TransformPoint(radRange.position), radRange.rad2);
    }
}
