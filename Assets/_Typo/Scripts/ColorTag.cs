using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextColor
{
    red,
    green,
    blue,
    yellow,
    undefined,
}
public struct ColorTag
{
    public string Tag { get; }
    public TextColor TextColor { get; set; }
    public string hex { get; set; }
    public string Seperator { get; set; }
    public int Height { get; set; }

    public ColorTag(TextColor textColor, string seperator = "&SPT&")
    {
        TextColor = textColor != TextColor.undefined ? textColor : TextColor.red;
        hex = "#ff0000";
        Seperator = seperator;
        Tag = "<color=" + TextColor + ">" + seperator + "</color>";
        Height = Tag.Length - seperator.Length + 1;
    }
    public ColorTag(string hex = "#ff0000", string seperator = "&SPT&")
    {
        TextColor = TextColor.undefined;
        this.hex = hex;
        Seperator = seperator;
        Tag = "<color=" + this.hex + ">" + seperator + "</color>";
        Height = Tag.Length - seperator.Length + 1;
    }
}