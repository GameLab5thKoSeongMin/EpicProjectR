using System;
using UnityEngine;

public enum DialogueSpeaker
{
    Contractor,
    Player
}

[Serializable]
public class DialogueLine
{
    [SerializeField] private string key;
    [TextArea]
    [SerializeField] private string text;
    [SerializeField] private DialogueSpeaker speaker;

    public string Key => key;
    public string Text => text;
    public DialogueSpeaker Speaker => speaker;

    public DialogueLine(string key, string text)
        : this(key, text, ResolveSpeaker(key))
    {
    }

    public DialogueLine(string key, string text, DialogueSpeaker speaker)
    {
        this.key = key ?? string.Empty;
        this.text = text ?? string.Empty;
        this.speaker = speaker;
    }

    public static DialogueSpeaker ResolveSpeaker(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return DialogueSpeaker.Contractor;

        if (key.EndsWith(".player", StringComparison.OrdinalIgnoreCase))
            return DialogueSpeaker.Player;

        return DialogueSpeaker.Contractor;
    }
}
