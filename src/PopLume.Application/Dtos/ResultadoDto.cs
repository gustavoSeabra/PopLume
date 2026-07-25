namespace PopLume.Application.Dtos;

public sealed record ResultadoDto<T>
{
    private readonly string[]? _errors;

    // Propriedades
    public bool Ok { get; }
    public bool NotFound { get; }
    public T? Data { get; set; }

    // public PaginacaoDto? Paginacao { get; set; }

    public string? Message { get; }
    public string[]? Errors => (_errors is null || _errors.Length == 0) && !Ok ? [Message!] : _errors;

    public ResultadoDto()
    {
    }

    public ResultadoDto(bool ok, bool notFound, T? data, /*PaginacaoDto? paginacao, */ string? message, string[]? errors)
    {
        Ok = ok;
        NotFound = notFound;
        Data = data;
        // PaginacaoDto = paginacao;
        Message = message;
        _errors = errors;
    }

    // Métodos
    public static ResultadoDto<T> RetornaSucesso(T? data) =>
        new(true, false, data, /* default, */ null, null);

    //public static ResultadoDto<T> RetornaSucesso(T? data, PaginacaoDto paginacao) =>
    //    new(true, false, data,  paginacao, null, null, null);

    public static ResultadoDto<bool> RetornaSucesso(string mensagemSucesso) =>
        new(true, false, true, /* default, */ mensagemSucesso, null);

    public static ResultadoDto<T> RetornaErro(string mensagemErro) =>
        new(false, false, default, /*default, */ mensagemErro, null);

    public static ResultadoDto<T> RetornaErro(string mensagemErro, string[] erros) =>
        new(false, false, default, /*default, */ mensagemErro, erros);

    public static ResultadoDto<T> RetornaNaoEncontrado(string mensagem) =>
        new(false, true, default, /*default, */ mensagem, null);
}
