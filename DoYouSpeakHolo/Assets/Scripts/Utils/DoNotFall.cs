
using UnityEngine;
using static Positions;

public class DoNotFall : MonoBehaviour {
    private GravityEnabler gravity;
    private float floor;

    // Start is called before the first frame update
    void Start() {
        gravity = gameObject.AddComponent<GravityEnabler>();
        Positions positions = ObjectPooler.GetPooler().Positions;
        floor = positions.GetPosition(positions.floorPosition).y;  
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.transform.position.y < Floor) {
            Vector3 temp = new Vector3(0, floor -gameObject.transform.position.y, 0);
            gameObject.transform.position += temp;
        }

        if (gameObject.transform.position.y == floor) {
            gravity.DisableGravity();
        }
    }
}
