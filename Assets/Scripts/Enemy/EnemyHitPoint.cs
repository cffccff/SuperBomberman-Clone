using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPoint : MonoBehaviour
{
    [SerializeField] private List<GameObject> list = new List<GameObject>();
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(list.Count==0)
        {
            if (!collision.gameObject.CompareTag("Explosion")) return;
            list.Add(collision.gameObject.GetComponent<Explosion>().belongToBomb);
            gameObject.GetComponentInParent<EnemyHealth>().Hurt();
            Invoke(nameof(RemoveBombFromList), 0.5f);
        }
        else
        {
            if (!collision.gameObject.CompareTag("Explosion")) return;
            if (InTheList(collision.gameObject.GetComponent<Explosion>().belongToBomb)) return;
            list.Add(collision.gameObject.GetComponent<Explosion>().belongToBomb);
            gameObject.GetComponentInParent<EnemyHealth>().Hurt();
            Invoke(nameof(RemoveBombFromList), 0.5f);
        }
        


    }
    private bool InTheList(GameObject bomb)
    {
        foreach (var t in list)
        {
            if (t == bomb) return true;
        }

        return false;
    }
    private void RemoveBombFromList()
    {
        list.RemoveAt(list.Count-1);
    }
}
