using System.Collections.Generic;
using UnityEngine;

public class CabinetInfo
{
    string _userId;
    public string userId
    {
        get {return _userId ;}
        private set 
        {
            _userId = value; 
        }
    }

   // public Dictionary<string, string> grids{get;private set;}

    //public Dictionary<int, int> devices;

    //public string _decorations { get; }

    public Vector2 vb;
}

