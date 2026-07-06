# M8.16 Review Layout And Chat Report

Status: Implemented with static validation.

## Review Transition

Preserved: clicking the presented document after character arrival enters review. The view still opens review through tweened movement of the dialogue panel, central workbench, right shelf, and contractor, so the transition reads as a layout/camera shift rather than a static hard panel swap.

Preserved: the presented document in the review left composition is the only return-to-Main trigger. Workstation document cards still attach only `FirstPlayableDragController` and do not submit return/advance events.

## Layout Proportions

Implemented: `FirstPlayableMainSceneRectSpec` was tuned around a 1920x1080 reference:

- left composition remains about 30% width,
- center workstation is the largest area at about 48% width,
- right panel is narrowed to about 17% width,
- HUD height is reserved above the review surfaces.

## Left Area

Implemented: the chat panel occupies the upper left area and is taller than before. The lower left composition keeps the contractor, inactive bell, and presented document visible below the chat region.

Implemented: the contractor review position was raised so it is not hidden behind the chat panel.

## Chat Dialogue

Preserved: dialogue is an accumulated scrollable log of message bubbles, not one giant speech bubble.

Implemented: mouse-wheel scrolling sensitivity was increased on the dialogue `ScrollRect`. Clicking the dialogue panel still appends the next dialogue line, and previous lines remain in the scroll content.

Preserved: no visible scrollbar is required or created.

## Center Workstation

Preserved: multiple documents render on the central workstation and remain draggable. Drag clamp bounds still use the document board rect.

Preserved: normal document cards avoid visible internal document IDs and status clutter.
