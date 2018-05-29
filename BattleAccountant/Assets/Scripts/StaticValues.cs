using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticValues {

    public static int StartingCash = 100;
    public static int StartMonth = 1;
    public static int StartDay = 1;
    public static int StartYear = 3000;
    public static int CrewCost = 1;
    public static int MaxCrew = 15;
    public static int ShipMaxCrew = 6;
    public static int ShipMaxMechs = 4;
    public static string[] CrewFirstNames= { "Bill" , "Tom", "Darius", "Max", "Jericho", "Marcus", "Luke"
    ,"Abel", "Adam", "Alexis", "August", "Barry", "Brian", "Carmen", "Casey"};
    public static string[] MechModelNames = { "Mad Cat", "Rifleman", "Star Adder" };
    public static string[] MechNames = { "Alyosius", "Valerian", "Olympus" , "Canus"};
    public static string[] FactionNames = {"Reim Empire","Auran Collective", "Dragoons" };
    public static List<string> PlanetNames = new List<string>(new string[] { "Icarus", "Helios", "Cerebus", "Kronos" });
    public static int[,] PlanetDistances =new int[,] 
    { { 0,1,2,3 },
      { 1,0,1,2 },
      { 2,1,0,1 },
      { 3,2,1,0 } };

    public static int GetDistanceBetweenPlanets(string start, string end)
    {
        int startIndex = PlanetNames.IndexOf(start);
        int endIndex = PlanetNames.IndexOf(end);
        return PlanetDistances[startIndex, endIndex];
    }

    public static string GetRandomPlanet()
    {
        return PlanetNames[(int)Random.Range(0, PlanetNames.Count)];
    }

    public static string GenerateFirstName()
    {
        return CrewFirstNames[(int)Random.Range(0,CrewFirstNames.Length)];
    }

    public static string GenerateMechModel()
    {
        return MechModelNames[(int)Random.Range(0, MechModelNames.Length)];
    }

    public static string GenerateMechName()
    {
        return MechNames[(int)Random.Range(0, MechNames.Length)];
    }

    public static string GenerateFaction()
    {
        return FactionNames[(int)Random.Range(0, FactionNames.Length)];
    }

    public static int DaysInMonth(int month)
    {
        switch (month){
            case 1:
                return 31;
            case 2:
                return 28;
            case 3:
                return 31;
            case 4:
                return 30;
            case 5:
                return 31;
            case 6:
                return 30;
            case 7:
                return 31;
            case 8:
                return 31;
            case 9:
                return 30;
            case 10:
                return 31;
            case 11:
                return 30;
            default:
                return 31;
        }
    }
}
