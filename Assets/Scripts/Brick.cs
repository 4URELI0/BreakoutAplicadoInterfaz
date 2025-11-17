using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Brick : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject[] powerUpPrefabs;
    [SerializeField] int basePoint = 10;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null) 
        {
           gameManager.BricksOnLevel++;
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (gameManager != null)
        {
            gameManager.BricksOnLevel--;
            gameManager.AddScore(1);
        }

        if (!gameManager.bigSize && !gameManager.bigSpeed)
        {
            int numeroRandom = Random.Range(0, 100);
            if (numeroRandom < 40)
            {
                int powerUpRandom = Random.Range(0, powerUpPrefabs.Length);
                Instantiate(powerUpPrefabs[powerUpRandom], transform.position, Quaternion.identity);
            }
        }
        if (collision.gameObject.CompareTag("Ball"))
        {
            int finalPoints = basePoint * gameManager.comboMultiplier;
            gameManager.AddScore(finalPoints);
            gameManager.BlockDestroyed();
            Destroy(gameObject);
        }
        
    }
}