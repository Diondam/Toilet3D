// using System;
// using System.Collections;
// using RayFire;
// using Unicorn.Unicorn.Scripts.Controller.KissGame;
// using UnityEngine;
//
// namespace Unicorn
// {
//     public class PumpkinKiss : MonoBehaviour
//     {
//         private RayfireRigid rfRigid;
//         public GameObject slashEffectPrefab;
//         KissLevelManager kmn => KissLevelManager.Instance;
//
//         private void Start()
//         {
//             rfRigid = GetComponent<RayfireRigid>();
//             transform.parent = LevelManager.Instance.transform;
//         }
//
//
//         private void Update()
//         {
//             //nếu vào vị trí có thể chém được
//             if (transform.position.y <= kmn.heighWin && transform.position.y > kmn.heighWin - 0.2f &&
//                 kmn.Result == LevelResult.NotDecided)
//             {
//                 kmn.canTap = true;
//                 kmn.readyToTap.gameObject.SetActive(true);
//                 print("Active image green");
//             }
//         }
//
//
//         private bool isWin;
//
//         private void OnCollisionEnter(Collision other)
//         {
//             if (other != null)
//             {
//                 if (other.gameObject.CompareTag("Player") && KissLevelManager.Instance.Result == LevelResult.NotDecided)
//                 {
//                     GameObject effect = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
//                     effect.transform.parent = kmn.transform;
//                     rfRigid.Demolish();
//                     kmn.numberToWin--;
//                     kmn.ChangeSlider();
//                     if (kmn.numberToWin == 0)
//                     {
//                         kmn.Win();
//                     }
//                 }
//                 else if (other.gameObject.CompareTag("Finish"))
//                 {
//                     GameObject effect = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
//                     effect.transform.parent = kmn.transform;
//                     rfRigid.Demolish();
//                 }
//             }
//         }
//     }
// }