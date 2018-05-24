using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Network;
using Voxelated.Events;
using Voxelated.Network.Messages;
using Voxelated.Utilities;

/// <summary>
/// Attach this to the IChatDisplay gameobject.
/// </summary>
public class NetChatDisplay : MonoBehaviour {
    #region Properties
    /// <summary>
    /// The chat display of the client.
    /// </summary>
    public ITextDisplay Display { get; private set; }
    #endregion

    #region Mono Events
    // Use this for initialization
    private void Awake() {
        //Find the chat display attached to this object.
        Display = GetComponent<ITextDisplay>();

        if (Display == null) {
            LoggerUtils.LogError("NetChatDisplay: No display found.");
        }
    }

    /// <summary>
    /// Use this for event subscription.
    /// </summary>
    private void OnEnable() {
        //NetMessageHandler.OnLobbyChatMessage += OnChatMessage;
    }

    /// <summary>
    /// Use this for event unsubscribing.
    /// </summary>
    private void OnDisable() {
        //NetMessageHandler.OnLobbyChatMessage -= OnChatMessage;
    }
    #endregion

    /// <summary>
    /// Fired when a chat message is recieved.
    /// </summary>
    private void OnChatMessage(object sender, NetMessageArgs e) {
        //LobbyChatMessage chatMsg = e.Message as LobbyChatMessage;

        //if (chatMsg == null || Display == null) {
        //    return;
        //}

        //Display.DisplayMessage(chatMsg.ToString());
    }
}
