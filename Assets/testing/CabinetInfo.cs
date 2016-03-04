using System.Collections.Generic;

public class CabinetInfo
{
    private int _userId;

    internal int userId
    {
        set { _userId = value; }
    }

    public int UserId
    {
        get { return _userId; }
    }

    public Dictionary<string, string> grids;
    public Dictionary<int, int> devices;


    public string _decorations;

}


