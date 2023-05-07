using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumabiliChipDescription", menuName = "Scriptable/Other chips/ConsumabiliChipDescription")]
public class ConsumabiliChipScriptable : ChipScriptable
{
    public ChipConsumabiliType chipConsumabili;


    private void Awake()
    {
        chipType = ChipType.CONSUMABILE;
    }

}
