using System;
using System.IO;
using UnityEngine;


namespace DM
{
    public class LOIO
    {
        public static string[] LoadStreamingAsset(string filePath)
        {
            string fullPath = Application.streamingAssetsPath + filePath;

            if (File.Exists(fullPath))
            {
                return File.ReadAllLines(fullPath);
            }
            else
            {
                Debug.LogError($"File not found at: {fullPath}");
                return new string[] { };
            }
        }
    }
}
