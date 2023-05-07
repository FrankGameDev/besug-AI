using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Image[] BG;

    public void StartGame()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.PLAY);
    }

    public void Tutorial()
    {

        GameManager.Instance.UpdateGameState(GameManager.GameState.TUTORIAL);
    }
    
}
