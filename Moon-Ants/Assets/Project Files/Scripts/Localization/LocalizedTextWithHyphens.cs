using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalizedTextWithHyphens : LocalizedText
{
    private const char Separator = '$';

    protected override string GetTranslation(LocalizationData data)
    {
        string[] words = data.GetTranslation(LocalizationID).Split(Separator);
        string text = string.Empty;

        foreach (var word in words)
        {
            text += word;
            text += Environment.NewLine;
        }
        return text;
    }
}
