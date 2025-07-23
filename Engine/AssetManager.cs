using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Engine.Components2d;
using Raylib_cs;
using Engine.Logging;

namespace Engine;

public static class AssetManager
{
    private static readonly Dictionary<string, Texture2D> Textures = new();
    private static readonly Dictionary<string, Sound> Sounds = new();
    private static readonly Dictionary<string, Music> Songs = new();
    private static readonly Dictionary<string, Font> Fonts = new();
    private static ResourceManager? ResourceManager;

    public static async Task LoadAllTextures(string folderPath = "Assets/", string keyPrefix = "", TextureFilter filterMode = TextureFilter.Point, bool recursive = true, IProgress<float>? progress = null)
    {
        if (!Directory.Exists(folderPath))
        {
            Log.Warning($"{nameof(LoadAllTextures)} - Directory not found: {folderPath}");
            return;
        }
        
        var textureFiles = Directory.GetFiles(folderPath, "*.png", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Concat(Directory.GetFiles(folderPath, "*.jpg", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            .Concat(Directory.GetFiles(folderPath, "*.jpeg", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            .ToArray();
        
        if (textureFiles.Length == 0)
        {
            Log.Warning($"{nameof(LoadAllTextures)} - No textures found in path: {folderPath}");
            return;
        }

        var totalFiles = textureFiles.Length;
        for (var i = 0; i < textureFiles.Length; i++)
        {
            var textureFile = textureFiles[i];
            LoadTexture(textureFile, $"{keyPrefix}{Path.GetFileNameWithoutExtension(textureFile)}", filterMode);

            var progressPercent = i + 1f / totalFiles;
            progress?.Report(progressPercent);
            
            if (progressPercent % 1 == 0)
                await Task.Yield();
        }
    }
    
    public static async Task LoadAllFonts(string folderPath = "Assets/", string keyPrefix = "", int fontSize = 64, bool recursive = true, IProgress<float>? progress = null)
    {
        if (!Directory.Exists(folderPath))
        {
            Log.Warning($"{nameof(LoadAllFonts)} - Directory not found: {folderPath}");
            return;
        }

        var fontFiles = Directory.GetFiles(folderPath, "*.ttf", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        
        if (fontFiles.Length == 0)
        {
            Log.Warning($"{nameof(LoadAllFonts)} - No fonts found in path: {folderPath}");
            return;
        }

        var totalFiles = fontFiles.Length;
        for (var i = 0; i < fontFiles.Length; i++)
        {
            var fontFile = fontFiles[i];
            LoadFont(fontFile, $"{keyPrefix}{Path.GetFileNameWithoutExtension(fontFile)}", fontSize);

            var progressPercent = i + 1f / totalFiles;
            progress?.Report(progressPercent);
            
            if (progressPercent % 1 == 0)
                await Task.Yield();
        }
    }
    
    public static Texture2D LoadTexture(string path, string key = "", TextureFilter filterMode = TextureFilter.Point)
    {
        var asset = Raylib.LoadTexture(path);
        Raylib.SetTextureFilter(asset, filterMode);
        Textures[string.IsNullOrEmpty(key) ? Path.GetFileNameWithoutExtension(path) : key] = asset;
        return asset;
    }
    
    public static Texture2D GetTexture(string key)
    {
        if (Textures.TryGetValue(key, out var asset))
            return asset;
        throw new KeyNotFoundException($"Texture with key '{key}' was not found.");
    }

    public static void UnloadTexture(string key)
    {
        var asset = Textures[key];
        Raylib.UnloadTexture(asset);
        Textures.Remove(key);
    }
    
    public static Sound LoadSound(string path, string key = "")
    {
        var asset = Raylib.LoadSound(path);
        Sounds[string.IsNullOrEmpty(key) ? Path.GetFileNameWithoutExtension(path) : key] = asset;
        return asset;
    }
    
    public static SoundEffect GetSound(string key, float volume = 1f, float pitch = 1f)
    {
        if (Sounds.TryGetValue(key, out var asset))
        {
            var sound = new SoundEffect(asset, volume, pitch);
            return sound;
        }
        throw new KeyNotFoundException($"Sound with key '{key}' was not found.");
    }

    public static void UnloadSound(string key)
    {
        var asset = Sounds[key];
        Raylib.UnloadSound(asset);
        Sounds.Remove(key);
    }
    
    public static Music LoadSong(string path, string key = "")
    {
        var asset = Raylib.LoadMusicStream(path);
        Songs[string.IsNullOrEmpty(key) ? Path.GetFileNameWithoutExtension(path) : key] = asset;
        return asset;
    }
    
    public static Song GetSong(string key, float volume = 1f, float pitch = 1f, bool loop = true)
    {
        if (Songs.TryGetValue(key, out var asset))
            return new Song(asset, volume, pitch, loop);
        throw new KeyNotFoundException($"Song with key '{key}' was not found.");
    }

    public static void UnloadSong(string key)
    {
        var asset = Songs[key];
        Raylib.UnloadMusicStream(asset);
        Songs.Remove(key);
    }

    internal static void UnloadAllAssets()
    {
        foreach (var texture in Textures.Values)
            Raylib.UnloadTexture(texture);
        Textures.Clear();
        
        foreach (var sound in Sounds.Values)
            Raylib.UnloadSound(sound);
        Sounds.Clear();
        
        foreach (var song in Songs.Values)
            Raylib.UnloadMusicStream(song);
        Songs.Clear();
    }

    public static void LoadLocalizationResource(string resourceName, CultureInfo? cultureInfo = null)
    {
        ResourceManager = new ResourceManager(resourceName, Assembly.GetEntryAssembly()!);
        if (cultureInfo != null)
            CultureInfo.CurrentUICulture = cultureInfo;
    }

    public static string GetLocalizedText(string key)
    {
        return ResourceManager?.GetString(key) ?? string.Empty;
    }

    public static Font LoadFont(string path, string key, int fontSize = 64)
    {
        var asset = Raylib.LoadFontEx(path, fontSize, null, 0);
        Fonts[string.IsNullOrEmpty(key) ? Path.GetFileNameWithoutExtension(path) : key] = asset;
        return asset;
    }
    
    public static void UnloadFont(string key)
    {
        var asset = Fonts[key];
        Raylib.UnloadFont(asset);
        Fonts.Remove(key);
    }
    
    public static Font GetFont(string key)
    {
        if (Fonts.TryGetValue(key, out var asset))
            return asset;
        throw new KeyNotFoundException($"Font with key '{key}' was not found.");
    }
}