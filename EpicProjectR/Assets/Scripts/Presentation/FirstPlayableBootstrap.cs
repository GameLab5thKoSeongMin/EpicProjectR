// Responsibility: Composes the first playable runtime UI, session, presenter, and input event system.
using System;
using EpicProjectR.Application;
using EpicProjectR.Content;
using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

namespace EpicProjectR.Presentation
{
    public sealed class FirstPlayableBootstrap : MonoBehaviour
    {
        private FirstPlayablePresenter presenter;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoCreate()
        {
            if (FindObjectOfType<FirstPlayableBootstrap>() != null)
            {
                return;
            }

            var root = new GameObject("First Playable Bootstrap");
            DontDestroyOnLoad(root);
            root.AddComponent<FirstPlayableBootstrap>();
        }

        private void Awake()
        {
            EnsureEventSystem();

            var assets = Require(FirstPlayableAssetCatalog.Load(), nameof(FirstPlayableAssetCatalog));
            var theme = Require(FirstPlayableUiTheme.Create(assets), nameof(FirstPlayableUiTheme));
            var factory = Require(new FirstPlayableUiFactory(theme, assets), nameof(FirstPlayableUiFactory));
            var view = Require(new FirstPlayableView(theme, factory), nameof(FirstPlayableView));
            view.Build(transform);

            var session = Require(FirstPlayableSessionFactory.Create(), nameof(FirstPlayableSession));
            var allContracts = Require(FirstPlayableFixtures.Contracts(), "FirstPlayableContracts");
            var allRules = Require(FirstPlayableFixtures.Rules(), "FirstPlayableRules");
            presenter = new FirstPlayablePresenter(session, allContracts, allRules, view);
            presenter.ShowCurrent();
        }

        private static T Require<T>(T instance, string name) where T : class
        {
            if (instance == null)
            {
                throw new InvalidOperationException($"{name} was not created for the first playable composition root.");
            }

            return instance;
        }

        private static void EnsureEventSystem()
        {
            var eventSystem = FindObjectOfType<EventSystem>();

            if (eventSystem == null)
            {
                var eventSystemObject = new GameObject("EventSystem");
                eventSystem = eventSystemObject.AddComponent<EventSystem>();
            }

#if ENABLE_INPUT_SYSTEM
            var legacyModules = eventSystem.GetComponents<StandaloneInputModule>();
            foreach (var legacyModule in legacyModules)
            {
                Destroy(legacyModule);
            }

            if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
            {
                eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            }
#else
            if (eventSystem.GetComponent<StandaloneInputModule>() == null)
            {
                eventSystem.gameObject.AddComponent<StandaloneInputModule>();
            }
#endif
        }
    }
}
