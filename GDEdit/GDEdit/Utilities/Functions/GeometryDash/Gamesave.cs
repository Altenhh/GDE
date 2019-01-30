﻿using GDEdit.Utilities.Attributes;
using GDEdit.Utilities.Functions.Extensions;
using GDEdit.Utilities.Functions.General;
using GDEdit.Utilities.Objects.GeometryDash;
using GDEdit.Utilities.Objects.GeometryDash.General;
using GDEdit.Utilities.Objects.GeometryDash.LevelObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using static GDEdit.Application.Status;
using static GDEdit.Utilities.Information.GeometryDash.LevelObjectInformation;
using static System.Convert;

namespace GDEdit.Utilities.Functions.GeometryDash
{
    public static class Gamesave
    {
        public const string DefaultLevelString = "kS38,1_40_2_125_3_255_11_255_12_255_13_255_4_-1_6_1000_7_1_15_1_18_0_8_1|1_0_2_102_3_255_11_255_12_255_13_255_4_-1_6_1001_7_1_15_1_18_0_8_1|1_0_2_102_3_255_11_255_12_255_13_255_4_-1_6_1009_7_1_15_1_18_0_8_1|1_255_2_255_3_255_11_255_12_255_13_255_4_-1_6_1002_5_1_7_1_15_1_18_0_8_1|1_255_2_0_3_0_11_255_12_255_13_255_4_-1_6_1005_5_1_7_1_15_1_18_0_8_1|1_255_2_255_3_255_11_255_12_255_13_255_4_-1_6_1006_5_1_7_1_15_1_18_0_8_1|,kA13,0,kA15,0,kA16,0,kA14,,kA6,0,kA7,0,kA17,0,kA18,0,kS39,0,kA2,0,kA3,0,kA8,0,kA4,0,kA9,0,kA10,0,kA11,0;";
        public const string LevelDataStart = "<?xml version=\"1.0\"?><plist version=\"1.0\" gjver=\"2.0\"><dict><k>LLM_01</k><d><k>_isArr</k><t />";
        public const string LevelDataEnd = "</d><k>LLM_02</k><i>33</i></dict></plist>";
        public const string DefaultLevelData = "<?xml version=\"1.0\"?><plist version=\"1.0\" gjver=\"2.0\"><dict><k>LLM_01</k><d><k>_isArr</k><t /></d><k>LLM_02</k><i>33</i></dict></plist>";

        /// <summary>Returns the decrypted version of the gamesave after checking whether the gamesave is encrypted or not. Returns true if the gamesave is encrypted; otherwise false.</summary>
        /// <param name="decrypted">The string to return the decrypted gamesave.</param>
        /// <returns>Returns true if the gamesave is encrypted; otherwise false.</returns>
        public static bool TryDecryptGamesave(string gamesave, out string decrypted)
        {
            decrypted = gamesave;
            bool isEncrypted = CheckIfGamesaveIsEncrypted(gamesave);
            if (isEncrypted)
                decrypted = GDGamesaveDecrypt(gamesave);
            DoneDecryptingGamesave = true;
            return isEncrypted;
        }
        public static bool CheckIfGamesaveIsEncrypted(string gamesave)
        {
            int checks = 0;
            string[] tests = { "<k>bgVolume</k>", "<k>sfxVolume</k>", "<k>playerUDID</k>", "<k>playerName</k>", "<k>playerUserID</k>" };
            for (int i = 0; i < tests.Length; i++)
                if (gamesave.Contains(tests[i]))
                    checks++;
            return checks != tests.Length;
        }

        /// <summary>Returns the decrypted version of the level data after checking whether the level data is encrypted or not. Returns true if the level data is encrypted; otherwise false.</summary>
        /// <param name="decrypted">The string to return the decrypted level data.</param>
        /// <returns>Returns true if the level data is encrypted; otherwise false.</returns>
        public static bool TryDecryptLevelData(string levelData, out string decrypted)
        {
            decrypted = levelData;
            bool isEncrypted = CheckIfLevelDataIsEncrypted(levelData);
            if (isEncrypted)
                levelData = GDGamesaveDecrypt(levelData);
            decrypted = levelData;
            return isEncrypted;
        }
        public static bool CheckIfLevelDataIsEncrypted(string levelData)
        {
            string test = "<?xml version=\"1.0\"?><plist version=\"1.0\" gjver=\"2.0\">";
            return !levelData.Contains(test);
        }

