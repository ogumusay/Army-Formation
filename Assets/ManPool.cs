using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManPool : MonoBehaviour
{
    [SerializeField]
    Transform dirTransform;
    
    public Man[] mans;
    public GameObject[] markers;
    
    Camera cam;
    
    public int column = 1;
    Vector3 hitPoint;
    Vector3 mousePosition;
    Vector3 backwardVector;
    Vector3 rightVector;
    int distance = 0;

    List<Vector3> positions = new List<Vector3>();

    public float rowGap = 1.3f;
    public float columnGap = 1.3f;


    void Start()
    {
        cam = Camera.main;
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;

            Ray ray = cam.ScreenPointToRay(mousePosition);

            RaycastHit hit;

            positions.Clear();

            if (Physics.Raycast(ray, out hit))
            {
                hitPoint = hit.point;
                LookDirection();
                SquareForm(hitPoint);
            }
        }

        if (Input.GetMouseButton(0))
        {
            int distanceLive = (int) (Input.mousePosition.x - mousePosition.x) / 100;

            if (distanceLive > distance)
            {
                positions.Clear();
                distance = distanceLive;
                column++;
            }
            else if(distance > distanceLive)
            {
                positions.Clear();
                distance = distanceLive;
                column--;
                if (column <= 0)
                {
                    column = 1;
                }
            }
            
            SquareForm(hitPoint);
        }

        if (Input.GetMouseButtonUp(0))
        {
            distance = 0;

            for (int i = 0; i < mans.Length; i++)
            {
                mans[i].agent.SetDestination(positions[i]);
            }
        }
    }

    private void SquareForm(Vector3 hit)
    {

        int manCount = mans.Length;
        int row = manCount % column == 0 ? manCount / column : manCount / column + 1;

        for (int i = 0; i < row; i++)
        {
            int columnNum = 0;

            if (column > manCount)
            {
                columnNum = manCount;
            }
            else
            {
                columnNum = column;
                manCount -= column;
            }

            for (int j = 0; j < columnNum; j++)
            {
                Vector3 target;

                if (j % 2 == 0)
                {
                    target = hitPoint + (j / 2) * rightVector * 1.3f + backwardVector * i * 1.3f;
                }
                else
                {
                    target = hitPoint - ((j + 1) / 2) * rightVector * 1.3f + backwardVector * i * 1.3f;
                }

                markers[j + (i * column)].transform.position = target + Vector3.up * 0.1f;

                positions.Add(target);
            }
        }
    }

    private void LookDirection()
    {
        Vector3 vectorBetween = hitPoint - new Vector3(mans[0].transform.position.x, hitPoint.y, mans[0].transform.position.z);
        Vector3 sumVector = hitPoint + vectorBetween.normalized;

        dirTransform.position = hitPoint;
        dirTransform.LookAt(sumVector, Vector3.up);

        rightVector = dirTransform.right;
        backwardVector = -dirTransform.forward;
    }
}
