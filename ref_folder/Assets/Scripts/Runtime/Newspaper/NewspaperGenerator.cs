using System.Collections.Generic;
using UnityEngine;

public class NewspaperGenerator : MonoBehaviour
{
    [SerializeField] private NewspaperView newspaperPrefab;
    [SerializeField] private Transform newspaperRoot;
    [SerializeField] private NewspaperArticlePoolDefinition articlePool;
    [SerializeField] private string newspaperTitle = "Daily Chronicle";
    [SerializeField] private int articleCount = 2;
    [SerializeField] private bool publishGeneratedEffects = true;

    private readonly List<NewspaperView> generatedNewspapers = new List<NewspaperView>();

    public NewspaperView GenerateNewspaper()
    {
        return GenerateNewspaper(newspaperRoot);
    }

    public NewspaperView RegenerateNewspaper()
    {
        ClearGeneratedNewspapers();

        if (NewspaperRuntimeContext.Current != null)
            NewspaperRuntimeContext.Current.ClearPublishedNewspapers();

        return GenerateNewspaper();
    }

    public void ClearGeneratedNewspapers()
    {
        for (int i = generatedNewspapers.Count - 1; i >= 0; i--)
        {
            NewspaperView view = generatedNewspapers[i];

            if (view != null)
            {
                view.gameObject.SetActive(false);
                Destroy(view.gameObject);
            }
        }

        generatedNewspapers.Clear();
    }

    public NewspaperView GenerateNewspaper(Transform root)
    {
        if (root == null || newspaperPrefab == null)
            return null;

        NewspaperView view = Instantiate(newspaperPrefab, root);
        generatedNewspapers.Add(view);

        List<NewspaperArticle> selectedArticles = PickArticles();
        view.Bind(newspaperTitle, selectedArticles);

        if (publishGeneratedEffects)
            PublishArticleEffects(selectedArticles);

        return view;
    }

    public void GenerateNewspaperAt(Transform root)
    {
        GenerateNewspaper(root);
    }

    public void GenerateNewspaperAtDefaultRoot()
    {
        GenerateNewspaper();
    }

    private List<NewspaperArticle> PickArticles()
    {
        List<NewspaperArticle> pool = new List<NewspaperArticle>();

        if (articlePool != null)
            articlePool.CollectArticles(pool);

        List<NewspaperArticle> selected = new List<NewspaperArticle>();
        int targetCount = Mathf.Max(0, articleCount);

        for (int i = 0; i < targetCount; i++)
        {
            NewspaperArticle article = PickRandomArticle(pool);

            if (article == null)
                break;

            selected.Add(article);
        }

        return selected;
    }

    private NewspaperArticle PickRandomArticle(List<NewspaperArticle> pool)
    {
        if (pool == null || pool.Count == 0)
            return null;

        int index = Random.Range(0, pool.Count);
        NewspaperArticle article = pool[index];
        pool.RemoveAt(index);
        return article;
    }

    private void PublishArticleEffects(IReadOnlyList<NewspaperArticle> articles)
    {
        NewspaperRuntimeContext context = NewspaperRuntimeContext.Current;

        if (context == null)
            return;

        context.PublishArticleEffects(articles);
    }
}
