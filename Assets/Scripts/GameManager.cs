using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int[] totalBricks = new int[] {0, 28, 21};

    // Variable para llevar el control de la puntuación 
    public static int Score { get; private set; } = 0;
    public static int Lives { get; private set; } = 3;
    // Referencia al texto para mostrar la puntuación en la interfaz 
    
    // Método para actualizar las vidas 
    public static void Updatelives() { Lives--; }
    // Método para actualizar la puntuación 
    public static void UpdateScore(int points) { Score += points; }

    // Método para reiniciar el juego
    public static void ResetGame(){
        Score = 0;

        Lives = 3;

        SceneManager.LoadScene(0);
    }

    void Start() {
    // Mejora: Desactivar el cursor
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // Mejora: Salir del juego al pulsar Escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Saliendo del juego...");
            Application.Quit(); 
        }
    }
}
