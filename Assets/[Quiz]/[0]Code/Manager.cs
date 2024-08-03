using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private void Awake()
    {
        GameData.Hearts = PlayerPrefs.GetInt("Hearts", Constants.DefaultHeartCount);
    }
}

public static class GameData
{
    public static int Hearts;
}