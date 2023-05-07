using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpChipDescription", menuName = "Scriptable/Other chips/PowerUpChipDescription")]

public class PowerUpChipScriptable : ChipScriptable
{
    public ChipPowerUpType chipPowerUp;

    private void Awake()
    {
        chipType = ChipType.POWERUP;
    }

}
