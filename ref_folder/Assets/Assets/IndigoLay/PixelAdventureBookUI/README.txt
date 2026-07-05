Pixel Adventure Book UI: Fantasy RPG Kit
=========================================

Thank you for purchasing Pixel Adventure Book UI: Fantasy RPG Kit!
This document will help you get started quickly.


HOW TO IMPORT
-------------

1. Open your Unity project (URP 2D template recommended).
2. Go to Assets > Import Package > Custom Package...
3. Select the .unitypackage file.
4. Click Import All.
5. The asset will be located at Assets/IndigoLay/PixelAdventureBookUI/.
6. A dialog will automatically appear asking you to import
   TMP Essential Resources if they are not yet installed.
   Click "Import TMP Essentials" to proceed.
   (You can also import manually via
   Window > TextMeshPro > Import TMP Essential Resources.)


FOLDER STRUCTURE
----------------

Assets/IndigoLay/PixelAdventureBookUI/
|
|-- Animation/                  UI animations
|-- Demo/                       Demo scene
|-- Editor/                     Editor utilities (TMP auto-check)
|-- Font/                       Noto Serif font (Regular, Medium, Bold + SDF)
|-- Prefabs/                    9 ready-to-use prefabs
|-- Script/                     Tab switching script
|
+-- Sprites/
    |-- Features/
    |   +-- Bonus/              (6)  Letter UI set
    |-- Icons/
    |   |-- Items/              (6)  Item icons
    |   |-- Pets/               (8)  Creature / pet icons
    |   +-- System/             (82) System UI icons
    |-- Thumbnails/             (9)  Thumbnail and NPC portrait images
    +-- UI/
        |-- Backgrounds/        (4)  Page and panel backgrounds
        |-- Badges/             (4)  Status and rank badges
        |-- Buttons/            (17) Button set
        |-- Controls/           (21) Sliders, toggles, checkboxes, dropdowns
        |-- Decorations/        (7)  Decorative elements (ribbons, dividers)
        |-- InputFields/        (1)  Text input field
        |-- Lists/              (6)  List item components
        |-- Panels/             (4)  Panel frames
        +-- Tabs/               (8)  Tab UI elements

Total sprites: 183


PREFABS (9)
-----------

  Prefab          Description
  --------------- ---------------------------------------------------
  Quest           Quest list and detail page
  Relationships   NPC affinity and relationship status page
  Creature        Creature / pet encyclopedia page
  Save_Load       Game save and load page
  Settings        Game settings and options page
  Popup           Confirmation, warning, and selection dialogs
  Letter          Bonus letter UI set page
  Component       Component overview reference page
  SystemIcons     System icon preview page

All prefabs are ready to use. Drag and drop them into your scene.


KEY FEATURES
------------

  9-Slice
    Resizable resources (panels, buttons, frames, etc.) have 9-slice
    applied. Scale them freely without breaking corners or borders.

  Tab Switching
    Tab UI includes a scripted switching system (TabController).
    Tabs work immediately after import with no additional coding required.
    The script is located under the IndigoLay.PixelAdventureBookUI namespace.

  Animations
    UI animations are included and ready to use.

  Font
    Noto Serif font is included in Regular, Medium, and Bold weights
    with pre-generated TextMesh Pro SDF assets.


BONUS: LETTER UI SET
---------------------

A bonus Letter UI Set is included in Sprites/Features/Bonus/:

  - Letter paper
  - Envelope
  - Wax seal
  - Stamp images
  - Quest complete stamp mark

The Letter prefab is also included for immediate use.


TEXTMESHPRO AUTO-CHECK
----------------------

This asset includes an Editor script that automatically detects whether
TMP Essential Resources have been imported. On first load after importing
this package, a dialog will prompt you to import them if they are missing.

This prevents rendering errors (TMP Cull) that can occur when Canvas
components try to render TextMeshPro elements before the required
resources are available.

  - The check runs once per editor session (not on every script reload).
  - If you dismiss the dialog, you can always import manually via
    Window > TextMeshPro > Import TMP Essential Resources.
  - The script is located at Editor/TMP_EssentialResourcesChecker.cs
    and runs only in the Unity Editor (not included in builds).


SCRIPTING REFERENCE
-------------------

  Namespace: IndigoLay.PixelAdventureBookUI

  TabController
    A MonoBehaviour that manages tab-based UI navigation.
    Assign tab button-panel pairs in the Inspector, along with
    normal / selected sprites for visual feedback.

    Public fields:
      tabs             TabPair[]   Tab button-panel pairs
      normalSprite     Sprite      Sprite for inactive tabs
      selectedSprite   Sprite      Sprite for the active tab

    Public methods:
      SelectTab(int index)   Selects the tab at the given index


REQUIREMENTS
------------

  - Unity 6 (6000.x) or later
  - Universal Render Pipeline (URP) 2D
  - TextMesh Pro (included with Unity)


THIRD-PARTY LICENSES
---------------------

  Noto Serif Font
    Copyright 2022 The Noto Project Authors
    (https://github.com/googlefonts/noto-fonts)
    Licensed under the SIL Open Font License, Version 1.1.
    See Font/LICENSE_NotoSerif.txt for the full license text.


NOTES
-----

  - This asset provides UI sprite resources, prefabs, animations,
    a font, and a tab switching script.
  - Game logic scripts beyond tab switching are not included.
  - Designed for Unity 2D projects using URP.


SUPPORT
-------

If you have any questions or feedback, feel free to reach out!

  Email : lattemongling@gmail.com


COPYRIGHT
---------

Copyright (c) IndigoLay. All rights reserved.
Licensed under the Unity Asset Store End User License Agreement.
See https://unity.com/legal/as-terms for details.
