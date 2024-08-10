using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashCan : MonoBehaviour
{
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DestroyPiece);
    }

    // Update is called once per frame
    void DestroyPiece()
    {
        if (player && player.CurrentPiece && player.PieceController.PlacedPiecesList.Count > 0 && player.CurrentPiece.Matrix[0][0] != 4) 
        {
            Destroy(player.CurrentPiece.gameObject);
            player.CurrentPiece = null;
        }
    }
}
