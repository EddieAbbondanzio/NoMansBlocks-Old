using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voxelated;
using Voxelated.Engine.Console;
using Voxelated.Network;
using Voxelated.Utilities;

/// <summary>
/// Script to handle pulling commands from an UI input field. Attach
/// this to the game object that has the input field on it.
/// </summary>
public class InputFieldController : MonoBehaviour {
    #region Properties
    [Header("Key Settings")]
    /// <summary>
    /// The key that triggers command submission.
    /// </summary>
    public KeyCode SubmitKey = KeyCode.Return;

    /// <summary>
    /// The key that triggers focus onto the field.
    /// </summary>
    public KeyCode FocusKey = KeyCode.T;

    /// <summary>
    /// The key that triggers defocus of the field.
    /// </summary>
    public KeyCode ReleaseKey = KeyCode.Escape;

    [Header("Text Settings")]
    /// <summary>
    /// If the text should be cleared from the field after 
    /// submitting a command.
    /// </summary>
    public bool ClearOnSubmit = true;

    /// <summary>
    /// Should the text be cleared when a command is cancelled?
    /// </summary>
    public bool ClearOnDefocus = true;

    /// <summary>
    /// The input UI.
    /// </summary>
    private InputField InputField { get; set; }
    #endregion

    #region Events
    /// <summary>
    /// Fired when there is a command that was sent in
    /// </summary>
    //public EventHandler<CommandRecievedArgs> OnCommand { get; set; }
    #endregion

    #region Mono Events
    private void Awake() {
        //Find the input field element
        InputField = GetComponent<InputField>();

        //Subscribe to the on end edit method.
        InputField.onEndEdit.AddListener(delegate {
            if (Input.GetKeyDown(SubmitKey)) {
                //Execute command
                string inputString = InputField.text.Trim();
                ParseAndExecute(inputString);

                //Fire off the event
                //if (OnCommand != null) {
                //    OnCommand(this, new CommandRecievedArgs(inputString));
                //}

                Release();
            }

            //Return control to user.
            if (Input.GetKeyDown(ReleaseKey)) {
                Release();
            }
        });
    }

    private void Update() {
        //If the console is not focused, and focus key is hit. Focus it.
        if (Input.GetKeyDown(FocusKey) && !MessageBoxController.Instance.IsMaximized) {
            Focus();
        }

        //If the console is focused and release key is hit. Release it.
        if (Input.GetKeyDown(ReleaseKey) && MessageBoxController.Instance.IsMaximized) {
            Release();
        }
    }
    #endregion
    /// <summary>
    /// Focus on the chat input
    /// </summary>
    public void Focus() {
        InputManager.FocusOnConsole(this);
        InputField.ActivateInputField();

        if (ClearOnSubmit) {
            ClearText();
        }
    }

    /// <summary>
    /// Release focus from the chat
    /// input.
    /// </summary>
    public void Release() {
        InputManager.ReleaseConsole(this);
        InputField.DeactivateInputField();

        if (ClearOnDefocus) {
            ClearText();
        }
    }

    #region Helpers
    /// <summary>
    /// Parse a string for a command
    /// </summary>
    private void ParseAndExecute(string input) {
        if(input[0] == CommandConsole.EscapeChar) {
            VoxelatedEngine.Engine.Console.Parse(input);
        }

        ////Commmand. Process it.
        //if (input[0] == InputParser.EscapeChar) {
        //    ICommand command = InputParser.Parse(input);
        //    string errorMsg;

        //    if (command != null) {
        //        command.Execute(out errorMsg, InputParser.Arguments);

        //        if (errorMsg != string.Empty) {
        //            MessageBox.Instance.DisplayError(errorMsg);
        //        }
        //    }
        //    //Invalid Command
        //    else {
        //        MessageBox.Instance.DisplayError("Invalid command: " + input);
        //    }
        //}
        //////Standard chat
        //else {
        //    LoggerUtils.Log("sending chat!");
        //    NetManager.Instance.Chat.SendLobbyMessage(input);
        //    //MessageBox.Instance.DisplayMessage("BERT", input);
        //}
    }

    /// <summary>
    /// Clear the input field of text.
    /// </summary>
    private void ClearText() {
        InputField.text = "";
    }
    #endregion
}
