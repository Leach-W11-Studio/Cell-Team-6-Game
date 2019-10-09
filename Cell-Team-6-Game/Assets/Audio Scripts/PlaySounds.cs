using System.Collections;
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
        SoundManager.PlaySound(gameObject.GetHashCode() , "ring");
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
        if (transsnd.Length > 0) SoundManager.PlaySound(gameObject.GetHashCode(), transsnd);
    }




    
   
}
