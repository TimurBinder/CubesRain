using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Cube : MonoBehaviour
{
    private int _minDestroyDelay = 2;
    private int _maxDestroyDelay = 5;
    private bool _isActivatedDisable;
    private MeshRenderer _meshRenderer;
    private Color _originalColor;

    public event UnityAction<Cube> DisableActivated;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
    }

    private void OnEnable()
    {
        _isActivatedDisable = false;
        _meshRenderer.material.color = _originalColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isActivatedDisable)
            return;

        if (collision.gameObject.TryGetComponent(out Platform platform))
        {
            _isActivatedDisable = true;
            Color activatedColor = Random.ColorHSV(); ;
            gameObject.GetComponent<Renderer>().material.color = activatedColor;
            int destroyDelay = Random.Range(_minDestroyDelay, _maxDestroyDelay + 1);
            StartCoroutine(Disable(destroyDelay));
        }
    }

    private IEnumerator Disable(int delay)
    {
        yield return new WaitForSeconds(delay);
        DisableActivated?.Invoke(this);
    }
}
