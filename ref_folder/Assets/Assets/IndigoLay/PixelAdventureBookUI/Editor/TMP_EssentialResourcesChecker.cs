// Copyright (c) IndigoLay. All rights reserved.
// Licensed under the Unity Asset Store EULA.
// See https://unity.com/legal/as-terms for details.

using UnityEditor;
using UnityEngine;

namespace IndigoLay.PixelAdventureBookUI.Editor
{
    /// <summary>
    /// Automatically checks whether TextMeshPro Essential Resources have been imported
    /// when this asset package is first loaded. If missing, prompts the user to import them
    /// before the Canvas rendering errors (TMP Cull) block the normal TMP import popup.
    /// </summary>
    [InitializeOnLoad]
    public static class TMP_EssentialResourcesChecker
    {
        private const string SessionKey = "IndigoLay_TMPEssentialCheck";

        static TMP_EssentialResourcesChecker()
        {
            // Run once per editor session to avoid repeated popups.
            if (SessionState.GetBool(SessionKey, false)) return;
            SessionState.SetBool(SessionKey, true);

            // Delay until editor is fully initialized.
            EditorApplication.delayCall += CheckTMPResources;
        }

        private static void CheckTMPResources()
        {
            // TMP_Settings asset is part of Essential Resources.
            // If it exists, resources are already imported.
            string[] guids = AssetDatabase.FindAssets("t:TMP_Settings");
            if (guids != null && guids.Length > 0) return;

            bool import = EditorUtility.DisplayDialog(
                "Pixel Adventure Book UI - TextMeshPro Required",
                "This asset uses TextMeshPro components.\n\n" +
                "TMP Essential Resources (fonts, shaders, materials) must be imported " +
                "for the UI to render correctly.\n\n" +
                "Would you like to import them now?",
                "Import TMP Essentials",
                "Later");

            if (import)
            {
                OpenTMPImportWindow();
            }
        }

        private static void OpenTMPImportWindow()
        {
            // Try the package version importer (Unity 2018.3+ with TMP as package).
            var windowType = System.Type.GetType(
                "TMPro.EditorUtilities.TMP_PackageResourceImporterWindow, Unity.TextMeshPro.Editor");

            if (windowType != null)
            {
                EditorWindow.GetWindow(windowType, true, "TMP Importer", true);
                return;
            }

            // Fallback: try via menu item.
            if (EditorApplication.ExecuteMenuItem("Window/TextMeshPro/Import TMP Essential Resources"))
            {
                return;
            }

            // Last resort: inform the user manually.
            EditorUtility.DisplayDialog(
                "Manual Import Required",
                "Could not open the TMP importer automatically.\n\n" +
                "Please go to:\n" +
                "Window > TextMeshPro > Import TMP Essential Resources",
                "OK");
        }
    }
}
