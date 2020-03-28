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
    public int shootChance = 10;

    public Transform muzzle;
    protected override void Initalize()
    {
        //currentHealth = initalHealth;
        healthScript = GetComponent<HealthScript>();
        tentacles = new List<Animator>(transform.Find("Boss Body").GetComponentsInChildren<Animator>());
        //Phase2Threshold = 200;
        muzzle = transform.Find("Muzzle");
        BuildFSM();
    }
    protected virtual void BuildFSM() //To Finish
    {
        //Phase 1 Stuff
        BossIdleState bossIdle = new BossIdleState();
        bossIdle.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        //Add transition for HealthLessThanThreshold here - Goes to phase 2
        //---------------------------------
        bossIdle.AddTransitionState(FSMStateID.LashReady, FSMTransitions.InMeleeRange);
        bossIdle.AddTransitionState(FSMStateID.Projectile, FSMTransitions.GreaterThanRad2);

        LashReadyState lashReady = new LashReadyState(shootChance);
        lashReady.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        lashReady.AddTransitionState(FSMStateID.Lash, FSMTransitions.InRad1);
        lashReady.AddTransitionState(FSMStateID.Projectile, FSMTransitions.OORad1AndChance);
        lashReady.AddTransitionState(FSMStateID.Lunge, FSMTransitions.InRad2);

        LashState lash = new LashState();
        lash.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        lash.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.BehaviorComplete);

        LungeState lunge = new LungeState();
        lunge.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        lunge.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.BehaviorComplete);

        ProjectileAttackState projectile = new ProjectileAttackState();
        projectile.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        projectile.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.BehaviorComplete);

        DeadState dead = new DeadState();

        AddFSMState(projectile);
        AddFSMState(bossIdle);
        AddFSMState(lashReady);
        AddFSMState(lash);
        AddFSMState(lunge);
        AddFSMState(dead);

        #region Depricated Code
        /* GrappleLashState grappleLash = new GrappleLashState();
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


        AddFSMState(projectileAttack);
        AddFSMState(bossIdle);
        AddFSMState(lashState);
        AddFSMState(lungeState);
        AddFSMState(grappleLash);
        AddFSMState(trackRoundState);
        AddFSMState(wallSpawnState);
        AddFSMState(dead); */
        #endregion
    }

    /// <summary>
    /// Returns the raidus range of the other object in relation to the current object.
    /// </summary>
    /// <param name="other">The other object for which range is to be measured from</param>
    /// <returns></returns>
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
                return Radius.Rad1;

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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.TransformPoint(radRange.position), radRange.rad2);
    }
}
