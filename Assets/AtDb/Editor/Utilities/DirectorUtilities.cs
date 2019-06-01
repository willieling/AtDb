using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtDb
{
    public static class DirectorUtilities
    {
        public static bool CreateDirectoryIfNeeded(string folderPath)
        {
            bool exists = Directory.Exists(folderPath);
            if(!exists)
            {
                Directory.CreateDirectory(folderPath);
            }
            return !exists;
        }
    }
}