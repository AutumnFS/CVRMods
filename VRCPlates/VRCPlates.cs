﻿using System.Collections;
using MelonLoader;
using UnityEngine;
using VRCPlates.Compatibility;

[assembly: MelonInfo(typeof(VRCPlates.VRCPlates), "VRCPlates", "1.0.8", "FS (Autumn)")]
[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
namespace VRCPlates;

public class VRCPlates : MelonMod
{
    private static readonly MelonLogger.Instance Logger = new("VRCPlates");

    public static NameplateManager? NameplateManager;

    public override void OnInitializeMelon()
    {
        AssetManager.Init();
        Settings.Init();
        Compat.Init();

        MelonCoroutines.Start(UIManagerInit());
    }

    private static IEnumerator UIManagerInit()
    {
        while (GameObject.Find("Cohtml/QuickMenu") == null) yield return null;
        try
        {
            NameplateManager = new NameplateManager();
            Patching.Patching.Init();
        }
        catch (Exception obj)
        {
            MelonLogger.Error("Unable to Apply Patches: " + obj);
        }
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (NameplateManager?.Nameplates == null) return;
        if (NameplateManager.Nameplates.Count <= 0) return;
        NameplateManager.ClearNameplates();
    }
    
    public override void OnPreferencesSaved()
    {
        if (NameplateManager?.Nameplates == null) return;

        foreach (var plate in NameplateManager.Nameplates)
        {
            MelonDebug.Msg("Applying Settings for user: " + plate.Key);

            if (plate.Value != null && plate.Value.Nameplate != null)
            {
                plate.Value.ApplySettings();
            }
        }
    }

    internal static void Log(object msg) => Logger.Msg(msg);

    internal static void Debug(object msg) {
        if (MelonDebug.IsEnabled())
            Logger.Msg(System.ConsoleColor.Cyan, msg);
    }

    internal static void Error(object obj) => Logger.Error(obj);

    internal static void DebugError(object obj) {
        if (MelonDebug.IsEnabled())
            Logger.Error($"[DEBUG] {obj}");
    }

    public static void Warning(string s)
    {
        Logger.Warning(s);
    }
}
