namespace MainMenu
{
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using UnityEngine;

    public class MainMenuController : MonoBehaviour
    {
        #region Configuration
        [Header("MainMenu screens")]
        [SerializeField] private GameObject _firstScreen;
        [SerializeField] private GameObject _secondScreen;

        [Header("Buttons")]
        [SerializeField] private Button _startSimulationButton;

        [Header("Settings Conffiguration")]
        [SerializeField] private SettingsController _settingsController;
        #endregion

        #region Unity Methods
        private void Start()
        {
            _firstScreen.SetActive(true);
            _secondScreen.SetActive(false);
        }
        #endregion

        #region API
        public void ExitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void StartConfigurationButton()
        {
            _firstScreen.SetActive(false);
            _secondScreen.SetActive(true);
        }

        public void BackButton()
        {
            _firstScreen.SetActive(true);
            _secondScreen.SetActive(false);
        }

        public void StartSimulation()
        {
            bool canStart = CanStartSimulation();

            _startSimulationButton.interactable = canStart;

            if (canStart)
            {
                // save configuration
                StateSettingsController.MainMenuStart = true;
                StateSettingsController.PreyConfiguration = _settingsController.PreyConfiguration;
                StateSettingsController.PredatorConfiguration = _settingsController.PredatorConfiguration;
                StateSettingsController.GeneralConfiguration = _settingsController.GeneralConfiguration;

                SceneManager.LoadScene("EcosystemSimulation");
            }
        }
        #endregion

        #region Local Methods
        private bool CanStartSimulation()
        {
            return true;
        }
        #endregion

        #region Properties
        #endregion
    }
}