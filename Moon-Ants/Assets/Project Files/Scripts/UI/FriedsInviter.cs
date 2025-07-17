using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriedsInviter : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private WebSdk _webSdk;
    [SerializeField] private Button _inviteButton;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private int _cristalCount;

    private void Awake()
    {
#if YANDEX_GAMES
        _inviteButton.gameObject.SetActive(false);
#endif
    }

#if VK_GAMES
    private void Start()
    {
        _label.text = _cristalCount.ToString();
    }
 
    private void OnEnable()
    {
        _inviteButton.onClick.AddListener(OnInviteButtonClick);
        _webSdk.FriendsInvited += OnFriendsInvited;
    }

    private void OnDisable()
    {
        _inviteButton.onClick.RemoveListener(OnInviteButtonClick);
        _webSdk.FriendsInvited -= OnFriendsInvited;
    }

    private void OnFriendsInvited()
    {
        _wallet.AddBlueCristals(_cristalCount);
    }

    private void OnInviteButtonClick()
    {
        _webSdk.InviteFriends();
    }
#endif
}
