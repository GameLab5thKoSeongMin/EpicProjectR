public static class TutorialGate
{
    public static bool IsActionAllowed(string actionId)
    {
        TimelineSequenceController controller = TimelineSequenceController.Active;
        return controller == null || controller.IsActionAllowed(actionId);
    }

    public static bool TryPerformAction(string actionId)
    {
        TimelineSequenceController controller = TimelineSequenceController.Active;
        return controller == null || controller.TryPerformAction(actionId);
    }
}
