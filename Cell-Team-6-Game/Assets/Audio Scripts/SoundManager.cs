using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    private static Hashtable SoundList = null;

  
    void Awake()
    {
        SoundList = new Hashtable();
        AudioSource[] Sounds = FindObjectsOfType<AudioSource>();
        foreach(AudioSource sound in Sounds)
        {
         //   if(sound.clip != null)
         //   {
                string SoundId = sound.gameObject.GetHashCode().ToString();
                SoundList.Add(SoundId, sound);
         //   }
        }
    }
    public static bool PlaySound(int hash,string clip)
    {
        
        string SoundId = hash.ToString() ;
        Debug.Log("in play:" + SoundId);
        Debug.Log("in go:" + SoundId);

        if (SoundList[SoundId] == null)
        {
            Debug.Log("no Source error.");
            return false;

        }
        AudioSource toPlay = (AudioSource)SoundList[SoundId];
        if(toPlay == null)
        {
            Debug.Log("Source error.");
            return false;
        }
    
        AudioClip clip2play = (AudioClip)Resources.Load("sounds/" + clip );
        if (toPlay == null)
        {
            Debug.Log("Source error.");
            return false;
        }
       // Debug.Log("Ready.");
        toPlay.clip = clip2play;
        toPlay.Stop();
        toPlay.Play();
        return true; 
    }

    
}
