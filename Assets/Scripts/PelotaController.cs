using UnityEngine;
using System.Collections.Generic;
using static GameManager;
using UnityEngine.SceneManagement;
public class PelotaController : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] float force;
    Rigidbody2D rb;
    int brickCount;
    AudioSource sfx;
    //[SerializeField] GameManager gameManager;
    [SerializeField] AudioClip sfxPaddel;  // Sonido al chocar con la pala
    [SerializeField] AudioClip sfxBrick;   // Sonido al chocar con un ladrillo
    [SerializeField] AudioClip sfxWall;    // Sonido al chocar con una pared
    [SerializeField] AudioClip sfxFail;    // Sonido al salir por la pared inferior
    [SerializeField] GameObject pala;


    int sceneId;

    // Mantenemos un registro de los golpes con la pala.
    int contadorGolpes = 0;

    // Definimos la fuerza a aplicar para aumentar la velocidad.
    [SerializeField] float fuerzaIncrementada;

    bool halved = false;


    Dictionary<string, int> ladrillos = new Dictionary<string, int>(){
        {"LadrilloCian", 10},
        {"LadrilloVerde", 15},
        {"LadrilloNaranja", 20},
        {"LadrilloMorado", 25},
        {"LadrilloAtravesable", 25},
    };
    void Start()
    {
        sceneId = SceneManager.GetActiveScene().buildIndex;
        sfx = GetComponent<AudioSource>();

        if (sfx == null)
        {
            sfx = gameObject.AddComponent<AudioSource>();
        }

        rb = GetComponent<Rigidbody2D>();
        Invoke("LanzarPelota", delay);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DestroyBrick(GameObject obj){
    sfx.clip = sfxBrick; 
    sfx.Play();
    // Actualizamos la puntuación 
    GameManager.UpdateScore(ladrillos[obj.tag]);
    // Se destruye el objeto
    Destroy(obj);
    // Actualizamos el contador de ladrillos destruidos
    ++brickCount;
    // Comprobamos si hemos alcanzado el máximo de ladrillos. Necesitamos el índice de la escena en la que nos encontramos para saber cuántos ladrillos tenemos. 
    if(brickCount == GameManager.totalBricks[sceneId]){
        
        sfx.Play();
        // Detenemos el movimiento de la pelota
        rb.linearVelocity = Vector2.zero;
        Invoke("NextScene", 3);
    }
}

void NextScene(){
    int nextId = sceneId + 1; 
    if(nextId == SceneManager.sceneCountInBuildSettings){
        nextId = 0;
    }
    SceneManager.LoadScene(nextId);
}

    private void LanzarPelota()
    {
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        float dirX, dirY = -1;
        dirX = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector2 dir = new Vector2(dirX, dirY);
        dir.Normalize();

        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Almacenamos la etiqueta del objeto con el que estamos colisionando
        string tag = other.gameObject.tag;

        if (tag == "Pala")
        {
            sfx.clip = sfxPaddel;
            sfx.Play();
            contadorGolpes++;

            // Si el contador de golpes es un múltiplo de 4, incrementamos la velocidad.
            if (contadorGolpes % 4 == 0)
            {
                // Aplicamos una fuerza adicional en la dirección actual de movimiento de la pelota.
                rb.AddForce(rb.velocity * fuerzaIncrementada, ForceMode2D.Impulse);
            }
        }
        else if (ladrillos.ContainsKey(tag) && tag != "LadrilloAtravesable")
        {

            DestroyBrick(other.gameObject);
            sfx.clip = sfxBrick;
            sfx.Play();
            GameManager.UpdateScore(ladrillos[tag]);




        }
        else if (tag == "LadrilloAtravesable")
        {
            //Sumamos puntos
            GameManager.UpdateScore(ladrillos[tag]);
            //Sonido del ladrillo
            sfx.clip = sfxBrick;
            sfx.Play();
            //Se desactiva el collider para que la pelota no detecte el "Trigger" y no sumar puntos
            enabled = false;
        }
        else if (tag == "ParedDerecha" || tag == "ParedIzquierda" || tag == "ParedSuperior" || tag == "LadrilloIndestructible")
        {
            sfx.clip = sfxWall;
            sfx.Play();
        }

        if (!halved && tag == "ParedSuperior")
        {
            HalvePaddle(true);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos si el objeto que estamos atravesando es la pared inferior 
        if (other.tag == "ParedInferior")
        {
            GameManager.Updatelives();
            sfx.clip = sfxFail;
            sfx.Play();

            if (halved)
            {
                HalvePaddle(false);

            }
            // Volvemos a lanzar la pelota
        }
        if (GameManager.Lives <= 0)
    {
        //Se detiene el movimiento de la pelota
        rb.linearVelocity = Vector2.zero;
        //Se desactiva la pelota
        gameObject.SetActive(false);
        //Se sale del método para que no se relance
        return;
    }

        // Si aún quedan vidas se vuelve a lanzar la pelota
        Invoke("LanzarPelota", delay);
    }
    

    public void HalvePaddle(bool reducir)
    {
        halved = reducir;
        Vector3 escalaActual = pala.transform.localScale;
        pala.transform.localScale = reducir ?
            new Vector3(escalaActual.x * 0.5f, escalaActual.y, escalaActual.z) :
            new Vector3(escalaActual.x * 2f, escalaActual.y, escalaActual.z);
    }
}
