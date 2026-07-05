using System;
using System.Collections.Generic;
using UnityEngine;

public enum NewspaperArticleKind
{
    Story,
    Rumor,
    Advertisement,
    Warning
}

[CreateAssetMenu(menuName = "Underwriting/Newspaper/Article Pool")]
public class NewspaperArticlePoolDefinition : ScriptableObject
{
    [SerializeField] private List<NewspaperArticle> meaninglessArticles =
        new List<NewspaperArticle>();
    [SerializeField] private List<NewspaperArticle> effectArticles =
        new List<NewspaperArticle>();

    public IReadOnlyList<NewspaperArticle> MeaninglessArticles => meaninglessArticles;
    public IReadOnlyList<NewspaperArticle> EffectArticles => effectArticles;

    public void CollectArticles(List<NewspaperArticle> result)
    {
        if (result == null)
            return;

        AddArticles(result, meaninglessArticles);
        AddArticles(result, effectArticles);
    }

    private void AddArticles(
        List<NewspaperArticle> result,
        IReadOnlyList<NewspaperArticle> source)
    {
        if (source == null)
            return;

        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] != null)
                result.Add(source[i]);
        }
    }
}

[Serializable]
public class NewspaperArticle
{
    public NewspaperArticleKind kind = NewspaperArticleKind.Story;
    public string headline;
    public string subtitle;
    public Sprite iconSprite;

    [TextArea]
    public string body;

    public Sprite image;
    public List<NewspaperEffectDefinition> effects = new List<NewspaperEffectDefinition>();

    public Sprite DisplayIcon
    {
        get { return iconSprite != null ? iconSprite : image; }
    }
}

