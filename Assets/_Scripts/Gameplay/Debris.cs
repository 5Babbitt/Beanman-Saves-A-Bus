using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour, IDamageable
{
    [SerializeField] private int damage;

    [SerializeField] private AudioClip[] debrisDestroyedClips;

    public Rigidbody2D rb { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bus")
        {
            other.gameObject.GetComponent<Bus>().TakeDamage(damage);
            Debug.Log($"{this.name} hit the bus");
        }
        
        TakeDamage(0);
    }

    public void TakeDamage(float value)
    {
        EventManager.Instance.OnDebrisDestroyed?.Invoke(transform.position);
        Destroy(gameObject);
    }

    public void SetDamageValue(float value)
    {
        damage = Mathf.RoundToInt(value);
    }

    void OnDestroy()
    {
        AudioManager.PlayRandomSoundEffect(debrisDestroyedClips);
    }
}