        /// <summary>Returns the decrypted version of the level string after checking whether the level string is encrypted or not. Returns true if the level string is encrypted; otherwise false.</summary>
        /// <param name="ls">The level string to try decrypting. If it is already encrypted, it will be returned in the <paramref name="decrypted"/> parameter.</param>
        /// <param name="decrypted">The string to return the decrypted level string.</param>
        /// <returns>Returns true if the level string is encrypted; otherwise false.</returns>
        public static bool TryDecryptLevelString(string ls, out string decrypted)
        {
            bool isEncrypted = CheckIfLevelStringIsEncrypted(ls, out decrypted);
            if (isEncrypted)
                decrypted = DecryptLevelString(decrypted);
            return isEncrypted;
        }
        public static bool CheckIfLevelStringIsEncrypted(string ls, out string levelString)
        {
            levelString = ls;
            int checks = 0;
            string[] tests = { "kA13,", "kA15,", "kA16,", "kA14,", "kA6," };
            for (int i = 0; i < tests.Length; i++)
                if (levelString.Contains(tests[i]))
                    checks++;
            return checks != tests.Length;
        }
        public static string DecryptLevelString(string ls)
        {
            return GDLevelStringDecrypt(ls);
        }

        public static LevelObjectCollection GetObjects(string objectString)
        {
            List<GeneralObject> objects = new List<GeneralObject>();
            while (objectString.Length > 0 && objectString[objectString.Length - 1] == ';')
                objectString = objectString.Remove(objectString.Length - 1);
            if (objectString.Length > 0)
            {
                string[,] objectParameters = objectString.Split(';').Split(',');
                int length0 = objectParameters.GetLength(0);
                int length1 = objectParameters.GetLength(1);
                for (int i = 0; i < length0; i++)
                {
                    objects.Add(new GeneralObject(ToInt16(objectParameters[i, 1]), ToDouble(objectParameters[i, 3]), ToDouble(objectParameters[i, 5]))); // Get IDs of the selected objects
                    for (int j = 7; j < length1; j += 2)
                    {
                        if (objectParameters[i, j] != null)
                        {
                            try
                            {
                                int currentParameterID = ToInt32(objectParameters[i, j - 1]);
                                if (IntParameters.Contains(currentParameterID))
                                    objects[objects.Count - 1].SetParameterWithID(currentParameterID, ToInt32(objectParameters[i, j]));
                                else if (DoubleParameters.Contains(currentParameterID))
                                    objects[objects.Count - 1].SetParameterWithID(currentParameterID, ToDouble(objectParameters[i, j]));
                                else if (BoolParameters.Contains(currentParameterID))
                                    objects[objects.Count - 1].SetParameterWithID(currentParameterID, ToBoolean(ToInt32(objectParameters[i, j])));
                                else if (HSVParameters.Contains(currentParameterID))
                                {
                                    string[] values = objectParameters[i, j].ToString().Split('a');
                                    HSVAdjustment HSVValues = new HSVAdjustment(ToDouble(values[2]), ToDouble(values[3]), ToDouble(values[4]), (SVAdjustmentMode)ToInt32(values[0]), (SVAdjustmentMode)ToInt32(values[1]));
                                    objects[objects.Count - 1].SetParameterWithID(currentParameterID, HSVValues);
                                }
                                else if (currentParameterID == 31)
                                    objects[objects.Count - 1].SetParameterWithID(currentParameterID, Encoding.ASCII.GetString(Base64Decrypt(objectParameters[i, j])));
                                else if (IntArrayParameters.Contains(currentParameterID))
                                    objects[objects.Count - 1].SetParameterWithID(currentParameterID, objectParameters[i, j].ToString().Split('.').ToInt32Array());
                            }
                            catch (FormatException) // If the parameter is not just a number; most likely a Start Pos object
                            {
                                // Something to do, I guess
                            }
                        }
                        else break;
                    }
                }
                objectParameters = null;
            }
            return new LevelObjectCollection(objects);
        }
        public static GeneralObject GetCommonAttributes(LevelObjectCollection list, short objectID)
        {
            GeneralObject common = GeneralObject.GetNewObjectInstance(objectID);

            for (int i = list.Count; i >= 0; i--)
                if (list[i].ObjectID != objectID)
                    list.RemoveAt(i);
            if (list.Count > 1)
            {
                var properties = common.GetType().GetProperties();
                foreach (var p in properties)
                    if (Attribute.GetCustomAttributes(p, typeof(ObjectStringMappableAttribute), false).Count() > 0)
                    {
                        var v = p.GetValue(list[0]);
                        bool isCommon = true;
                        foreach (var o in list)
                            if (isCommon = (p.GetValue(o) != v))
                                break;
                        if (isCommon)
                            p.SetValue(common, v);
                    }
                return common;
            }
            else if (list.Count == 1)
                return list[0];
            else
                return null;
        }
        public static bool GetBoolKeyValue(string level, int key)
        {
            return level.Find($"<k>k{key}</k>") > -1;
        }
        public static int GetGuidelineStringStartIndex(string ls)
        {
            return ls.Find("kA14,") + 5;
        }
        public static string GetGuidelineString(string ls)
        {
            int guidelinesStartIndex = ls.Find("kA14,") + 5;
            int guidelinesEndIndex = ls.Find(",kA6");
            int guidelinesLength = guidelinesEndIndex - guidelinesStartIndex;
            return ls.Substring(guidelinesStartIndex, guidelinesLength);
        }

