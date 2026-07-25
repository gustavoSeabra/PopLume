﻿namespace PopLume.Domain.Entities;

public class ProdutoComposicao
{
    public Guid IdProdutoComposicao { get; set; }
    public Guid? IdProdutoPai { get; set; }
    public Guid? IdProdutoFilho { get; set; }
    public int Quantidade { get; set; }

    // Propriedades de Navegação
    public virtual Produto? ProdutoPai { get; set; }
    public virtual Produto? ProdutoFilho { get; set; }
}
