public class TutorialController : TimelineSequenceController
{
    public static new TutorialController Active => TimelineSequenceController.Active as TutorialController;

    public bool TutorialEnabled => SequenceEnabled;

    public void StartTutorial()
    {
        StartSequence();
    }

    public void StopTutorial()
    {
        StopSequence();
    }

    public void SetTutorialEnabled(bool value)
    {
        SetSequenceEnabled(value);
    }
}
