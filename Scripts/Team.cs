using UnityEngine;

[System.Serializable]
public struct Team
{
    public int losses, ties, wins;
    public string name;
    public Color color;

    public Team(string teamName, int teamLosses, int teamTies, int teamWins, Color teamColor)
    {
        name = teamName;
        losses = teamLosses;
        ties = teamTies;
        wins = teamWins;
        color = teamColor;
    }
}
