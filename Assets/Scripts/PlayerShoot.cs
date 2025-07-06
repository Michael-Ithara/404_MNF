using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    public Vector2 mousePosition;
    public Vector2 shootDirection;

    // Update is called once per frame
    void Update()
    {
        // mousePosition = Mouse.current.position.ReadValue();
       /* if (Input.GetMouseButtonDown(1)) // Left click
        {
            Shoot();
        } */
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            // Get mouse position
            mousePosition = Mouse.current.position.ReadValue();

            Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            // Direction from us to mouse
            shootDirection = (mousePosition - playerScreenPos).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(shootDirection.x, shootDirection.y) * bulletSpeed;
            Destroy(bullet, 2f); // Destroy bullet after 2 seconds to prevent memory leaks
        }
    }
}