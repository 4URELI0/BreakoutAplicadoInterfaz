using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] float speed = 9f;
    GameManager gameManager;//Obtener referencia al GameManager
    [SerializeField] float xLimit = 7.29f;//Limites que el paddle puede manejar
    [SerializeField] float xLimitWhenBig = 6f;
    [SerializeField] float speedPU = 50.5f;
    [SerializeField] byte timeBigSize = 10;//Para modificar el tiempo en la ventana inspector la velocidad del powerUp
    [SerializeField] byte timeBigSpeed = 10;//Para modificar el tiempo en la ventana inspector la velocidad del power ups
    [SerializeField] Transform trasnChildren;

    [SerializeField] GameObject[] arrayLives;


    // Referencia al SpriteRenderer
    private SpriteRenderer childSpriteRenderer;

    // El sprite del hijo
    public Sprite originalSprite;
    // El sprite del hijo
    public Sprite newSprite;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        // Obtener el SpriteRenderer del primer hijo (índice 0)
        childSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        originalSprite = childSpriteRenderer.sprite;
    }

    void Update()
    {
        MovimientoJugador();       
    }
    void MovimientoJugador()
    {
        //Va a verificar si el usuario presiono una tecla con el if
        if (Input.GetKey(KeyCode.D) && transform.position.x < xLimit)//Verifica si se presiono la tecla D para mover el paddle hacia la derecha
        {
            transform.position += speed * Time.deltaTime * Vector3.right;
        }
        else if (Input.GetKey(KeyCode.A) && transform.position.x > -xLimit)//Verifica si se presiono la tecla A para mover el paddle hacia la izquierda y los limite del rango del paddle
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
        }
        if (Input.GetMouseButtonDown(0))//Usamos GetMouseBottonDown para leer el input que necesitamos, el 0 indica que usamos el botón izquierdo del mouse ya que es un arreglo
        {
            /*La razon por cual revisamos si el valor de la propiedad es falso es por que cada una tiene una condición diferente */
            if (gameManager.BallOnPlay == false)//Pasara a false cada vez que la bola se detenga y regresara verdadero cuando sea lanzada de nuevo
            {
                gameManager.BallOnPlay = true; //A)Lee el input del usuario y le dice a GameManager en que momento lanzar la bola 
            }
            if (gameManager.GameStarted == false)//Se ejecutara una vez al inicio del juego
            {
                gameManager.GameStarted = true;  
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            //Obtenemos el componente PowerUp pero con el objeto de colisión por la información que este contiene con el paddle
            //Obtenemos su variable powerUpType y verificamos si corresponde al 
            //Accedemos a la clase (PowerUp) y luego el nombre del enum (PowerUpType)
            //Indicamos que el paddle tiene un power up
            if (collision.GetComponent<PowerUp>().powerUpType == PowerUp.PowerUpType.IncreaseSize)
            {

                gameManager.bigSize = true;
                Debug.Log("Obtuvo un power up de:Aumento de tamaño" + gameManager.bigSize);//Verificacion
                StartCoroutine(BigSizePower());//Llamamos a la corrutina justo en el momento que se detecte
            
            }else if (collision.GetComponent<PowerUp>().powerUpType == PowerUp.PowerUpType.IncreaseSpeed)
            {
                gameManager.bigSpeed = true;
                Debug.Log("Obtuvo un power up de:Aumento de velocidad " + gameManager.bigSpeed);
                StartCoroutine(BigSpeedPower());
            }
            Destroy(collision.gameObject);   
        }
    }
   IEnumerator BigSizePower()
   {
        float originalXLimit = xLimit;//Guardamos el limite original en una variable local
        xLimit = xLimitWhenBig;//Indicamos que el nuevo limit sera el valor de xLimitWhenBig

        // Cambia el sprite del hijo
        childSpriteRenderer.sprite = newSprite;
        arrayLives[0].SetActive(false);
        arrayLives[1].SetActive(true);

        //Incrementar de tamaño
        Vector3 newSize = trasnChildren.localScale;//Un vector3 que almacena el tamaño actual del paddle
        while (trasnChildren.localScale.x< 4f)//Un while que toma el eje X del newSize y lo incrementa mientras el valor sea menor a 1.5
        {
            newSize.x += Time.deltaTime;//Utilizamos un deltaTime para que su animación sea fluida en el incremento
            trasnChildren.localScale = newSize;//Luego asignamos el valor actualizado a la scala del paddle
        }
        yield return new WaitForSeconds(timeBigSize);
        //Reducir a su tamaño original
        while (trasnChildren.localScale.x > 2)
        {
            newSize.x -= Time.deltaTime;
            trasnChildren.localScale = newSize;
        }


        arrayLives[1].SetActive(false);
        arrayLives[0].SetActive(true);

        childSpriteRenderer.sprite = originalSprite;

        gameManager.bigSize = false;//Indicamos que el paddle ya no tiene el power up
        xLimit = originalXLimit;//Una vez terminado los efectos del power Up este volvera a su tamanio original
   }
   
    IEnumerator BigSpeedPower()
    {
        //Aumentar la velocidad del paddle
        float originalSpeed = speed;
        speed = speedPU;
        yield return new WaitForSeconds(timeBigSpeed);
        //Disminuir la velocidad del paddle
        speed = originalSpeed;
        
        gameManager.bigSpeed = false;
    }
    
}