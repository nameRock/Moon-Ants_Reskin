using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Finisher : MonoBehaviour
{
    [SerializeField] private TargetBuilder _builder;
    /*
    [SerializeField] private EndCanvas _endCanvas;
    */
    [SerializeField] private UI _ui;
    /*[SerializeField] private GameObject _canvasUpgrade;
    [SerializeField] private GameObject _canvasRevarde;
    [SerializeField] private GameObject _canvasBoard;
    [SerializeField] private GameObject _canvasLocalization;
    [SerializeField] private SkinsDisplay _canvasSkins;*/
    [SerializeField] private ParticleSystem _stars;
    [SerializeField] protected PlayerBaza _baza;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private GameObject _text;
    [SerializeField] private GameObject _circle;
    [SerializeField] private Transform _finishPosition;
    [SerializeField] private bool _isCollection = false;
    [SerializeField] private GameObject _porter;
    [SerializeField] private AudioSource _audioSourcComplited;

    [SerializeField] protected float FlyDelay;
    [SerializeField] protected float ShowFinishCanvasDelay = 3f;

    private bool _isReached = false;

    private void OnEnable()
    {
        _builder.Builded += OnBuilded;
    }

    private void OnDisable()
    {
        _builder.Builded -= OnBuilded;
    }

    protected abstract void Launch();

    private void OnBuilded()
    {
        if (_isReached == false)
        {
            _audioSourcComplited.Play();
            StartCoroutine(StartWaitingLaunch());
            _isReached = true;
        }
    }

    private IEnumerator StartWaitingLaunch()
    {
        yield return new WaitForSeconds(0.5f);
        _text.SetActive(false);
        _circle.SetActive(false);
        _stars.transform.SetParent(null);
        _stars.Play();
        _ui.DisableUIForFinish();
        _playerMover.SetJoystickState(false);
        StartCoroutine(WaitLaunch());
        _ui.HideSoundImage();
    }

    protected virtual IEnumerator WaitLaunch()
    {
        if (!_isCollection)
        {
            ReturnAll();
            yield return new WaitForSeconds(FlyDelay);
            Launch();
        }
        else
        {
            _porter.SetActive(true);
            Launch();
            _baza.StopAllAnts();

            yield return new WaitForSeconds(FlyDelay);
        }
        yield return new WaitForSeconds(ShowFinishCanvasDelay);
        ShowFinishCanvas();
    }

    protected void ReturnAll()
    {
        _playerMover.MoveToFinish(_finishPosition);
        _baza.ReturnAllAnts(_finishPosition);
    }

    protected void ReturnPlayerToPath(Transform[] targets)
    {
        Vector3[] path = new Vector3[targets.Length];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = targets[i].position;
        }
        _playerMover.MoveToPath(path);
    }

    protected void ShowFinishCanvas()
    {
        _ui.ActivateEndCanvas();
    }

    /*protected void DisableUI()
    {
        _canvasUpgrade.SetActive(false);
        _canvasRevarde.SetActive(false);
        _canvasBoard.SetActive(false);
        _canvasLocalization.SetActive(false);
        _playerMover.SetJoystickState(false);
        _canvasSkins.gameObject.SetActive(false);
    }*/
}
