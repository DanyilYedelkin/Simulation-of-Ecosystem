using UnityEngine;

public class StateSettingsController : MonoBehaviour
{
    public static bool MainMenuStart = false;
    public static MainMenu.AnimalConfiguration PreyConfiguration { get; set; }
    public static MainMenu.AnimalConfiguration PredatorConfiguration { get; set; }
    public static MainMenu.GeneralConfiguration GeneralConfiguration { get; set; }
}
