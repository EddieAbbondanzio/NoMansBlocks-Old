using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Master class to handle input and control when focus is given
/// to specific scripts
/// </summary>
public static class InputManager {
    /// <summary>
    /// Triggered when there is a change in the 
    /// current active focus of the scene.
    /// </summary>
    public static event EventHandler OnConsoleFocus;

    public static event EventHandler OnConsoleRelease;

    static InputManager () {
        LockCursor();
    }

    /// <summary>
    /// Fire off event to focus on the console.
    /// </summary>
    public static void FocusOnConsole(object sender) {
        if(OnConsoleFocus != null) {
            OnConsoleFocus(sender, null);
        }
    }

    /// <summary>
    /// Fire off event to release focus from the console.
    /// </summary>
    public static void ReleaseConsole(object sender) {
        if(OnConsoleRelease != null) {
            OnConsoleRelease(sender, null);
        }
    }

    /// <summary>
    /// Focus on one specific target.
    /// </summary>
    public static void FocusOnTarget(string target) {

    }

    /// <summary>
    /// Return the focus to everything.
    /// </summary>
    public static void ReleaseFocus() {

    }

    //Locks and hides cursor
   public static void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Unlocks and makes cursor visible
    public static void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
