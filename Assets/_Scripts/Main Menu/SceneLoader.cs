using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string ShipGraveyardMap = "ShipGraveyard";
    private static string AsteroidFieldMap = "AsteroidField";
    private static string MainMenu = "Main Menu";


    

    public static void LoadCombatMap(Maps map)
    {
        // Randomize select a map.
        if(map == Maps.Random)
        {
            int i = UnityEngine.Random.Range(1, Enum.GetNames(typeof(Maps)).Length);
            map = (Maps)i;
        }

        switch (map)
        {
            case Maps.Graveyard:
                SceneManager.LoadSceneAsync(ShipGraveyardMap);
                break;
            case Maps.AsteroidField:
                SceneManager.LoadSceneAsync(AsteroidFieldMap);
                break;
        }
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(MainMenu);
    }
}
