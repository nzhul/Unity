using System;

public class InputSet
{
    public readonly string horizontalAxis;
    public readonly string fire;
    public readonly string altFire;
    public readonly string jump;

    public InputSet(PlayerID id)
    {
        string playerSuffix = "P1";
        if (id == PlayerID.P2)
            playerSuffix = "P2";

        horizontalAxis = "Horizontal_" + playerSuffix;
        fire = "Fire_" + playerSuffix;
        altFire = "AltFire_" + playerSuffix;
        jump = "Jump_" + playerSuffix;
    }
}
