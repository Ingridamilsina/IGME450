using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum ForceDirection
{
    Left = -1,
    Right = 1
}

public class Ball : MonoBehaviour
{
    [SerializeField] private float horizontalForceVelocty = 5.0f;
    [SerializeField] private float springForce = 10.0f;

    [SerializeField] private int nextLevelIndex;

    private bool horizontalForce = false;
    private ForceDirection forceDirection = ForceDirection.Left;

    private Vector2 startPosition;

    private Rigidbody2D rb;

    private int stars = 0;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = GameObject.Find("Start").transform.position;
        rb = GetComponent<Rigidbody2D>();

        // Finally, set the object position to the start position
        //rb.MovePosition(startPosition);
        rb.position = startPosition;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontalForce)
        {
            // Movement augmented by horizontal force
            float addedForce = horizontalForceVelocty * (int)forceDirection;
            rb.velocity = new Vector2(rb.velocity.x + addedForce * Time.deltaTime, rb.velocity.y);
        }
        else
        {
            // Movement
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        if(gameObject.transform.position.y < -10)
        {
            GameManager.instance.UpdateGameState(GameManager.GameState.Building);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "End")
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Level" + nextLevelIndex);
        }

        if (collision.gameObject.tag == "InvertedGravity")
        {
            // Inverts the gravity when entering an inverted gravity zone
            rb.gravityScale *= -1;
        }

        if (collision.gameObject.tag == "LeftForce")
        {
            // Sets horizontalForce to true and sets forceDirection to Left
            horizontalForce = true;
            forceDirection = ForceDirection.Left;
        }

        if (collision.gameObject.tag == "RightForce")
        {
            // Sets horizontalForce to true and sets forceDirection to Right
            horizontalForce = true;
            forceDirection = ForceDirection.Right;
        }

        if (collision.gameObject.tag == "Spring")
        {
            // Add a force to the object in the direction the spring is pointing
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(collision.gameObject.transform.up * springForce, ForceMode2D.Impulse);
        }

        if (collision.gameObject.tag == "Star")
        {
            stars++;
            collision.gameObject.SetActive(false);
            print("Star collected");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InvertedGravity")
        {
            // Reverts the inverted gravity when exiting an inverted gravity zone
            rb.gravityScale *= -1;
        }

        if (collision.gameObject.tag == "LeftForce")
        {
            // Sets horizontalForce to false
            horizontalForce = false;
        }

        if (collision.gameObject.tag == "RightForce")
        {
            // Sets horizontalForce to false
            horizontalForce = false;
        }
    }

    //RESTART THE LEVEL IF THE OBJECT LEAVES THE SCREEN
    //private void OnBecameInvisible()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    public void ResetPosition()
    {
        //rb.MovePosition(startPosition);
        rb.position = startPosition;
    }
}
