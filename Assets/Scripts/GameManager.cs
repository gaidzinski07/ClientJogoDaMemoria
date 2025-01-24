using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Peca> pecas;
    public List<Sprite> images;
    public static GameManager Instance;
    public ClientSocketBehaviour client;
    public string ultimaJogada = "";
    public static string resultado = "";

    private void Start()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }

    private void Update()
    {
        if (ultimaJogada != "")
        {
            string[] temp = ultimaJogada.Split(" ");
            List<int> list = new List<int>();
            for (int i = 0; i < temp.Length; i++)
            {
                list.Add(int.Parse(temp[i]));
            }
            MostrarJogada(list[0], list[1], list[2], list[3]);
            ultimaJogada = "";
        }
        if(resultado != "")
        {
            SceneManager.LoadScene("End");
        }
    }

    /*Index = id da peca, vai de 0 a 4;
    Peca = objeto da peca. vai de 0 a 9*/
    public void MostrarJogada(int index1, int peca1, int index2, int peca2)
    {
        Peca p1 = pecas[peca1];
        Peca p2 = pecas[peca2];
        bool manter = (index1 == index2);
        p1.virarCarta(images[index1], manter);
        p2.virarCarta(images[index2], manter);
    }

    public void SelecionarPeca(Peca peca)
    {
        if (peca.selected)
        {
            peca.Selecionar(false);
            return;
        }
        
        int selecionadas = 0;
        foreach(Peca p in pecas)
        {
            selecionadas += p.selected ? 1 : 0;
        }
        
        if(selecionadas < 2)
        {
            peca.Selecionar(true);
        }
    }

    public void Jogar()
    {
        List<int> ps = new List<int>();

        for (int i = 0; i < pecas.Count; i++)
        {
            if (pecas[i].selected)
            {
                ps.Add(i);
            }
        }
        Debug.Log(ps.Count);
        if(ps.Count == 2)
        {
            client.SendData(ps[0] + " " + ps[1], MessageTypeEnum.PLAY);
        }
    }

}
