using System;

public enum TagEnum { Missile, Obstacle, Zone}

public class TagManager
{

    public static string GetTagNameByEnum(TagEnum value)
    {
        return Enum.GetName(typeof(TagEnum), value);
    }

}


