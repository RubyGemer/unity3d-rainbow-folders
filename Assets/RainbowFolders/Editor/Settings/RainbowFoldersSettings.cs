﻿/*
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Borodar.RainbowFolders.Editor.Settings
{
    public class RainbowFoldersSettings : ScriptableObject
    {
        public const string RESOURCE_NAME = "RainbowFoldersSettings";
        public const string SETTINGS_ASSET_EXTENSION = "asset";

        public const string SETTINGS_FOLDER = "RainbowFolders";
        private static readonly string SETTINGS_PATH = Path.Combine("Editor Default Resources", SETTINGS_FOLDER);

        public List<RainbowFolder> Folders;

        public static RainbowFoldersSettings Load()
        {
            string assetNameWithExtension = string.Join (".", new [] 
            {
                RESOURCE_NAME,
                SETTINGS_ASSET_EXTENSION
            });

            var settings = LoadSettings(assetNameWithExtension);
            if (settings == null)
            {
                RainbowFoldersEditorUtility.CreateAsset<RainbowFoldersSettings>(RESOURCE_NAME, SETTINGS_PATH);
                settings = LoadSettings(assetNameWithExtension);
            }
            return settings;
        }

        private static RainbowFoldersSettings LoadSettings(string assetNameWithExtension)
        {
            return EditorGUIUtility.Load(Path.Combine(SETTINGS_FOLDER, assetNameWithExtension)) as RainbowFoldersSettings;
        }

        public Texture2D GetCustomFolderIcon(string folderPath, bool small = true)
        {
            var folder = GetFolderByKey(Folders, folderPath);
            if (folder == null) return null;

            return small ? folder.SmallIcon : folder.LargeIcon;
        }

        //---------------------------------------------------------------------
        // Helpers
        //---------------------------------------------------------------------

        private static bool IsNullOrEmpty(ICollection collection)
        {
            return collection == null || (collection.Count == 0);
        }

        private static RainbowFolder GetFolderByKey(List<RainbowFolder> folders, string folderPath)
        {
            if (IsNullOrEmpty(folders)) return null;

            foreach (var folder in folders)
            {
                switch (folder.Type)
                {
                    case RainbowFolder.KeyType.Name:
                        var folderName = Path.GetFileName(folderPath);
                        if (folder.Key.Equals(folderName)) return folder;
                        break;
                    case RainbowFolder.KeyType.Path:
                        if (folder.Key.Equals(folderPath)) return folder;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }
    }
}