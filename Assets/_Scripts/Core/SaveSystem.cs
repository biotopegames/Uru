using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void SavePlayerPosition(string sceneName, Vector3 position)
    {
        PlayerPrefs.SetFloat(sceneName + "_PlayerPosX", position.x);
        PlayerPrefs.SetFloat(sceneName + "_PlayerPosY", position.y);
    }

    public static Vector3 LoadPlayerPosition(string sceneName)
    {
        float x = PlayerPrefs.GetFloat(sceneName + "_PlayerPosX", 0f);
        float y = PlayerPrefs.GetFloat(sceneName + "_PlayerPosY", 0f);
        return new Vector3(x, y, 0);
    }
}