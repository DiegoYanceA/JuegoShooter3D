using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public static class Utilities
{
    public static int playerDeaths = 0;

    // Metodos

    /// <summary>
    /// Reinicia el level en el que se encuentra
    /// </summary>
    public static void RestartLevel()
    {

        // Obtiene la escena actual
        int index = SceneManager.GetActiveScene().buildIndex;

        // Recarga la escena
        SceneManager.LoadScene(index);

        // Vuele el tiempo a su normalidad
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// Reinicia el level que se desea
    /// </summary>
    /// <param name="scene">scene es la escena que se desea reiniciar</param>
    public static void RestartLevel(int scene)
    {

        // Recarga la escena
        SceneManager.LoadScene(scene);

        // Vuele el tiempo a su normalidad
        Time.timeScale = 1.0f;
    }
}
