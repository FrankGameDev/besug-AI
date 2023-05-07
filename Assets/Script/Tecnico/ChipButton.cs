using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]

//TODO Refactor con aggiunta di 2 sottoclassi, una per powerup e una per consumabili
public class ChipButton : MonoBehaviour
{
    public GameObject tooltip;

    public ChipType chipType;

    public ChipPowerUpType chipPowerUp;
     public ChipConsumabiliType chipConsumabili;

    public ChipScriptable descrizioneChip;

    private void Awake()
    {
        chipPowerUp = ChipPowerUpType.NULL;
        chipConsumabili = ChipConsumabiliType.NULL;
    }
    private void Start()
    {
        tooltip.GetComponentInChildren<TextMeshProUGUI>().text = "\"" + descrizioneChip.nome + "\" \n\n" + descrizioneChip.descrizione;
    }
    private void OnMouseOver()
    {
        tooltip.SetActive(true);
    }

    private void OnMouseExit()
    {
        tooltip.SetActive(false);
    }


    public void SelectChip()
    {
        TecnicoManager.Instance.MostraPopupSceltaChip(chipType, descrizioneChip.nome);
    }

    public void SelectPowerChip()
    {
        TecnicoManager.Instance.MostraPopupSceltaChip(chipPowerUp, descrizioneChip.nome);
    }

    public void SelectConsumabiliChip()
    {
        TecnicoManager.Instance.MostraPopupSceltaChip(chipConsumabili, descrizioneChip.nome);
    }
}
