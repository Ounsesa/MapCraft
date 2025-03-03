using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashCan : MonoBehaviour
{
    [SerializeField]
    private Player m_player;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DestroyPiece);
    }

    // Update is called once per frame
    void DestroyPiece()
    {
        if (m_player && m_player.currentPiece && m_player.pieceController.placedPiecesList.Count > 0 && m_player.currentPiece.matrix[0][0] != 4) 
        {
            Destroy(m_player.currentPiece.gameObject);
            m_player.currentPiece = null;
        }
    }
}
