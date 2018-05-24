using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated;

/// <summary>
/// Interface to abstract away the actual implmenetion for
/// displaying the messages.
/// </summary>
public interface ITextDisplay {
    /// <summary>
    /// Display a message to the user.
    /// </summary>
    void DisplayMessage(string message);

    /// <summary>
    /// Display a colored text message to the user.
    /// </summary>
    void DisplayColorMessage(string message, Color32 color);

    /// <summary>
    /// Display an error message to the screen.
    /// </summary>
    void DisplayError(string message);

    /// <summary>
    /// Display an info message to the screen.
    /// </summary>
    void DisplayInfo(string message);
}
