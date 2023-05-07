using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PannelloCavi : Puzzle
{
    private static PannelloCavi _instance;

    public static PannelloCavi instance
    {
        get
        {
            return _instance;
        }
    }

    [Header("Cavi")]
    public GameObject[] caviInferiori;
    public GameObject[] caviSuperiori;

    private Image[] caviInferioriImage;
    private Image[] caviSuperioriImage;

    public ConnettoreCavo[] scriptConnettore;
    public CableDrop[] scriptDrop;

    private List<int> caviConnessi = new List<int>();

    List<Color> coloriCavi = new List<Color>(new[] { Color.red, Color.yellow, Color.blue });

    private int indexCavoDiPartenza;
    private int _countCaviCollegati;

    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    void InitializeAllComponent()
    {
        caviInferioriImage = new Image[caviInferiori.Length];
        caviSuperioriImage = new Image[caviSuperiori.Length];

        for (int i = 0; i < caviInferiori.Length; i++)
        {
            caviInferioriImage[i] = caviInferiori[i].GetComponent<Image>();
            caviSuperioriImage[i] = caviSuperiori[i].GetComponent<Image>();
        }
    }



    void RandomizeCableOrders()
    {
        InitializeAllComponent();

        int numeroCavi = caviInferioriImage.Length;

        List<Color> coloriCaviSuperiori = new List<Color>(coloriCavi);
        List<Color> coloriCaviInferiori = new List<Color>(coloriCavi);

        for (int i = 0; i < numeroCavi; i++)
        {
            int indexRandomSup = Random.Range(0, coloriCaviSuperiori.Count);
            int indexRandomInf = Random.Range(0, coloriCaviInferiori.Count);
            caviInferioriImage[i].color = coloriCaviInferiori[indexRandomInf];
            caviSuperioriImage[i].color = coloriCaviSuperiori[indexRandomSup];

            coloriCaviInferiori.RemoveAt(indexRandomInf);
            coloriCaviSuperiori.RemoveAt(indexRandomSup);

            scriptConnettore[i].SetCavo(i);

        }
    }

    public void ImpostaConnettore(int index) => indexCavoDiPartenza = index;

    public void ControllaCavoCorretto(int dropConnettore, Transform cableDrop)
    {
        Color dropColor = caviSuperioriImage[dropConnettore].color;

        if (indexCavoDiPartenza < 0 || caviConnessi.Contains(indexCavoDiPartenza))
            return;

        Debug.Log("Colore drop: " + dropColor.ToString());
        Debug.Log("Colore cavo di partenza: " + caviInferioriImage[indexCavoDiPartenza].color.ToString());
        if (dropColor.Equals(caviInferioriImage[indexCavoDiPartenza].color))
        {
            scriptConnettore[indexCavoDiPartenza].DrawCable(cableDrop.position);
            Debug.Log("cavo uguale");
            _countCaviCollegati += 1;
            caviConnessi.Add(indexCavoDiPartenza);
        }
        if (_countCaviCollegati == caviInferiori.Length)
        {
            //inserire animazioni o altro
            Debug.Log("tutto giusto bravo pollo");
            _isFaultActive = false;
        }

    }

    public override void InitPuzzle()
    {
        if (!_isFaultActive)
        {
            indexCavoDiPartenza = -1;
            RandomizeCableOrders();
            _isFaultActive = true;
        }
    }

    public override void ResetStatus()
    {
        Debug.Log("Reset");
        _isFaultActive = false;

        Array.ForEach(scriptConnettore, c =>
        {
            c.enabled = true;
            c.ResetCavo();
        }
        );
    }
}
