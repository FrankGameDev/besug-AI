using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChipDescription", menuName = "Scriptable/Chip Description")]
public class ChipScriptable : ScriptableObject
{
    public string nome;
    public string descrizione;
    public ChipType chipType;
}
