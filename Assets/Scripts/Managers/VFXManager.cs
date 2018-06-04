using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _robotExplosion;
        [SerializeField]
        private ParticleSystem _robotSparks;

        public void PlayRobotExplosionEffect(Vector3 position)
        {
            _robotExplosion.transform.position = position;
            _robotExplosion.gameObject.SetActive(true);
            _robotExplosion.Play();
            StartCoroutine(TurnOffEffect(_robotExplosion.gameObject, 2f));
        }

        public void PlayRobotSparksEffect(Vector3 position, Vector3 direction)
        {
            _robotSparks.GetComponent<Light>().intensity = 15;
            _robotSparks.GetComponent<Light>().intensity = Mathf.Lerp(15, 0, 0.3f);
            _robotSparks.transform.position = position;
            _robotSparks.transform.forward = direction;
            _robotSparks.gameObject.SetActive(true);
            _robotSparks.Play();
            StartCoroutine(TurnOffEffect(_robotSparks.gameObject, 1f));
        }

        private IEnumerator TurnOffEffect(GameObject obj, float timeToInactive)
        {
            yield return new WaitForSeconds(timeToInactive);
            obj.SetActive(false);
        }

    }
}