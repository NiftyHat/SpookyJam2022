using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum LocationsSeen
{
    None        = 0,      
    Balcony     = 1 << 0, 
    UpperLanding = 1 << 1, 
    Buffet      = 1 << 2, 
    Stairs      = 1 << 4,
    Garden      = 1 << 5,
    LowerLanding = 1 << 6,
    DanceFloor = 1 << 7
}

public class MaskedGuestMenuWidget : MonoBehaviour
{
    public Image guestPortrait;
    public TextMeshProUGUI nameDisplayText;
    public TextMeshProUGUI locationDisplayText;
    public TextMeshProUGUI tagDisplayText;


}