        public static string GetKeyValue(string level, int key, string valueType)
        {
            string startKeyString = $"<k>k{key}</k><{valueType}>";
            string endKeyString = $"</{valueType}>";

            int parameterStartIndex, parameterEndIndex, parameterLength;
            parameterStartIndex = level.Find(startKeyString, 0, level.Length - 1) + startKeyString.Length;
            parameterEndIndex = level.Find(endKeyString, parameterStartIndex, level.Length - 1);
            parameterLength = parameterEndIndex - parameterStartIndex;
            if (parameterStartIndex == startKeyString.Length - 1)
                throw new KeyNotFoundException();
            return level.Substring(parameterStartIndex, parameterLength);
        }
        public static string GetLevelString(string level)
        {
            try
            {
                return GetKeyValue(level, 4, "s");
            }
            catch (KeyNotFoundException)
            {
                return "";
            }
        }
        public static string GetObjectString(string ls)
        {
            try
            {
                return ls.Split(';').RemoveAt(0).Combine(";");
            }
            catch
            {
                return "";
            }
        }
        public static string RemoveLevelIndexKey(string level)
        {
            string result = level;
            int keyIndex = result.Find("<k>k_");
            if (keyIndex > -1)
            {
                int endIndex = result.Find("</k>", keyIndex, result.Length) + 4;
                result = result.Remove(keyIndex, endIndex);
            }
            return result;
        }
        public static string TryGetKeyValue(string level, int key, string valueType, string defaultValueOnException)
        {
            string startKeyString = $"<k>k{key}</k><{valueType}>";
            string endKeyString = $"</{valueType}>";
            int parameterStartIndex = level.Find(startKeyString) + startKeyString.Length;
            int parameterEndIndex = level.Find(endKeyString, parameterStartIndex, level.Length - 1);
            int parameterLength = parameterEndIndex - parameterStartIndex;
            if (parameterStartIndex == startKeyString.Length - 1)
                return defaultValueOnException;
            else
                return level.Substring(parameterStartIndex, parameterLength);
        }
        
        public static string GetData(string path)
        {
            StreamReader sr = new StreamReader(path);
            string readfile = sr.ReadToEnd();
            sr.Close();
            return readfile;
        }

        public static byte[] Base64Decrypt(string encodedData)
        {
            while (encodedData.Length % 4 != 0)
                encodedData += "=";
            byte[] encodedDataAsBytes = FromBase64String(encodedData.Replace('-', '+').Replace('_', '/').Replace("\0", string.Empty));
            return encodedDataAsBytes;
        }
        public static string Base64Encrypt(byte[] decryptedData)
        {
            return ToBase64String(decryptedData);
        }
        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                var buffer = new byte[4096];
                int read;

                while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                    resultStream.Write(buffer, 0, read);

                return resultStream.ToArray();
            }
        }
        public static byte[] Compress(byte[] data)
        {
            using (var decompressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(decompressedStream, CompressionMode.Compress))
            using (var resultStream = new MemoryStream())
            {
                var buffer = new byte[4096];
                int read;

                while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                    resultStream.Write(buffer, 0, read);

                return resultStream.ToArray();
            }
        }
        public static string XORDecrypt(string text, int key)
        {
            byte[] result = new byte[text.Length];
            for (int c = 0; c < text.Length; c++)
                result[c] = (byte)((uint)text[c] ^ key);
            string dexored = Encoding.UTF8.GetString(result);
            return dexored;
        }
        public static string XOR11Decrypt(string text)
        {
            byte[] result = new byte[text.Length];
            for (int c = 0; c < text.Length - 1; c++)
                result[c] = (byte)((uint)text[c] ^ 11);
            string dexored = Encoding.UTF8.GetString(result);
            return dexored;
        }
        public static string GDGamesaveDecrypt(string data)
        {
            string xored = XOR11Decrypt(data); // Decrypt XOR ^ 11
            string replaced = xored.Replace('-', '+').Replace('_', '/').Replace("\0", string.Empty); // Replace characters
            byte[] gzipb64 = Decompress(Base64Decrypt(replaced)); // Decompress
            string result = Encoding.ASCII.GetString(gzipb64); // Change to string
            return result;
        }
        public static string GDLevelStringDecrypt(string ls)
        {
            string replaced = ls.Replace('-', '+').Replace('_', '/').Replace("\0", string.Empty); // Replace characters
            //string fixedBase64 = replaced.FixBase64String();
            byte[] gzipb64 = Base64Decrypt(replaced);
            if (replaced.StartsWith("H4sIAAAAAAAA"))
                gzipb64 = Decompress(gzipb64); // Decompress
            else throw new ArgumentException("The level string is not valid.");
            string result = Encoding.ASCII.GetString(gzipb64); // Change to string
            return result;
        }
    }
}