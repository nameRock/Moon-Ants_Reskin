using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    private const char Separator = '$';

    [SerializeField] private float _delay;
    [SerializeField] private TMP_Text _label;
    
    private string[] _textBlocks;
    private int _maxCharInLine;
    private WaitForSecondsRealtime _secondsRealtime;
    private TMP_FontAsset _defaultFont;
    public int TextIndex { get; private set; }
    public int TextBlocksLenght => _textBlocks.Length;

    public void Init(string text, TMP_FontAsset font)
    {
        if (font != null)
            _label.font = font;

        _secondsRealtime = new WaitForSecondsRealtime(_delay);
        TextIndex = 0;
        _textBlocks = text.Split(Separator);
        string maxLenghtText = _textBlocks.OrderByDescending(block => block.Length).ToArray()[0];
        Debug.Log(maxLenghtText);
        CalculateFontSize(maxLenghtText);
        _maxCharInLine = GetMaxCharInLine();
    }

    public IEnumerator WaitTextAnimation()
    {
        string animatedText = _textBlocks[TextIndex];
        int lineIndex = 0;
        _label.text = string.Empty;

        string[] words = animatedText.Split();

        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words[i].Length; j++)
            {
                _label.text += words[i][j];
                yield return _secondsRealtime;
            }
            _label.text += " ";
            yield return _secondsRealtime;

            int nextIndex = i + 1;
            if (nextIndex >= words.Length)
                break;

            TryToSwitchNewLine(words[nextIndex],_maxCharInLine,  ref lineIndex);
        }
    }

    public void IncreaseTextIndex()
    {
        TextIndex++;
    }  

    private void TryToSwitchNewLine(string nextWord, int maxCharInLine, ref int lineIndex)
    {
        int nextCharacterCount = _label.textInfo.lineInfo[lineIndex].characterCount + nextWord.Length;
        if (lineIndex == _label.textInfo.lineCount - 1)
        {
            if (nextCharacterCount > maxCharInLine)
            {
                _label.text += Environment.NewLine;
                lineIndex++;
            }
        }
        else if (nextCharacterCount == maxCharInLine)
        {
            lineIndex++;
        }
    }

    private void CalculateFontSize(string maxLenghtText)
    {
        _label.enableAutoSizing = true;
        _label.text = maxLenghtText;
        _label.ForceMeshUpdate();
        float fontSize = _label.fontSize;
        _label.enableAutoSizing = false;
        _label.fontSize = fontSize*0.9f;
    }

    private int GetMaxCharInLine()
    {
        int maxCharInLine = 0;

        foreach (var block in _textBlocks)
        {
            _label.text = block;
            _label.ForceMeshUpdate();
            int currentCherInLine = _label.textInfo.lineInfo.Max(line => line.characterCount);
            if (maxCharInLine < currentCherInLine)
                maxCharInLine = currentCherInLine;
        }
        _label.text = string.Empty;
        _label.ForceMeshUpdate();
        return maxCharInLine;
    }
}
