using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationsSeen
{
    None = 0,
    Balcony = 1 << 0,
    UpperLanding = 1 << 1,
    Buffet = 1 << 2,
    Stairs = 1 << 3,
    Garden = 1 << 4,
    LowerLanding = 1 << 5,
    DanceFloor = 1 << 6
}

//Data object for Player's notes on masked guests
public class MaskedGuestCardData
{
    public string name;
    public LocationsSeen locationsSeen;
    public string tags;
}
