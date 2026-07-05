using UnityEngine;

public class NewspaperRuntimeContext : MonoBehaviour
{
    [SerializeField] private NewspaperRuntimeState newspaperState = new NewspaperRuntimeState();

    public static NewspaperRuntimeContext Current { get; private set; }
    public NewspaperRuntimeState State
    {
        get
        {
            if (newspaperState == null)
                newspaperState = new NewspaperRuntimeState();

            return newspaperState;
        }
    }

    private void Awake()
    {
        if (newspaperState == null)
            newspaperState = new NewspaperRuntimeState();

        newspaperState.ClearPublishedArticleEffects();
        Current = this;
    }

    public void PublishArticleEffects(System.Collections.Generic.IReadOnlyList<NewspaperArticle> articles)
    {
        State.PublishArticleEffects(articles);
    }

    public void ClearPublishedNewspapers()
    {
        State.ClearPublishedArticleEffects();
    }

    private void OnDestroy()
    {
        if (Current == this)
            Current = null;
    }

    public static NewspaperRuntimeState GetState()
    {
        return Current != null ? Current.State : null;
    }
}
