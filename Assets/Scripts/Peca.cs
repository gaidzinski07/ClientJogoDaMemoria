using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Peca : MonoBehaviour
{
    [SerializeField]
    private Sprite versoSprite;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject outline;
    public bool selected;
    public bool jaFoi = false;
    private float sx;

    private void Start()
    {
        sx = transform.localScale.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (jaFoi)
        {
            Selecionar(false);
        }
    }

    public void virarCarta(Sprite sprite, bool manter)
    {
        jaFoi = manter;
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOScaleX(0, .5f));
        s.AppendCallback(()=> { spriteRenderer.sprite = sprite; });
        s.Append(transform.DOScaleX(sx, .5f));
        if (!manter)
        {
            s.AppendInterval(1f);
            s.Append(transform.DOScaleX(0, .5f));
            s.AppendCallback(() => { spriteRenderer.sprite = versoSprite; });
            s.Append(transform.DOScaleX(sx, .5f));
        }
        s.Play();
    }

    public void Selecionar(bool s)
    {
        selected = s;
        outline.SetActive(selected);
    }

    void OnMouseDown()
    {
        GameManager.Instance.SelecionarPeca(this);
    }

}
