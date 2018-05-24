using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Utilities;
using UnityEngine.UI;

/// <summary>
/// Tiny script to handle the fading out of messages that have been
/// on the screen too long.
/// </summary>
public class MessageFader : MonoBehaviour {
    #region Constants
    /// <summary>
    /// How long each message stays on screen.
    /// </summary>
    private float FadeTime = 8.0f;
    #endregion

    #region Properties
    /// <summary>
    /// The color of the message.
    /// </summary>
    public Color32 Color { get; set; }

    /// <summary>
    /// The text UI of the message.
    /// </summary>
    private Text Text { get; set; }

    /// <summary>
    /// The parent control script.
    /// </summary>
    private MessageBoxController MessageBox { get; set; }

    /// <summary>
    /// How long until the message is faded.
    /// </summary>
    private float TimeLeft { get; set; }

    /// <summary>
    /// If the message is currently fading.
    /// </summary>
    private bool Fading { get; set; }
    #endregion

    #region Mono Events
    /// <summary>
    /// Find references
    /// </summary>
    private void Awake() {
        Text = GetComponent<Text>();
        MessageBox = GameObject.FindGameObjectWithTag("Console").GetComponent<MessageBoxController>();
    }

    /// <summary>
    /// Use this to subscribe to events.
    /// </summary>
    private void OnEnable() {
        InputManager.OnConsoleFocus += InputManager_OnFocusConsole;
        InputManager.OnConsoleRelease += InputManager_OnReleaseConsole;
    }

    /// <summary>
    /// Use this to desubscribe from events
    /// </summary>
    private void OnDisable() {
        InputManager.OnConsoleFocus -= InputManager_OnFocusConsole;
        InputManager.OnConsoleRelease -= InputManager_OnReleaseConsole;
    }


    private void InputManager_OnReleaseConsole(object sender, System.EventArgs e) {
        Hide();
    }

    private void InputManager_OnFocusConsole(object sender, System.EventArgs e) {
        Show();
    }

    /// <summary>
    /// Update timer if it still has time
    /// left.
    /// </summary>
    private void Update () {
        if (Fading) {
            //Not there yet
            if (TimeLeft > 0) {
                TimeLeft -= Time.deltaTime;

                //Don't lerp when the box is open.
                if (!MessageBox.IsMaximized) {
                    //Lerp dat color
                    if (TimeLeft < 1.0f) {
                        Text.color = Color32.Lerp(Color, new Color32(), 1.0f - TimeLeft);
                    }
                }
            }
            //Done!
            else {
                Fading = false;
            }
        }
	}
    #endregion

    #region Publics
    /// <summary>
    /// Begin to fade out the message.
    /// </summary>
    public void StartFade(Color32 color) {
        Color = color;
        Text.color = color;
        Fading = true;
        TimeLeft = FadeTime;
    }

    /// <summary>
    /// Bypass the fader and show the message.
    /// </summary>
    public void Show() {
        Text.color = Color;
    }

    /// <summary>
    /// Return control back to the fader.
    /// This hides the message if it is no longer fading.
    /// </summary>
    public void Hide() {
        if (!Fading) {
            Text.color = new Color32();
        }
    }
    #endregion
}
