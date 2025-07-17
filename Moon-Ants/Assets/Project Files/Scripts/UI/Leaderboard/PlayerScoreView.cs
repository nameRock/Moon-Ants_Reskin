using TMPro;
using UnityEngine;

public class PlayerScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _scoreLabel;

    public void Init(string name, int score )
    {
        _nameLabel.text = name;
        _scoreLabel.text = score.ToString();
    }
}
