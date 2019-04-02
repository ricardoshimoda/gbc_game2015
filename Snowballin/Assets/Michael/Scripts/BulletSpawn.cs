using UnityEngine;
using System.Collections;

public class BulletSpawn : MonoBehaviour {
    
   // [SerializeField] float speed = 6.0f;
    [SerializeField] float lifeSpan;
    GameObject target;
    Rigidbody snowball;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        snowball = GetComponent<Rigidbody>();
       // speed = UnityEngine.Random.Range(5, 8);
    }
    // Use this for initialization
    void Start()
    {
        
        snowball.velocity = caculateVelocity(target.transform, Random.Range(20, 30));
        Destroy(gameObject, lifeSpan);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Obstacle")
        {
            this.gameObject.SetActive(false);

        }
    }

    Vector3 caculateVelocity(Transform target, float angle)
    {
        Vector3 direction = target.position - transform.position;  // get target direction
        float h = direction.y;  // get height difference
        direction.y = 0;  // retain only the horizontal direction
        float distance = direction.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        direction.y = distance * Mathf.Tan(a);  // set dir to the elevation angle
        distance += h / Mathf.Tan(a);  // correct for small height differences
        float vel = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));    // calculate the velocity magnitude
       
        return vel * direction.normalized;
    }

}

