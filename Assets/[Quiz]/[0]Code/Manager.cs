using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private void Awake()
    {
        GameData.Hearts = PlayerPrefs.GetInt("Hearts", 1);
    }
}

public static class GameData
{
    public static int Hearts;
}