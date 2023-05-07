using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using Random = UnityEngine.Random;

public class SystemController : MonoBehaviour
{

    [Tooltip("Il gameobject inserito deve contenere uno degli script relativi ai puzzle, il quale dovr� implementare l'interfaccia IPuzzle")]
    public Puzzle[] puzzles;
    int[] displayIndex;

    public SystemButton systemButton;

    [HideInInspector]
    public bool[] puzzleFaultActive;
    private float[] puzzleTimer;
    private HashSet<int> puzzleNotActiveIndexes = new HashSet<int>();

    public bool overheating;
    public bool voltageDrop;

    public bool isDisabled;


    private void Start()
    {
        puzzleFaultActive = new bool[puzzles.Length];
        puzzleTimer = new float[puzzles.Length];
        displayIndex = new int[puzzles.Length];
        for (int i = 0; i < puzzles.Length; i++) { displayIndex[i] = -1; }

        for (int i = 0; i < puzzles.Length; i++)
            puzzleNotActiveIndexes.Add(i);

    }

    private void Update()
    {
        if (overheating)
            systemButton.AFuoco();
        if (voltageDrop)
            systemButton.Scaricato();

        isDisabled = overheating || voltageDrop;
        //Inserire un pannello che copre il system e non permettere di premere sopra

        CheckStatusPuzzle();
    }

    public bool CheckAllPuzzleActive() => Array.TrueForAll(puzzleFaultActive, b => b);

    public void ActivateRandomPuzzle()
    {
        ChooseRandomPuzzle();
    }

    void ChooseRandomPuzzle()
    {
        int index = Random.Range(0, puzzleNotActiveIndexes.Count);

        //puzzleNotActiveIndexes.ToList().ForEach(p => Debug.Log(p));

        ActivatePuzzle(puzzleNotActiveIndexes.ElementAt(index));

    }

    private void ActivatePuzzle(int index)
    {
        puzzleNotActiveIndexes.Remove(index);
        puzzleFaultActive[index] = true;
        puzzles[index].InitPuzzle();
        Debug.Log("SAREBBE DISPLAY: " + puzzles[index].customTerminalText);
        puzzles[index].activeElementCode = LevelManager.Instance.EnqueueElement(puzzles[index].customTerminalText);
        Debug.Log("Ho attivato il puzzle " + gameObject.name + " " + puzzles[index].name);
    }




    private void DisablePuzzle(int index)
    {
        Debug.Log("Disabilito puzzle " + puzzles[index]);
        LevelManager.Instance.RemoveDisplayLine(puzzles[index].activeElementCode);
        puzzles[index].activeElementCode = 0;
        puzzleNotActiveIndexes.Add(index);
        puzzleFaultActive[index] = false;
        puzzleTimer[index] = 0;
        displayIndex[index] = -1;
        puzzles[index].ResetStatus();
        LevelManager.Instance.activeFaultCounter -= 1;
    }


    private void CheckStatusPuzzle()
    {
        for (int i = 0; i < puzzleFaultActive.Length; i++)
        {
            if (puzzleFaultActive[i] == false) continue;

            if (!puzzles[i].GetStatusFault())
            {
                DisablePuzzle(i);
                continue;
            }

            if (puzzleTimer[i] >= LevelManager.Instance.maxResolvingFaultTimer)
            {
                PlayerManager.Instance.OverheatingAfterEndOfTimer();
                DisablePuzzle(i);
                continue;
            }

            puzzleTimer[i] += Time.deltaTime;
            LevelManager.Instance.activeElements.Find(x => x.code == puzzles[i].activeElementCode).timer = puzzleTimer[i];

            if (puzzleTimer[i] == 5)
                PlayerManager.Instance.OverheatingAfterHalfTimer();
        }
    }

    public void DisableAllPuzzle()
    {
        for (int i = 0; i < puzzles.Length; i++)
        {
            DisablePuzzle(i);
        }
    }

    #region Estintore e Scarica

    public void Reboot()
    {
        if (voltageDrop)
        {
            voltageDrop = false;
            systemButton.ResetColor();
            Debug.Log("Voltage disabilitato per " + gameObject.name);
        }
    }

    public void ManualCooling()
    {
        if (overheating)
        {
            Debug.Log("Overheating disabilitato per " + gameObject.name);
            systemButton.ResetColor();
            overheating = false;
        }
    }

    #endregion

}
