using UnityEngine;

public static class GameOptions 
{
    public static float Volume = 1f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Load()
    {
        Volume = PlayerPrefs.GetFloat("Volume", 1f);
    }

    public static void Save()
    {
        PlayerPrefs.SetFloat("Volume", Volume);
    }
}    
