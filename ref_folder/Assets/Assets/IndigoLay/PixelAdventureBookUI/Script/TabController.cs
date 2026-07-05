// Copyright (c) IndigoLay. All rights reserved.
// Licensed under the Unity Asset Store EULA.
// See https://unity.com/legal/as-terms for details.

using UnityEngine;
using UnityEngine.UI;

namespace IndigoLay.PixelAdventureBookUI
{
    /// <summary>
    /// Controls tab-based UI navigation by switching between tab buttons and their associated panels.
    /// Assign tab button-panel pairs in the Inspector, along with normal/selected sprites for visual feedback.
    /// </summary>
    public class TabController : MonoBehaviour
    {
        [System.Serializable]
        public struct TabPair
        {
            [Tooltip("The button used to select this tab.")]
            public Button tabButton;

            [Tooltip("The panel displayed when this tab is selected.")]
            public GameObject tabPanel;
        }

        [Tooltip("List of tab button-panel pairs.")]
        public TabPair[] tabs;

        [Tooltip("Sprite used for inactive (unselected) tabs.")]
        public Sprite normalSprite;

        [Tooltip("Sprite used for the active (selected) tab.")]
        public Sprite selectedSprite;

        private void Start()
        {
            // Select the first tab on initialization.
            SelectTab(0);
        }

        /// <summary>
        /// Selects the tab at the given index, activating its panel and updating button sprites.
        /// </summary>
        /// <param name="index">Zero-based index of the tab to select.</param>
        public void SelectTab(int index)
        {
            if (tabs == null || tabs.Length == 0) return;

            index = Mathf.Clamp(index, 0, tabs.Length - 1);

            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i].tabButton == null || tabs[i].tabPanel == null) continue;

                bool isSelected = (i == index);

                // Update button sprite to reflect selection state.
                tabs[i].tabButton.image.sprite = isSelected ? selectedSprite : normalSprite;

                // Show the selected panel, hide the rest.
                tabs[i].tabPanel.SetActive(isSelected);
            }
        }
    }
}
