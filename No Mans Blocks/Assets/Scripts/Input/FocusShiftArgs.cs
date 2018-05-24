using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FocusShiftArgs : EventArgs {
    /// <summary>
    /// The target that needs to recieve input.
    /// </summary>
    public string Target { get; private set; }

    public FocusShiftArgs(string target) {
        Target = target;
    }
}
