using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public int TeamID { get { return teamID; } }
    public int PlayerID { get { return playerID; } }
    public bool Reflective { get { return reflective; } }

    private int teamID = -1;
    private int playerID = -1;
    private bool reflective = false;

    public void SetEntityProperties(int teamID, int playerID, bool reflective)
    {
        this.teamID = teamID;
        this.playerID = playerID;
        this.reflective = reflective;
    }

}
