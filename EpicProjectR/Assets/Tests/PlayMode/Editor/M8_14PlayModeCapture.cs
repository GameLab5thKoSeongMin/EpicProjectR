// Responsibility: Drives the M8.14 first-playable loop in the editor and captures repeatable visual QA evidence.
#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EpicProjectR.Tests
{
    public static class M8_14PlayModeCapture
    {
        private const string RunningKey = "EpicProjectR.M8_14Capture.Running";
        private const string StepKey = "EpicProjectR.M8_14Capture.Step";
        private const string NextTimeKey = "EpicProjectR.M8_14Capture.NextTime";
        private const string ErrorKey = "EpicProjectR.M8_14Capture.Error";
        private const int ExitStep = 100;

        [InitializeOnLoadMethod]
        private static void ResumeAfterReload()
        {
            Debug.Log($"M8.14 capture resume. Running={SessionState.GetBool(RunningKey, false)}, Playing={EditorApplication.isPlaying}.");
            if (SessionState.GetBool(RunningKey, false))
            {
                HookUpdate();
            }
        }

        public static void Run()
        {
            Debug.Log("M8.14 capture requested.");
            Directory.CreateDirectory(CaptureDirectory());
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);
            SessionState.SetBool(RunningKey, true);
            SessionState.SetInt(StepKey, 0);
            SessionState.SetString(ErrorKey, string.Empty);
            SessionState.SetFloat(NextTimeKey, 0f);
            HookUpdate();
            EditorApplication.EnterPlaymode();
        }

        private static void HookUpdate()
        {
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        private static void Tick()
        {
            if (!SessionState.GetBool(RunningKey, false))
            {
                EditorApplication.update -= Tick;
                return;
            }

            var step = SessionState.GetInt(StepKey, 0);
            if (!EditorApplication.isPlaying)
            {
                if (step == ExitStep)
                {
                    FinishAndExit();
                }

                return;
            }

            if (EditorApplication.timeSinceStartup < SessionState.GetFloat(NextTimeKey, 0f))
            {
                return;
            }

            try
            {
                ExecuteStep(step);
            }
            catch (Exception exception)
            {
                SessionState.SetString(ErrorKey, exception.ToString());
                SessionState.SetInt(StepKey, ExitStep);
                EditorApplication.ExitPlaymode();
            }
        }

        private static void ExecuteStep(int step)
        {
            Debug.Log($"M8.14 capture executing step {step}.");
            switch (step)
            {
                case 0:
                    UnityEngine.Application.runInBackground = true;
                    Screen.SetResolution(1920, 1080, false);
                    Schedule(1, 1.2f);
                    break;
                case 1:
                    Capture("01_initial_1920x1080.png");
                    Schedule(2, 0.45f);
                    break;
                case 2:
                    ClickObjectButton("Entry Bell");
                    Schedule(3, 0.8f);
                    break;
                case 3:
                    Capture("02_contractor_arrived_1920x1080.png");
                    Schedule(4, 0.45f);
                    break;
                case 4:
                    ClickObjectButton("Clickable Entry Contract");
                    Schedule(5, 0.85f);
                    break;
                case 5:
                    Capture("03_c001_review_1920x1080.png");
                    Schedule(6, 0.45f);
                    break;
                case 6:
                    ClickObjectButton("Final Decision Paper Button");
                    Schedule(7, 0.55f);
                    break;
                case 7:
                    Capture("04_c001_decision_open_1920x1080.png");
                    Schedule(8, 0.45f);
                    break;
                case 8:
                    ClickLabelButton("승인");
                    Schedule(9, 1.05f);
                    break;
                case 9:
                    Capture("05_next_contract_waiting_1920x1080.png");
                    ClickObjectButton("Entry Bell");
                    Schedule(10, 0.75f);
                    break;
                case 10:
                    ClickObjectButton("Clickable Entry Contract");
                    Schedule(11, 0.85f);
                    break;
                case 11:
                    SelectAllApplicableReasons();
                    Capture("06_c002_ar_review_1920x1080.png");
                    Schedule(12, 0.45f);
                    break;
                case 12:
                    ClickObjectButton("Final Decision Paper Button");
                    Schedule(13, 0.4f);
                    break;
                case 13:
                    ClickLabelButton("거절");
                    Schedule(14, 1.05f);
                    break;
                case 14:
                    ClickObjectButton("Entry Bell");
                    Schedule(15, 0.75f);
                    break;
                case 15:
                    ClickObjectButton("Clickable Entry Contract");
                    Schedule(16, 0.85f);
                    break;
                case 16:
                    SelectAllApplicableReasons();
                    ClickObjectButton("Final Decision Paper Button");
                    Schedule(17, 0.55f);
                    break;
                case 17:
                    Capture("07_c003_cr_decision_1920x1080.png");
                    Schedule(18, 0.45f);
                    break;
                case 18:
                    ClickLabelButton("조건부 승인");
                    Schedule(19, 1.2f);
                    break;
                case 19:
                    Capture("08_complete_1920x1080.png");
                    Screen.SetResolution(1280, 720, false);
                    Schedule(20, 0.8f);
                    break;
                case 20:
                    Capture("09_complete_1280x720.png");
                    Schedule(21, 0.5f);
                    break;
                case 21:
                    SessionState.SetInt(StepKey, ExitStep);
                    EditorApplication.ExitPlaymode();
                    break;
                default:
                    throw new InvalidOperationException($"Unknown M8.14 capture step {step}.");
            }
        }

        private static void ClickObjectButton(string objectName)
        {
            var button = ActiveButtons().FirstOrDefault(candidate => candidate.gameObject.name == objectName);
            Invoke(button, objectName);
        }

        private static void ClickLabelButton(string label)
        {
            var button = ActiveButtons().FirstOrDefault(candidate =>
            {
                var text = candidate.GetComponentInChildren<Text>(true);
                return text != null && text.text == label;
            });
            Invoke(button, label);
        }

        private static Button[] ActiveButtons()
        {
            return Resources.FindObjectsOfTypeAll<Button>()
                .Where(button => button.gameObject.scene.IsValid() && button.gameObject.activeInHierarchy)
                .ToArray();
        }

        private static void Invoke(Button button, string identity)
        {
            if (button == null || !button.interactable)
            {
                throw new InvalidOperationException($"Active interactable button '{identity}' was not found.");
            }

            button.onClick.Invoke();
        }

        private static void SelectAllApplicableReasons()
        {
            var toggles = Resources.FindObjectsOfTypeAll<Toggle>()
                .Where(toggle => toggle.gameObject.scene.IsValid()
                    && toggle.gameObject.activeInHierarchy
                    && toggle.gameObject.name == "Applicable Reason")
                .ToArray();

            if (toggles.Length == 0)
            {
                throw new InvalidOperationException("No applicable reason toggle was found for the current contract.");
            }

            foreach (var toggle in toggles)
            {
                toggle.isOn = true;
            }
        }

        private static void Capture(string fileName)
        {
            ScreenCapture.CaptureScreenshot(Path.Combine(CaptureDirectory(), fileName));
        }

        private static void Schedule(int nextStep, float delaySeconds)
        {
            SessionState.SetInt(StepKey, nextStep);
            SessionState.SetFloat(NextTimeKey, (float)EditorApplication.timeSinceStartup + delaySeconds);
        }

        private static string CaptureDirectory()
        {
            return Path.GetFullPath(Path.Combine(UnityEngine.Application.dataPath, "..", "..", "docs", "project", "screenshots", "m8_14"));
        }

        private static void FinishAndExit()
        {
            var error = SessionState.GetString(ErrorKey, string.Empty);
            SessionState.SetBool(RunningKey, false);
            EditorApplication.update -= Tick;

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError(error);
                EditorApplication.Exit(1);
                return;
            }

            Debug.Log("M8.14 Play Mode capture completed successfully.");
            EditorApplication.Exit(0);
        }
    }
}
#endif
