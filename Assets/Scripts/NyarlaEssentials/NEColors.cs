using UnityEngine;

public static class NEColors
{
    public static Color WithA(Color color, float newA) => new Color(color.r, color.g, color.b, newA);
    public static Color32 WithA(Color32 color, byte newA) => new Color(color.r, color.g, color.b, newA);

}
