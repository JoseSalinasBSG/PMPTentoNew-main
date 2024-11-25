using UnityEngine;

[CreateAssetMenu(menuName = "TextMessage", fileName = "TextMessage_Data")]
public class TextMessageSO : ScriptableObject
{ 
    [TextArea]
    public string[] nextQuestionMessage;
    [TextArea]
    public string[] correctQuestionMessage;
    [TextArea]
    public string[] incorrectQuestionMessage;
    [TextArea]
    public string[] wonMessage;
    [TextArea]
    public string[] lostMessage;
}
