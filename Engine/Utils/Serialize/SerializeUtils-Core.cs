using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Voxelated.Serialization;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Net;

namespace Voxelated.Utilities {
    /// <summary>
    /// Helper class for converting objects to byte arrays
    /// and back.
    /// </summary>
    public static partial class SerializeUtils {
        /// <summary>
        /// This checks every single object that implements ISerializable
        /// and verifies that it has a constructor that accepts a byte[]. 
        /// If not an error is thrown.
        /// </summary>
        static SerializeUtils() {
            ////Run through each serializable object checking it has a constructor that accepts a byte array
            //foreach (Type serType in Assembly.GetExecutingAssembly().GetTypes()
            //                 .Where(serType => serType.GetInterfaces().Contains(typeof(ISerializable)))) {

            //    //Skip interfaces that implement this interface for obvious reasons..
            //    if (serType.IsInterface) {
            //        continue;
            //    }

            //    //Check if the type that implements the object has the proper constructor
            //    if (serType.GetConstructor(new Type[] { typeof(byte[]), typeof(int) }) == null) {
            //        throw new Exception(string.Format("{0} implements ISerializable therefore it must have a constructor that accepts a byte[]!", serType.Name));
            //    }
            //}
        }

    }
}
