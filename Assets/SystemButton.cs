using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

public class SystemButton : MonoBehaviour
{

    private Image image;
    public TextMeshProUGUI tooltip;

    private Color startColor;

    private void Start()
    {
        image = GetComponent<Image>();
        startColor = image.color;

    }


    public void AFuoco()
    {
        StartCoroutine(FadeToColor(Color.red));
    }

    public void Scaricato()
    {
        StartCoroutine(FadeToColor(Color.blue));
    }

    public void ResetColor()
    {
        StartCoroutine(FadeToColor(startColor));
    }

    IEnumerator FadeToColor(Color finalColor)
    {
        float tick = 0f;

        while (image.color != finalColor)
        {
            tick += Time.deltaTime * 5;
            image.color = Color.Lerp(image.color, finalColor, tick);
            yield return null;
        }
    }

    private void OnMouseOver()
    {
        Debug.Log("Over");
        tooltip.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        tooltip.gameObject.SetActive(false);
    }
}
