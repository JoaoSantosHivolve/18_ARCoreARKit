using Assets.Scripts.Common;
using TMPro;

public class DebugText : Singleton<DebugText>
{
    public string Text
    {
        set => GetComponent<TextMeshProUGUI>().text = value;
    }
}
