using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        
        Destroy(gameObject);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Debris")
            other.gameObject.GetComponent<Debris>().TakeDamage(1f);
        
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        
    }
}
