using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private GameObject _emissionObject;
    [SerializeField] private float _blinkSpeed = 1f;
    [SerializeField] private float _minIntensity = 0.5f;
    [SerializeField] private float _maxIntensity = 1f;
    [SerializeField] private float _emmisionIntensity = 1f;
    private Material _emissionMaterial;
    [SerializeField]private bool _isBlinking = false;
    void Start()
    {
        _emissionMaterial = new Material(_emissionObject.GetComponent<Renderer>().material);
        StartCoroutine(Flicker());
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isBlinking)
        {
            float intensity = Mathf.Lerp(_minIntensity, _maxIntensity, Mathf.PingPong(Time.time * _blinkSpeed, 1));
            _light.intensity = intensity;
            _emissionMaterial.SetColor("_EmissionColor", new Color(intensity * _emmisionIntensity, intensity * _emmisionIntensity, intensity * _emmisionIntensity));
            _emissionObject.GetComponent<Renderer>().material = _emissionMaterial;
        }
    }

    private IEnumerator Flicker()
    {
        while(true)
        {
            _isBlinking = true;
            float curIntensity = _light.intensity;
            Color currentColor = _emissionMaterial.GetColor("_EmissionColor");
            
            _light.intensity *= 0.9f;
            _emissionMaterial.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
            _emissionObject.GetComponent<Renderer>().material = _emissionMaterial;
            yield return new WaitForSeconds(0.1f);
            
            _light.intensity = curIntensity;
            _emissionMaterial.SetColor("_EmissionColor", currentColor);
            _emissionObject.GetComponent<Renderer>().material = _emissionMaterial;
            
            _isBlinking = false;
            yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        }
    }
}
