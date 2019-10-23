using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void takeDamage(int damage);
}

public class BaseEnemy : FSM, IDamageable
{
    public int initalHealth = 10;
    public int currentHealth;
    public float rotationSpeed = 5f;
    public float agroDistance = 5f;

    public BulletSpawnerScript spawnerScript;

    protected override void Initalize()
    {
        currentHealth = initalHealth;
        spawnerScript = gameObject.GetComponentInChildren<BulletSpawnerScript>();
        spawnerScript.shoot = false;

        BuildFSM();
    }

    protected override void FSMFixedUpdate()
    {
        //throw new System.NotImplementedException();
    }

    protected override void FSMUpdate()
    {
        //throw new System.NotImplementedException();
    }

    protected virtual void BuildFSM()
    {
        IdleState idle = new IdleState();
        idle.AddTransitionState(FSMStateID.Shoot, FSMTransitions.SawPlayer);
        idle.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        ShootState shoot = new ShootState();
        shoot.AddTransitionState(FSMStateID.Idle, FSMTransitions.PlayerOutOfRange);
        shoot.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        DeadState dead = new DeadState();

        AddFSMState(idle);
        AddFSMState(shoot);
        AddFSMState(dead);
    }

    public void takeDamage(int damage)
    {
        Debug.Log("Taking Damage");
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, initalHealth);
    }
}
