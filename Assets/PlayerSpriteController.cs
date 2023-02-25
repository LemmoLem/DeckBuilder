using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpriteController : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float X = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(X * 7f, rb.velocity.y);
       }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        SceneManager.LoadScene("Card Battle");
    }
}
