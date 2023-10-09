using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class S_IReaderTest : MonoBehaviour
{
    [SerializeField] private S_InputReader s_InputReader;
    // Start is called before the first frame update
    void Start()
    {
        s_InputReader.MoveEvent += HandleMove;
    }

    private void OnDestroy() {
        s_InputReader.MoveEvent -= HandleMove;
    }

    private void HandleMove(UnityEngine.Vector2 movement){
        Debug.Log(movement);
    }

}
