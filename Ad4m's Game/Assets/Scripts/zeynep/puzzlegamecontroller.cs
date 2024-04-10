using UnityEngine;
public class puzzlegamecontroller: MonoBehaviour
{

    [SerializeField] public Transform empty = null;
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                if (Vector2.Distance(a: empty.position, b: hit.transform.position) < 1)
                {
                    Vector2 lastemptyposition = empty.position;
                    empty.position = hit.transform.position;
                    hit.transform.position = lastemptyposition;
                }
            }
        }

    }
