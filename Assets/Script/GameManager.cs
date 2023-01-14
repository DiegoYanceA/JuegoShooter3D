using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string labelText = "Recolecta todos los items causa";
    public const int MAX_ITEMS = 5;
    public bool showWinScreen = false;
    public bool showLossScreen = false;

    private int _itemsCollected = 0;
    public int Items
    {
        get
        {
            return _itemsCollected;
        }
        set
        {
            _itemsCollected = value;
            if (MAX_ITEMS <= _itemsCollected)
            {
                labelText = "Has encontrado todos los items";
                showWinScreen = true;
                Time.timeScale = 0;
            } else {
                labelText = "Item encontrado, te faltan: " + (MAX_ITEMS - _itemsCollected);
            }
        }
    }

    private int _playerHP = 3;
    public int HP
    {
        get
        {
            return _playerHP;
        }
        set
        {
            if (0 <= value && value <= 3)
            {
                _playerHP = value;
            }

            if(_playerHP <= 0)
            {
                showLossScreen = true;
                Time.timeScale = 0;
            } else
            {
                labelText = "Me han dado esos rojos";
            }
            
            Debug.LogFormat("Vida: {0}", _playerHP);
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(25, 25, 180, 25), "Vida: " + _playerHP);
        GUI.Box(new Rect(25, 65, 180, 25), "Item recolectado: " + _itemsCollected);

        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 200, 50),
                    labelText);



        if (showWinScreen)
        {
            ShowEndLevel("Has ganado");
        }

        if (showLossScreen)
        {
            ShowEndLevel("Has pedido");
        }
    }

    private void ShowEndLevel(string message)
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 100,
                                   Screen.height / 2 - 20,
                                   200, 40), message))
        {
            Utilities.RestartLevel();
        }
    }
}
