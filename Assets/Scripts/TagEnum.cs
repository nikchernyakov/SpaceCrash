using System;

public enum TagEnum { Player, MainCamera, Obstacle, Zone, Enemy}

public enum GameStateEnum { Play, Pause, GameOver}

public class TagManager
{

    public static string GetTagNameByEnum(TagEnum value)
    {
        return Enum.GetName(typeof(TagEnum), value);
    }

}


