using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;


namespace BlackyFox.TextureReplacer.BML
{
    public static class BuildInfo
    {
        public const string Name = "TextureReplacer - BepInEx.MelonLoader.Loader";
        public const string Author = "BlackyFox";
        public const string Company = null;
        public const string Version = "0.1.0.0";
        public const string DownloadLink = null;
    }
    
   public struct ReplacedTexture
    {
        public byte[] hash;
        public Texture textures;
        public HashSet<int> instances;
    }
    public class TextureReplacer : MelonMod
    {
        //public static MelonPreferences_Category config;
        //public static MelonPreferences_Entry<bool> reloadChangedTextures;

        public static Dictionary<string, ReplacedTexture> loaded = new Dictionary<string, ReplacedTexture>();
        public static bool reloadChangedTextures;
        
        //private readonly string folderPath = Path.Combine(MelonEnvironment.UserDataDirectory, "CustomTextures\\"); //Melonloader 0.6.6
        private readonly string folderPath = Path.Combine(MelonUtils.UserDataDirectory, "CustomTextures\\"); //Melonloader 0.5.7
        private readonly string imageExtension = ".png";

        public override void OnInitializeMelon()
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            
            var category = MelonPreferences.CreateCategory("TextureReplacer", "");
            category.CreateEntry("reloadChangedTextures", false);
            reloadChangedTextures = category.GetEntry<bool>("reloadChangedTextures").Value;
            MelonPreferences.Save();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg($"Loading texture replacements for scene \"{sceneName}\".");

            HashSet<string> replacements = Directory.GetFiles(folderPath, "*" + imageExtension).Select(p => Path.GetFileNameWithoutExtension(p)).ToHashSet();
            LoggerInstance.Msg($"Found {replacements.Count} *{imageExtension} texture replacements in \"{folderPath}\".");
            
            Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
            LoggerInstance.Msg($"Found {textures.Length} Texture2Ds.");
            for (int i = 0; i < textures.Length; i++)
            {
                Texture2D texture = textures[i];
                string texName = texture.name;
                if (replacements.Contains(texName))
                {
                    string filePath = folderPath + texName + imageExtension;
                    if (!loaded.TryGetValue(texName, out ReplacedTexture replacedTexture))
                    {
                        LoggerInstance.Msg($"Replacement texture for \"{texName}\" has not yet been loaded, reading from file...");
                        byte[] data = File.ReadAllBytes(filePath);

                        if (!texture.LoadImage(data, true))
                        {
                            LoggerInstance.Msg($"Failed to load texture \"{texName}\".");
                        }
                        else
                        {
                            byte[] hash = { 0 };
                            if (reloadChangedTextures)
                            {
                                using (MD5 md5 = MD5.Create())
                                {
                                    hash = md5.ComputeHash(data);
                                }
                                LoggerInstance.Msg($"Calculated hash for \"{texName}\": {HashToString(hash)}");
                            }

                            HashSet<int> instances = new HashSet<int>
                            {
                                texture.GetInstanceID()
                            };
                            loaded[texName] = new ReplacedTexture { hash = hash, textures = texture, instances = instances };
                        }
                    }
                    else
                    {
                        foreach (var item in replacedTexture.instances)
                        {
                            LoggerInstance.Msg(item);
                        }

                        if (replacedTexture.instances.Add(texture.GetInstanceID()))
                        {
                            LoggerInstance.Msg($"Copying already replaced texture \"{texName}\", as a new Texture2D with the same name but a different ID was found...");
                            Graphics.CopyTexture(replacedTexture.textures, texture);
                        }
                        else if (reloadChangedTextures)
                        {
                            byte[] data = File.ReadAllBytes(filePath);
                            using (MD5 md5 = MD5.Create())
                            {
                                byte[] newHash = md5.ComputeHash(data);
                                ReplacedTexture loadedTexture = loaded[texName];
                                byte[] oldHash = loadedTexture.hash;
                                if (!newHash.SequenceEqual(oldHash))
                                {
                                    LoggerInstance.Msg($"Hash for \"{texName}\" changed! Old hash {HashToString(oldHash)}, new hash: {HashToString(newHash)}. Loading new texture...");
                                    textures[i].LoadImage(data);
                                    loadedTexture.hash = newHash;
                                    loaded[texName] = loadedTexture;
                                }
                                else
                                {
                                    LoggerInstance.Msg($"Hash for \"{texName}\" is unchanged. Skipping...");
                                }
                            }
                        }
                        else
                        {
                            LoggerInstance.Msg($"Replacement texture for \"{texName}\" already loaded and reloading is disabled, skipping...");
                        }
                    }
                }
            }
        }
        
        private string HashToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
    }
}