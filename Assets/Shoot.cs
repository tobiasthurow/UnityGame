using UnityEngine;
using UnityEngine.Networking;

public class Shoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    public Weapon weapon;

    [SerializeField]
    private Rigidbody projectile;

    [SerializeField]
    private int bulletSpeed = 100;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    // Use this for initialization
    void Start() {
        if (cam == null) {
            Debug.LogError("Shoot: Camera not referenced");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            shoot();
        }
    }

    [Client]
    void shoot() {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)) {
            if(hit.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(hit.collider.transform.name, hit.collider.transform.parent.transform.name, weapon.damage);
            }
        }
        Rigidbody instanciatedObject = (Rigidbody) Instantiate(projectile, (cam.transform.position + Vector3.forward), cam.transform.rotation);
        instanciatedObject.velocity = cam.transform.TransformDirection(Vector3.forward * bulletSpeed);
    }

    [Command]
    void CmdPlayerShot(string bodyPart, string playerId, int damage) {
        if (bodyPart.Contains("Head")) {
            Player player = GameManager.GetPlayer(playerId);
            player.RpcTakeDamage(2*damage);
            Debug.Log("headshot on " + playerId);
        } else {
            Player player = GameManager.GetPlayer(playerId);
            player.RpcTakeDamage(damage);
            Debug.Log("bodyshot on " + playerId);
        }
        
    }
}