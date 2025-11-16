using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidBody2D;
    Vector2 moveDirection;
    Vector2 currentVelocity;
    [SerializeField] float speed = 3f;
    GameManager gameManager;//Acceso al gameManager desde el script
    
    /*Sonidos*/
    [SerializeField] AudioClip paddleBounce;
    [SerializeField] AudioClip bounce;
    [SerializeField] AudioClip loseLife;
    
    void Start()
    {
        //Nombre de la variable declarado en la linea 7,
        //(=)<- se encargar de guardar el resultado de la variable,
        //<El tipo de componente que estamos buscando>,
        //Finalizamos con un () dado que estamos ejecutando un metodo
        rigidBody2D = GetComponent<Rigidbody2D>();
        //Movimiento de la pelota hacia arriba
        gameManager = FindObjectOfType<GameManager>();//Obtenemos el componente  y es �nico
    }
    void FixedUpdate()//Los tiempos delta son constantes
    {
        currentVelocity = rigidBody2D.linearVelocity;   
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("La bola colisiono con: " + collision.transform.name);
        moveDirection = Vector2.Reflect(currentVelocity, collision.GetContact(0).normal);
        rigidBody2D.linearVelocity = moveDirection;

        if (collision.transform.CompareTag("LimiteMuerte"))
        {
            Debug.Log("Colision con el limite MUERTE");
            FindObjectOfType<AudioController>().PlaySfx(loseLife);

            // Asegurarnos de tener referencia al GameManager
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }

            if (gameManager != null)
            {
                // Decrementa vidas en GameManager -> este se encargará de llamar ResetBall() si corresponde
                gameManager.PlayerLives--;
            }
            else
            {
                Debug.LogWarning("Ball: no se encontró GameManager al colisionar con LimiteMuerte");
            }
        }

        if (collision.transform.CompareTag("Player"))
        {
            FindObjectOfType<AudioController>().PlaySfx(paddleBounce);
            Debug.Log("verificacion de clip");
        }
        if (collision.transform.CompareTag("Brick"))
        {
            FindObjectOfType<AudioController>().PlaySfx(bounce);
            Debug.Log("Clip de brick");
        }
    }
    public void LaunchBall()
    {
     transform.SetParent(null);//Utilizamos este codigo para decir que la bola ya no es hijo del paddle en el momento de ser lanzada; Surg�a el problema de que la bola segu�a al paddle cuando es lanzada
     rigidBody2D.linearVelocity = Vector2.up * speed;//No utilizamos el deltaTime por que, por que el motor de fisica tiene tiempos delta definido y constantes que no dependen de la velocidad de la computadora 
    }
    public void ResetBall()
    {
        rigidBody2D.linearVelocity = Vector2.zero;
        rigidBody2D.angularVelocity = 0f;

        GameObject paddleObj = GameObject.FindWithTag("Player");

        if (paddleObj == null)
        {
            Debug.LogError("No se encontró Paddle con tag Player");
            return;
        }

        Transform paddle = paddleObj.transform;

        transform.SetParent(paddle);

        Vector2 pos = paddle.position;
        pos.y += 0.3f;
        transform.position = pos;

        gameManager.BallOnPlay = false;
    }
}