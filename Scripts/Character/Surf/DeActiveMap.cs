using Unicorn.Unicorn.Scripts.Controller.SurfGame;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unicorn
{
    public class DeActiveMap : MonoBehaviour
    {
        void Update()
        {
            transform.position += Vector3.back * SurfLevelManager.Instance.mapSpeed * Time.deltaTime;
        }
        private void OnTriggerEnter(Collider collision)
        {
            print("DA VA CHAM MAP END");
            if (collision.gameObject.CompareTag("MapEnd"))
            {
                gameObject.SetActive(false);
                
                SurfLevelManager.Instance.ActivateMap();
            }
        }
        
        // void OnDrawGizmos() {
        //     MeshFilter meshFilter = GetComponent<MeshFilter>();
        //     if (meshFilter != null) {
        //         Mesh mesh = meshFilter.sharedMesh;
        //         if (mesh != null) {
        //             Gizmos.color = Random.ColorHSV();
        //             Gizmos.DrawMesh(mesh, transform.position, transform.rotation, transform.localScale);
        //         }
        //     }
        // }
    }
}
