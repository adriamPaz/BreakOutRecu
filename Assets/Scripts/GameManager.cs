using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static int[] totalBricks = new int[] {0, 32, 21};

    // Variable para llevar el control de la puntuación 
    public static int Score { get; private set; } = 0;
    public static int Lives { get; private set; } = 3;
    // Referencia al texto para mostrar la puntuación en la interfaz 
    
    // Método para actualizar las vidas 
    public static void Updatelives() { Lives--; }
    // Método para actualizar la puntuación 
    public static void UpdateScore(int points) { Score += points; }
}
