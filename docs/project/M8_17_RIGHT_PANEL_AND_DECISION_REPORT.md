# M8.17 Right Panel And Decision Report

Status: Implemented with static validation.

## Right Reason Panel

Preserved: `FirstPlayablePresenter` renders only rule rows whose IDs are present in the current review triggers, so the right panel shows only applicable reasons for the current case.

Preserved: empty absolute-rejection and rejection-consideration sections are hidden.

Preserved: the removed title `서류에서 확인되는 사유` is not rendered by the current Presentation UI.

Implemented: the right shelf was narrowed and its padding reduced so paper-like reason rows fit inside the panel at 1920x1080. Row text now uses best-fit sizing to reduce horizontal overflow risk.

Preserved: normal reason rows show reason text only and do not display internal IDs such as `AR01` or `CR01`.

## Decision Paper

Preserved: the bottom-right trigger is a paper-like UI object, not a bell image.

Implemented: the decision drawer toggle now closes downward and then deactivates after the close tween. A generation guard prevents a delayed close from hiding a freshly reopened drawer.

Preserved: the open decision paper contains the title, compensation, premium, reject button, and approve/conditional-approve button.

## CR Behavior

Preserved: selected CR rows change the right button from approve to conditional approve.

Preserved: selected CR count drives `FirstPlayableMainLoopState.PremiumPercentForSelectedConsiderations`, which applies +10% premium per selected CR. Compensation text is not recalculated by CR selection.

## Incentive

Preserved: approval and conditional approval award floor 1% of final premium once per contract through the presenter's `HashSet<ContractId>`. Rejection awards no incentive.

## Post-Decision Loop

Preserved: after submission, the character appends a short line, the decision UI is hidden, the presented document disappears, character exit plays, and the next contract returns to Main waiting for the bell. The next review does not auto-open.
