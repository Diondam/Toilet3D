using System;
using System.Collections;
using UnityEngine;

namespace Unicorn
{
    public class ItemScaleCandy : MonoBehaviour
    {
        public int scaleFactor;
        private Vector3 originScale;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var obj = other.gameObject.transform.parent.gameObject;
                //print("CO VA CHAM SCALE: "+ obj.name);
                ScaleItemY(obj);
                
            }
            if (other.gameObject.CompareTag("Finish"))
            {
                Invoke("DeActive", 0.5f);
            }
        }
        void DeActive()
        {
            gameObject.SetActive(false);
        }
        private int count = 0;
        public void ScaleItemY(GameObject obj)
        {
            count++;
            //print("SCALE thanh cong: "+ count);
            originScale = obj.transform.localScale;
            Vector3 newScale = originScale;
            newScale.y *= scaleFactor; // Gấp đôi kích thước theo trục y
            obj.transform.localScale = newScale;
            StartCoroutine(ResetScale(obj));
        }

        IEnumerator ResetScale(GameObject obj)
        {
            //print("RESET SCALE");
            yield return new WaitForSeconds(3f);
            obj.transform.localScale = originScale;
            gameObject.SetActive(false);
        }
    }
}