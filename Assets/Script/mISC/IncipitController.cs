using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncipitController : MonoBehaviour
{
    public Image[] imageToFade;
    public TextMeshProUGUI[] textToShow;

    void Start()
    {
        StartCoroutine(FadeIn(0));
    }

    IEnumerator FadeIn(int index)    // entri nella scena
    {
        Debug.Log("indexo " + index);
        float f = 1f;

        do
        {
            f -= 0.02f;
            imageToFade[index].color =  Color.Lerp(Color.white, Color.black, Mathf.PingPong(f, 1));
            textToShow[index].color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(f, 1));
            yield return new WaitForSeconds(0.02f);

        } while (f > 0f);

        imageToFade[index].color = Color.white;
        textToShow[index].color = Color.white;

        f = 1f;

        yield return new WaitForSeconds(4f);

        do
        {
            f -= 0.02f;
            textToShow[index].color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(f, 1));
            yield return new WaitForSeconds(0.001f);

        } while (f > 0f);


        textToShow[index].gameObject.SetActive(false);

        yield return new WaitForSeconds(.1f);

        index++;
        if (index < 3)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(FadeIn(index));
        }
        else if (index >= 3)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.MENU);
        }
    }

}
