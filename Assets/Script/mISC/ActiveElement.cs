public class ActiveElement
{
    public float timer;
    public string text;
    public int code;

    public ActiveElement(float t, string s, int c)
    {
        timer = t; text = s; code = c;
    }

    public ActiveElement() { timer = 0; text = ""; code = 0; }
}
