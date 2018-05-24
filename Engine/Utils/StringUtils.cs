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
    }
} 