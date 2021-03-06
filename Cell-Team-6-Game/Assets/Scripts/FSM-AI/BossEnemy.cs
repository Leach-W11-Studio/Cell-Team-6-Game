﻿using System.Collections;
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
    public HealthScript coreHealthScript;
    protected List<HealthScript> tentacleHealthScripts = new List<HealthScript>();
    public float shootTime;
    public float shootInterval;
    [Range(0f, 1f)]
    public float shootCone;
    [Range(0f, 1f)]
    public float wallSpawnThreshold;

    public Animator CoreAnim;
    public float wallSpawnInterval;

    [HideInInspector]
    public List<Animator> tentacles;
    [HideInInspector]
    public List<CircleCollider2D> bones;
    [HideInInspector]
    public Dictionary<Animator, List<CircleCollider2D>> tentacleColliders;
    [HideInInspector]
    public float timeSinceWallSpawn = 0;

    [Tooltip("The number of remaining tentacles at which the boss transitions to phase 2")]
    public int Phase2Threshold = 3;
    public float lashDistance;
    public float projectileDistance;
    public int shootChance = 10;

    [HideInInspector]
    public List<BossWalls> bossWallList = new List<BossWalls>();

    public bool doWallSpawnTrigger { get; protected set; } //Will set the trigger for wall spawn, given the need for complex timing logic.

    public Transform muzzle;

    private bool phase2 = false;
    private float healthPercent {
        get {
            return tentacles.Count / 5;
        }
    }
    protected override void Initalize()
    {
        //currentHealth = initalHealth;
        coreHealthScript = GetComponent<HealthScript>();
        coreHealthScript.invincible = true;
        tentacles = new List<Animator>(transform.Find("Boss Body").GetComponentsInChildren<Animator>());
        tentacleColliders = new Dictionary<Animator, List<CircleCollider2D>>();
        bossWallList = new List<BossWalls>(gameObject.GetComponentsInChildren<BossWalls>());

        foreach (var wall in bossWallList) { wall.gameObject.SetActive(false); }
        //Sets the bones for each tentcle in a dictionary... can be referenced via tentacleColliders[tentacle]
        foreach (var tentacle in tentacles)
        {
            //Populates list of health scripts
            tentacleHealthScripts.Add(tentacle.gameObject.GetComponent<HealthScript>());

            //Animations and Bones
            bones = new List<CircleCollider2D>(tentacle.transform.GetComponentsInChildren<CircleCollider2D>());
            Debug.Log("There are " + bones.Count + " bones in this tentacle.", tentacle);
            tentacleColliders.Add(tentacle, bones);
            //Sets bones inactive
            foreach (var bone in bones)
            {
                bone.enabled = false;
            }
        }
        doWallSpawnTrigger = false;
        //Phase2Threshold = 200;
        muzzle = transform.Find("Muzzle");
        BuildFSM();
    }

    private void ResetWallCheck() { doWallSpawnTrigger = true; }

    public void RemoveTentacle(HealthScript tentacleHealthScript)
    {
        if (tentacleHealthScripts.Contains(tentacleHealthScript))
        {
            tentacleHealthScripts.Remove(tentacleHealthScript);
            tentacles.Remove(tentacleHealthScript.GetComponent<Animator>());
        }
        else
        {
            Debug.LogError("Warning: TentacleHealthScripts does not contain the referenced TentacleHealthScript");
            return;
        }

        Debug.Log(tentacleHealthScripts.Count + " Tentacles remain in TentacleHealthScripts");
        if (tentacleHealthScripts.Count <= 0)
        {
            coreHealthScript.invincible = false;
            Debug.Log("Boss Core now vulnerable");
        }
    }

    public int CheckTentacleCount()
    {
        return tentacleHealthScripts.Count;
    }

    protected virtual void BuildFSM() //To Finish
    {
        phase2 = false;

        //Phase 1 Stuff
        BossIdleState bossIdle = new BossIdleState();
        bossIdle.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        bossIdle.AddTransitionState(FSMStateID.Phase2Setup, FSMTransitions.HealthLessThanThreshold);
        //---------------------------------
        bossIdle.AddTransitionState(FSMStateID.LashReady, FSMTransitions.InMeleeRange);
        bossIdle.AddTransitionState(FSMStateID.Projectile, FSMTransitions.GreaterThanRad2);
        bossIdle.AddTransitionState(FSMStateID.Roar, FSMTransitions.PlayerInRangeTooLong);

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

        ProjectileAttackState projectile = new ProjectileAttackState("BossBulletPhase1");
        projectile.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        projectile.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.BehaviorComplete);

        RoarState roar = new RoarState();
        roar.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        roar.AddTransitionState(FSMStateID.BossIdle, FSMTransitions.BehaviorComplete);

        SetupPhase2State phase2Setup = new SetupPhase2State();
        phase2Setup.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        phase2Setup.AddTransitionState(FSMStateID.BossIdlePhase2, FSMTransitions.BehaviorComplete);

        BossDeadState dead = new BossDeadState();

        AddFSMState(bossIdle);
        AddFSMState(projectile);
        AddFSMState(roar);
        AddFSMState(lashReady);
        AddFSMState(lash);
        AddFSMState(lunge);
        AddFSMState(phase2Setup);
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

    public virtual void RebuildFSMForPhase2()
    {
        if (phase2) { return; }
        phase2 = true;

        BossIdleStatePhase2 bossIdle2 = new BossIdleStatePhase2();
        bossIdle2.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        bossIdle2.AddTransitionState(FSMStateID.GrappleLash, FSMTransitions.InMeleeRange);
        bossIdle2.AddTransitionState(FSMStateID.Projectile, FSMTransitions.GreaterThanRad2);
        bossIdle2.AddTransitionState(FSMStateID.WallSpawn, FSMTransitions.WallSpawnTriggered);
        bossIdle2.AddTransitionState(FSMStateID.Roar, FSMTransitions.PlayerInRangeTooLong);

        RemoveFSMState(FSMStateID.BossIdle);
        RemoveFSMState(FSMStateID.Lunge);
        RemoveFSMState(FSMStateID.Projectile);
        AddFSMState(bossIdle2);

        var lashState = GetFSMState(FSMStateID.Lash);
        lashState.EditTransitionState(FSMStateID.BossIdlePhase2, FSMTransitions.BehaviorComplete);
        var lungeState = new GrappleLashState();
        lungeState.AddTransitionState(FSMStateID.BossIdlePhase2, FSMTransitions.BehaviorComplete);
        lungeState.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        var projectile = new ProjectileAttackState("BossBulletPhase2");
        projectile.AddTransitionState(FSMStateID.BossIdlePhase2, FSMTransitions.BehaviorComplete);
        projectile.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        var roar = GetFSMState(FSMStateID.Roar);
        roar.EditTransitionState(FSMStateID.BossIdlePhase2, FSMTransitions.BehaviorComplete);

        WallSpawnState wallSpawn = new WallSpawnState(bossWallList);
        wallSpawn.AddTransitionState(FSMStateID.BossDead, FSMTransitions.OutOfHealth);
        wallSpawn.AddTransitionState(FSMStateID.BossIdlePhase2, FSMTransitions.BehaviorComplete);

        AddFSMState(wallSpawn);
        AddFSMState(lungeState);
        AddFSMState(projectile);
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

        if (phase2)
        {
            timeSinceWallSpawn += Time.deltaTime;
        }
        tentacles = new List<Animator>(transform.Find("Boss Body").GetComponentsInChildren<Animator>());
        if (tentacles.Count == 0)
        {
            coreHealthScript.invincible = false;
        }
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

        Vector2 perp = Vector2.Perpendicular(-transform.up);
        Vector2 positiveCone = Vector2.Lerp(-transform.up, perp, shootCone).normalized;
        Vector2 negativeCone = Vector2.Lerp(-transform.up, -perp, shootCone).normalized;
        Debug.DrawRay(transform.position, positiveCone * 50, Color.yellow);
        Debug.DrawRay(transform.position, negativeCone * 50, Color.yellow);
    }
}
