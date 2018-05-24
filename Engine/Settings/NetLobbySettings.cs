//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Voxelated.Utilities;
//using System;
//using System.Runtime.Serialization;
//using Voxelated.Network.Teams;
//using Voxelated.Network.Lobby;
//using Voxelated;

//namespace Voxelated.Settings {
//    /// <summary>
//    /// Holds information about various settings of the NetServer
//    /// and it's components.
//    /// </summary>
//    [Serializable]
//    public class NetLobbySettings : ICopyable<NetLobbySettings> {
//        #region Properties
//        /// <summary>
//        /// The name of the server
//        /// </summary>
//        public string Name { get; private set; }

//        /// <summary>
//        /// The description of the server
//        /// </summary>
//        public string Description { get; private set; }

//        /// <summary>
//        /// The max number of players allowed.
//        /// </summary>
//        public byte Capacity { get; private set; }

//        /// <summary>
//        /// How many teams are in the lobby.
//        /// </summary>
//        public TeamMode TeamMode { get; private set; }

//        /// <summary>
//        /// The current game mode of the lobby.
//        /// </summary>
//        public GameMode GameMode { get; private set; }

//        /// <summary>
//        /// How many players are allowed per team.
//        /// </summary>
//        public byte TeamSize {
//            get { return (byte)(Capacity / (byte)TeamMode); }
//        }
//        #endregion

//        #region Constructors
//        public NetLobbySettings(string name, string desc, byte capacity, TeamMode teamMode, GameMode gameMode) {
//            if (!IsNameValid(name)) {
//                LoggerUtils.LogError("Invalid name");
//                throw new ArgumentException("Lobby name is invalid");
//            }

//            if (!IsDescriptionValid(desc)) {
//                LoggerUtils.LogError("Invalid description");
//                throw new ArgumentException("Lobby description is invalid");
//            }

//            if (!IsCapacityValid(capacity)) {
//                LoggerUtils.LogError("Invalid capacity. Must be 8, 16, or 32.");
//                throw new ArgumentException("Invalid capacity. Must be 8, 16, or 32.");
//            }

//            if (!IsTeamModeValid(teamMode)) {
//                LoggerUtils.LogError("Invalid team mode.");
//                throw new ArgumentException("Invalid team mode.");
//            }

//            if (!IsGameModeValid(gameMode)) {
//                LoggerUtils.LogError("Invalid game mode.");
//                throw new ArgumentException("Invalid game mode.");
//            }

//            Name = name;
//            Description = desc;
//            Capacity = capacity;
//            TeamMode = teamMode;
//        }
//        #endregion

//        #region Setters
//        /// <summary>
//        /// Set the name of the server. Must be less than NameLength
//        /// </summary>
//        public bool SetName(string name) {
//            if (IsNameValid(name)) {
//                Name = name;
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Set the description of the server. Must be less than DescriptionLength
//        /// </summary>
//        public bool SetDescription(string desc) {
//            if (IsDescriptionValid(desc)) {
//                Description = desc;
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Set the max capacity of the server.
//        /// </summary>
//        public bool SetCapacity(byte cap) {
//            if (IsCapacityValid(cap)) {
//                Capacity = cap;
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Set the team mode of the lobby.
//        /// </summary>
//        public bool SetTeamMode(TeamMode mode) {
//            if (Enum.IsDefined(typeof(TeamMode), mode)) {
//                TeamMode = mode;
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Set the game mode of the lobby.
//        /// </summary>
//        public bool SetGameMode(GameMode mode) {
//            if (Enum.IsDefined(typeof(GameMode), mode)) {
//                GameMode = mode;
//                return true;
//            }
//            else {
//                return false;
//            }
//        }
//        #endregion

//        #region Helpers
//        /// <summary>
//        /// Generates a new deep copy of the settings.
//        /// </summary>
//        public NetLobbySettings GetCopy() {
//            return new NetLobbySettings(
//                string.Copy(this.Name),
//                string.Copy(this.Description),
//                this.Capacity,
//                this.TeamMode,
//                this.GameMode);
//        }

//        /// <summary>
//        /// Checks if the name is of proper length.
//        /// </summary>
//        public static bool IsNameValid(string name) {
//            //Ensure it actually has characters in it.
//            if (string.IsNullOrEmpty(name)) {
//                return false;
//            }

//            //Ensure no illegal characters are in it.
//            if (!StringUtils.IsAlphaNumeric(name)) {
//                return false;
//            }

//            //If the string is the correct length return true.
//            if (MathUtils.InRange(4, 33, name.Length)) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Checks if the desired description is short enough.
//        /// </summary>
//        public static bool IsDescriptionValid(string desc) {
//            //Ensure it actually has characters in it.
//            if (string.IsNullOrEmpty(desc)) {
//                return false;
//            }

//            //Ensure no illegal characters are in it.
//            if (!StringUtils.IsAlphaNumericWithPunctuation(desc)) {
//                return false;
//            }

//            //Ensure the length is correct.
//            if (MathUtils.InRange(4, 129, desc.Length)) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Check if the desired capacity is within range.
//        /// </summary>
//        public static bool IsCapacityValid(int cap) {
//            if (cap == 8 || cap == 16 || cap == 32) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Check if the inputted game mode is valid. 
//        /// </summary>
//        public static bool IsGameModeValid(GameMode mode) {
//            if (Enum.IsDefined(typeof(GameMode), mode)) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Check if the inputted team mode is valid or not.
//        /// </summary>
//        public static bool IsTeamModeValid(TeamMode mode) {
//            if (Enum.IsDefined(typeof(TeamMode), mode)) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Validate to ensure no wonky business went down with the data from file.
//        /// </summary>
//        [OnDeserialized]
//        public void Validate(StreamingContext context) {
//            //Ensure loaded in name is still good.
//            if (!IsNameValid(Name)) {
//                LoggerUtils.LogError("Server name from file was modified. Resetting to default.");
//                Name = "Server";
//            }

//            //Ensure loaded in description is still good.
//            if (!IsDescriptionValid(Description)) {
//                LoggerUtils.LogError("Server description from file was modified. Resetting to default.");
//                Description = "Description";
//            }

//            //Ensure the team mode is ok.
//            if (!IsTeamModeValid(TeamMode)) {
//                LoggerUtils.LogError("Server team mode from file was invalid. Resetting to default.");
//                TeamMode = TeamMode.Standard;
//            }

//            //Ensure the game mode is ok.
//            if (!IsGameModeValid(GameMode)) {
//                LoggerUtils.LogError("Server game mode from file was invalid. Resetting to default.");
//            }

//            //Ensure the capacity is correct.
//            if (!IsCapacityValid(Capacity)) {
//                LoggerUtils.LogError("Server capacity from file was modified. Resetting to default.");
//                Capacity = 32;
//            }
//        }
//        #endregion
//    }
//}