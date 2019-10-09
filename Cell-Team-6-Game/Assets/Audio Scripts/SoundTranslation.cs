using UnityEngine;
using System.Collections;

public class SoundTranslation : MonoBehaviour {
	private static Hashtable SoundTranslations = null;
	
	private static void Init () {
		// read the translation file into the hash table.
		SoundTranslations = new Hashtable();
		string Translationfile = Application.dataPath + "/" + "translation.txt";
		if(System.IO.File.Exists(Translationfile))
		{
			System.IO.StreamReader sr = new System.IO.StreamReader(Translationfile);
			string fulltxt ;
			if(sr != null)
			{
				while((fulltxt = sr.ReadLine()) != null)
				{
					string[] stringSeparators = new string[] {","};
					string[] result;
					result = fulltxt.Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);
					if(result[0] != null && result[1] != null)
					{
						// add it to the hashtable..
						SoundTranslations.Add(result[0],result[1]);
					}
					
				}
				sr.Close();
			}
		}
		else
		{
			Debug.Log("No translation file :" + Translationfile);
		}
	}
	
	public static string GetSoundIDTranslation(string itemname)
	{
		if(SoundTranslations == null)Init();
		string ret = "";
		if(SoundTranslations.ContainsKey(itemname))
		{
		ret = (string)SoundTranslations[itemname]; 
		}
		else
		{
			Debug.Log ("No sound translation for " + itemname);
		}
		return ret;
	}
}
