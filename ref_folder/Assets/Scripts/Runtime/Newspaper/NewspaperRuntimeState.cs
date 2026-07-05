using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NewspaperRuntimeState
{
    [SerializeField] private List<NewspaperArticle> publishedArticles =
        new List<NewspaperArticle>();

    [SerializeField] private List<NewspaperEffectDefinition> publishedArticleEffects =
        new List<NewspaperEffectDefinition>();

    public IReadOnlyList<NewspaperArticle> PublishedArticles => publishedArticles;
    public IReadOnlyList<NewspaperEffectDefinition> PublishedArticleEffects => publishedArticleEffects;

    public void ClearPublishedArticleEffects()
    {
        publishedArticles.Clear();
        publishedArticleEffects.Clear();
    }

    public void PublishArticleEffects(IReadOnlyList<NewspaperArticle> articles)
    {
        if (articles == null)
            return;

        for (int i = 0; i < articles.Count; i++)
        {
            NewspaperArticle article = articles[i];

            if (article != null)
            {
                publishedArticles.Add(article);
                AddEffects(publishedArticleEffects, article.effects);
            }
        }
    }

    public NewspaperEffectDefinition FindPublishedEffect(string effectKey)
    {
        if (string.IsNullOrWhiteSpace(effectKey))
            return null;

        for (int i = 0; i < publishedArticleEffects.Count; i++)
        {
            NewspaperEffectDefinition effect = publishedArticleEffects[i];

            if (effect == null)
                continue;

            if (IsSameEffectKey(effect.EffectId, effectKey) ||
                IsSameEffectKey(effect.name, effectKey))
            {
                return effect;
            }
        }

        return null;
    }

    private bool IsSameEffectKey(string expected, string actual)
    {
        return !string.IsNullOrWhiteSpace(expected) &&
               string.Equals(
                   expected.Trim(),
                   actual.Trim(),
                   StringComparison.OrdinalIgnoreCase);
    }

    private void AddEffects(
        List<NewspaperEffectDefinition> target,
        IReadOnlyList<NewspaperEffectDefinition> source)
    {
        if (target == null || source == null)
            return;

        for (int i = 0; i < source.Count; i++)
        {
            NewspaperEffectDefinition effect = source[i];

            if (effect != null && !target.Contains(effect))
                target.Add(effect);
        }
    }
}
