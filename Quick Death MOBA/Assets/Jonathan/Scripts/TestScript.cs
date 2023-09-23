using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private InputReader InputReader;
    // Start is called before the first frame update
    void Start()
    {
        InputReader.MoveEvent += HandleMove;
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        InputReader.MoveEvent -= HandleMove;
    }

    private void HandleMove(Vector2 movement)
    {
        Debug.Log(movement);
    }
}
