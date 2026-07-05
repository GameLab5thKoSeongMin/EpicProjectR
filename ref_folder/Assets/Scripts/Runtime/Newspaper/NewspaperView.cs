using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewspaperView : MonoBehaviour
{
    [Serializable]
    private class ArticleSlot
    {
        public TMP_Text headlineText = null;
        public TMP_Text subtitleText = null;
        public TMP_Text bodyText = null;
        public Image iconImage = null;
        public Image image = null;

        public void Bind(NewspaperArticle article)
        {
            SetText(headlineText, article != null ? article.headline : string.Empty);
            SetText(subtitleText, article != null ? article.subtitle : string.Empty);
            SetText(bodyText, article != null ? article.body : string.Empty);
            SetImage(iconImage, article != null ? article.DisplayIcon : null);
            SetImage(image, article != null ? article.image : null);
        }

        private static void SetText(TMP_Text target, string value)
        {
            if (target != null)
                target.text = value;
        }

        private static void SetImage(Image target, Sprite sprite)
        {
            if (target == null)
                return;

            target.sprite = sprite;
            target.enabled = sprite != null;
        }
    }

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private List<ArticleSlot> articleSlots = new List<ArticleSlot>();

    public void Bind(string title, IReadOnlyList<NewspaperArticle> articles)
    {
        if (titleText != null)
            titleText.text = title;

        for (int i = 0; i < articleSlots.Count; i++)
        {
            NewspaperArticle article = articles != null && i < articles.Count
                ? articles[i]
                : null;

            if (articleSlots[i] != null)
                articleSlots[i].Bind(article);
        }
    }
}
