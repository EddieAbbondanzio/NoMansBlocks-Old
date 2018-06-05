using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voxelated;
using Voxelated.Utilities;

/// <summary>
/// Control class that handles display messages
/// to the user. Has an object pool to reduce overhead.
/// </summary>
public class MessageBoxController : MonoBehaviour, ITextDisplay  {
    #region Colors
    /// <summary>
    /// The background color of the chat box.
    /// </summary>
    private static readonly Color32 BackgroundColor = new Color32(0, 0, 0, 180);

    /// <summary>
    /// The color of the scroll bar handle
    /// </summary>
    private static readonly Color32 ScrollBarColor = new Color32(80, 80, 80, 160);

    /// <summary>
    /// The background color of the scroll bar.
    /// </summary>
    private static readonly Color32 ScrollBarBackgroundColor = new Color32(0, 0, 0, 50);

    /// <summary>
    /// The text color of regulat chat message.
    /// </summary>
    private static readonly Color32 TextColor = new Color32(203, 203, 203, 255);

    /// <summary>
    /// The text color of info message.
    /// </summary>
    private static readonly Color32 InfoColor = new Color32(225, 225, 225, 255);

    /// <summary>
    /// The text color of error message.
    /// </summary>
    private static readonly Color32 ErrorColor = new Color32(203, 0, 0, 255);
    #endregion

    #region Properties
    /// <summary>
    /// The singleton instance.
    /// </summary>
    public static MessageBoxController Instance { get; private set; }

    /// <summary>
    /// How many messages to save in the chat.
    /// </summary>
    public const int HistorySize = 32;

    /// <summary>
    /// The transform that messages should be 
    /// made a child of.
    /// </summary>
    public Transform MessageParent;

    /// <summary>
    /// The collection of message objects.
    /// </summary>
    public Queue<Text> Messages;

    /// <summary>
    /// If the box is in full display mode, or
    /// just small mode.
    /// </summary>
    public bool IsMaximized { get; set; }

    /// <summary>
    /// The background color of the textbox.
    /// </summary>
    private Image Background { get; set; }

    /// <summary>
    /// The rect transform for the box.
    /// </summary>
    private RectTransform BoxRect { get; set; }

    /// <summary>
    /// The scroll rect of the box.
    /// </summary>
    private ScrollRect ScrollRect { get; set; }

    /// <summary>
    /// The scroll bar on the message box.
    /// </summary>
    private Scrollbar ScrollBar { get; set; }

    /// <summary>
    /// How many messages currently have data in them.
    /// </summary>
    private int activeMessages { get; set; }
    #endregion

    #region Fade Control
    private const float FadeInterval = .1f;

    /// <summary>
    /// Timer used to track fading
    /// </summary>
    private float FadeTimer { get; set; }
    #endregion

    #region Mono Events
    // Use this for initialization
    void Awake() {
        Instance = this;

        //Find references
        Background = GetComponent<Image>();
        ScrollRect = GetComponent<ScrollRect>();
        BoxRect = GetComponent<RectTransform>();
        ScrollBar = GetComponentInChildren<Scrollbar>();

        activeMessages = 0;
        Messages = new Queue<Text>();
        Minimize();

        //Iterate through the children and save a reference to their text comps
        foreach(Transform child in MessageParent) {
            Messages.Enqueue(child.GetComponent<Text>());
            child.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Used to transition fading.
    /// </summary>
    private void Update() {
        if(FadeTimer < FadeInterval) {
            FadeTimer += UnityEngine.Time.deltaTime;
            float lerpScale = FadeTimer / FadeInterval;

            //We are fading in
            if (IsMaximized) {
                Background.color = Color32.Lerp(new Color32(), BackgroundColor, lerpScale);
                ScrollBar.GetComponent<Image>().color = Color32.Lerp(new Color32(), ScrollBarBackgroundColor, lerpScale);

                ColorBlock scrollColors = ScrollBar.colors;
                scrollColors.normalColor = Color32.Lerp(new Color32(), ScrollBarColor, lerpScale);

                ScrollBar.colors = scrollColors;
            }
            else {
                Background.color = Color32.Lerp(BackgroundColor, new Color32(), lerpScale);
                ScrollBar.GetComponent<Image>().color = Color32.Lerp(ScrollBarBackgroundColor, new Color32(), lerpScale);

                ColorBlock scrollColors = ScrollBar.colors;
                scrollColors.normalColor = Color32.Lerp(ScrollBarColor, new Color32(), lerpScale);

                ScrollBar.colors = scrollColors;
            }
        }
    }

    /// <summary>
    /// Use this to subscribe to events
    /// </summary>
    private void OnEnable() {
        InputManager.OnConsoleFocus += InputManager_OnFocusConsole;
        InputManager.OnConsoleRelease += InputManager_OnReleaseConsole;
    }

    /// <summary>
    /// Use this to desubscribe from events.
    /// </summary>
    private void OnDisable() {
        InputManager.OnConsoleFocus -= InputManager_OnFocusConsole;
        InputManager.OnConsoleRelease -= InputManager_OnReleaseConsole;
    }
    #endregion

    private void InputManager_OnReleaseConsole(object sender, EventArgs e) {
        Minimize();
    }

    private void InputManager_OnFocusConsole(object sender, EventArgs e) {
        Maximize();
    }

    #region Publics
    /// <summary>
    /// Display a standard message to the box.
    /// </summary>
    public void DisplayMessage(string message) {
        SetNewestMessage(message, TextColor);
        UpdateBoxSize();
    }

    /// <summary>
    /// Display a colored message to screen.
    /// </summary>
    public void DisplayColorMessage(string message, Color32 color) {
        SetNewestMessage(message, color);
        UpdateBoxSize();
    }

    /// <summary>
    /// Display an error Mesage to the screen.
    /// </summary>
    public void DisplayError(string errorMsg) {
        SetNewestMessage(errorMsg, ErrorColor);
        UpdateBoxSize();
    }

    /// <summary>
    /// Display an info message about
    /// various game state to the screen.
    /// </summary>
    public void DisplayInfo(string infoMsg) {
        SetNewestMessage(infoMsg, InfoColor);
        UpdateBoxSize();
    }

    /// <summary>
    /// Increase the box to it's full size.
    /// </summary>
    public void Maximize() {
        IsMaximized = true;
        FadeTimer = 0.0f;

        //Background.color = new Color32(0, 0, 0, 175);
        ScrollRect.vertical = true;
    }

    /// <summary>
    /// Shrink the box down to a small sample.
    /// </summary>
    public void Minimize() {
        IsMaximized = false;
        FadeTimer = 0.0f;

        //Background.color = new Color32(0, 0, 0, 0);
        ScrollRect.vertical = false;
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Sets the text to appear as the latest message.
    /// </summary>
    private void SetNewestMessage(string message, Color32 color) {
        //Pull it off the queue
        Text currMessage = Messages.Dequeue();

        //Modify it
        currMessage.text = message;
        currMessage.transform.SetAsLastSibling();
        currMessage.gameObject.SetActive(true);
        currMessage.GetComponent<MessageFader>().StartFade(color);

        //Put it back on!
        Messages.Enqueue(currMessage);
    }

    /// <summary>
    /// Update the message container
    /// size to properly adjust scroll bar.
    /// </summary>
    private void UpdateBoxSize() {
        if(activeMessages >= HistorySize) {
            return;
        }

        //Update the container size
        activeMessages++;
        int newHeight = Math.Max(156, activeMessages * 12);
        MessageParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

        //Show new message
        ScrollRect.verticalNormalizedPosition = 0;
    }
    #endregion
}
