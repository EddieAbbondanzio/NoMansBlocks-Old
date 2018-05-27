using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Voxelated.Utilities {
    /// <summary>
    /// String helper method class.
    /// </summary>
    public static class StringUtils {
        #region Format Checks
        /// <summary>
        /// Checks if the string only contains alphanumeric characters.
        /// </summary>
        /// <param name="input">The string to test.</param>
        /// <returns>True if the string only contains A-Z, or 0-9.</returns>
        public static bool IsAlphaNumeric(string input) {
            if(input == null) {
                return false;
            }

            Regex r = new Regex("^[a-zA-Z0-9\x20]*$");
            return r.IsMatch(input);
        }

        /// <summary>
        /// Checks if the string only contains alphabetic characters.
        /// </summary>
        /// <param name="input">The string to test.</param>
        /// <returns>True if the string only contains A-Z.</returns>
        public static bool IsAlphabetic(string input) {
            if (input == null) {
                return false;
            }

            Regex r = new Regex("^[a-zA-Z\x20]*$");
            return r.IsMatch(input);
        }

        /// <summary>
        /// Checks if the string contains only alphanumeric characters,
        /// and or punctuation.
        /// </summary>
        /// <param name="input">The string to test.</param>
        /// <returns>True if the string only contains A-Z, 0-9, or .,!-</returns>
        public static bool IsAlphaNumericWithPunctuation(string input) {
            if (input == null) {
                return false;
            }

            Regex r = new Regex("^[a-zA-Z\x20\\.\\-\\!\\,]*$");
            return r.IsMatch(input);
        }
        #endregion

        #region Clamps
        /// <summary>
        /// Ensure a string is never longer than the length, but permit
        /// them to be shorter as well.
        /// </summary>
        /// <param name="str">The string to clamp.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// <returns>The substring.</returns>
        public static string Clamp(string str, int maxLength) {
            if (str.Length > maxLength) {
                return str.Substring(0, maxLength);
            }
            else {
                return str;
            }
        }
        #endregion
    }
} 