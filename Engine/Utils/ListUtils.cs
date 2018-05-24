using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Utilities {
    /// <summary>
    /// Container class for various list helper
    /// methods. 
    /// </summary>
    public static class ListUtils {
        #region Insert Methods
        /// <summary>
        /// Copy the source list into the destination list starting at startIndex.
        /// If the destination list is not long enough it will be extended accordingly.
        /// </summary>
        /// <typeparam name="T">The type of lists being dealt with.</typeparam>
        /// <param name="source">The list to copy into the destination.</param>
        /// <param name="destination">The resulting array.</param>
        /// <param name="startIndex">Where to start copying in at.</param>
        public static void CopyInto<T>(List<T> source, List<T> destination, int startIndex) {
            for(int i = 0; i < source.Count; i++) {
                int targetIndex = i + startIndex;

                //If the destination already has an element at the index, overwrite it.
                if(destination.Count > targetIndex) {
                    destination[targetIndex] = source[i];
                }
                //Destination was not long enough, add to it.
                else {
                    destination.Add(source[i]);
                }
            }
        }

        /// <summary>
        /// Copy the source list into the destination list starting at startIndex.
        /// If the destination list is not long enough it will be extended accordingly.
        /// </summary>
        /// <typeparam name="T">The type of lists being dealt with.</typeparam>
        /// <param name="source">The array to copy into the destination.</param>
        /// <param name="destination">The resulting array.</param>
        /// <param name="startIndex">Where to start copying in at.</param>
        public static void CopyInto<T>(T[] source, List<T> destination, int startIndex) {
            for (int i = 0; i < source.Length; i++) {
                int targetIndex = i + startIndex;

                //If the destination already has an element at the index, overwrite it.
                if (destination.Count > targetIndex) {
                    destination[targetIndex] = source[i];
                }
                //Destination was not long enough, add to it.
                else {
                    destination.Add(source[i]);
                }
            }
        }
        #endregion
    }
}
