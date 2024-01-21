using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class MainMenuView : UIView
{
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private Button         _hostButton;
    [SerializeField] private Button         _joinButton;

    private MainMenuState _state => UIViewState as MainMenuState;

    public override IEnumerator Show()
    {
        RegisterListener();
        yield return base.Show();
    }

    public override IEnumerator Hide()
    {
        UnregisterListener();
        yield return base.Hide();
    }

    private void OnHostClicked()
    {
        if (CheckIfPlayerNameEntered())
        {
            _state.OnHostClicked(_playerNameInput.text);
            SetWidgetInteractions(false);
            GenericFloaterBuilder.Show("Hosting...");
        }
    }

    private void OnJoinClicked()
    {
        if (CheckIfPlayerNameEntered())
        {
            _state.OnJoinedClicked(_playerNameInput.text);
            SetWidgetInteractions(false);
            GenericFloaterBuilder.Show("Joining...");
        }
    }

    private bool CheckIfPlayerNameEntered()
    {
        bool status = _playerNameInput != null && _playerNameInput.text.Length > 2;
        if (!status)
        {
            GenericFloaterBuilder.Show("Enter Player Name First!");
        }

        return status;
    }

    private void SetWidgetInteractions(bool interactable)
    {
        _playerNameInput.interactable = interactable;
        _hostButton.interactable      = interactable;
        _joinButton.interactable      = interactable;
    }

    public void ResetView()
    {
        SetWidgetInteractions(true);
    }

    private void RegisterListener()
    {
        _hostButton.onClick.AddListener(OnHostClicked);
        _joinButton.onClick.AddListener(OnJoinClicked);
    }


    private void UnregisterListener()
    {
        _hostButton.onClick.RemoveListener(OnHostClicked);
        _joinButton.onClick.RemoveListener(OnJoinClicked);
    }
}