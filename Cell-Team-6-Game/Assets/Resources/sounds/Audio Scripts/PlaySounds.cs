﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[ExecuteInEditMode]
public class PlaySounds : MonoBehaviour
{

    public  AudioMixer mixer;
    public  AudioMixerSnapshot[] snapshots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool TransitionMixer(int mixTo)
    {
        snapshots[mixTo].TransitionTo(100);
        return true;
    }

    public void PlayUISound()
    {
        SoundManager.PlaySound(gameObject.GetHashCode() , "UI Sound");
    }

    public void PlayVocalization()
    {
        string snd = gameObject.name + "_vocal";
        string transsnd = SoundTranslation.GetSoundIDTranslation(snd);
        if(transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }
    public void PlayAttack()
    {
        string snd = gameObject.name + "_atk";
        string transsnd = SoundTranslation.GetSoundIDTranslation(snd);
        Debug.Log("Attack Sound Played");
        if (transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }

    public void PlayHurtSound()
    {
        string snd = gameObject.name + "_hurt";
        string transsnd = SoundTranslation.GetSoundIDTranslation(snd);
        if (transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }

    public void PlaySelfDestructSound()
    {
        string snd = gameObject.name + "_sd";
        string transsnd = SoundTranslation.GetSoundIDTranslation(snd);
        Debug.Log("Self Destruct Sound Played");
        if (transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }

    public void PlayIdleSound()
    {
        string snd = gameObject.name + "_idle";
        string transsnd = SoundTranslation.GetSoundIDTranslation(snd);
        if (transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }

    public void PlayHuntersMark()
    {
        string snd = gameObject.name + "_hm";
        string transsnd = SoundTranslation.GetSoundIDTranslation(snd);
        Debug.Log("Hunters Mark Sound Played");
        if (transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }

    public void PlayWaterPop()
    {
        string snd = gameObject.name + "_wp";
        string transsnd = SoundTranslation.GetSoundIDTranslation(snd);
        Debug.Log("Water Pop Sound Played");
        if (transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }

    public void PlayRiotShieldDeploy()
    {
        string snd = gameObject.name + "_rsd";
        string transsnd = SoundTranslation.GetSoundIDTranslation(snd);
        Debug.Log("Riot Shield Deployed Sound Played");
        if (transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }

    public void PlayRiotShieldStow()
    {
        string snd = gameObject.name + "_rss";
        string transsnd = SoundTranslation.GetSoundIDTranslation(snd);
        Debug.Log("Riot Shield Stow Sound Played");
        if (transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }



}
