using System;

public enum TagEnum { Player, MainCamera, Obstacle, Zone, Enemy}

public class TagManager
{

    public static string GetTagNameByEnum(TagEnum value)
    {
        return Enum.GetName(typeof(TagEnum), value);
    }

}


