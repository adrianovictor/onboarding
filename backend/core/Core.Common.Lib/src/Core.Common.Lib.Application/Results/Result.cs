namespace Core.Common.Lib.Application.Results;


/// <summary>
/// Representa o resultado de uma operação — sucesso ou falha com erro estruturado.
/// Substitui o uso de exceções para controle de fluxo de negócio.
/// Sempre construído via factory methods — construtor direto não é permitido.
/// </summary>
public class Result
{
    public bool  IsSuccess { get; }
    public bool  IsFailure => !IsSuccess;
    public Error Error     { get; }

    // ── Factory methods ────────────────────────────────────────────────────

    public static Result         Success()                    => new(true,  Error.None);
    public static Result         Failure(Error error)         => new(false, error);
    public static Result<T>      Success<T>(T value)          => new(value,   true,  Error.None);
    public static Result<T>      Failure<T>(Error error)      => new(default, false, error);

    /// <summary>
    /// Cria a partir de um valor nullable — null vira Failure automaticamente.
    /// Útil para converter retornos de repositório sem if/else.
    /// </summary>
    public static Result<T> Create<T>(T? value) =>
        value is not null ? Success(value) : Failure<T>(Error.NullValue);

    // ── Combinação ─────────────────────────────────────────────────────────

    /// <summary>
    /// Retorna o primeiro Failure encontrado, ou Success se todos passarem.
    /// Útil para validar múltiplas regras antes de commitar.
    /// </summary>
    public static Result Combine(params Result[] results)
    {
        var failed = Array.Find(results, r => r.IsFailure);
        return failed ?? Success();
    }

    // ── Construtor ─────────────────────────────────────────────────────────

    protected Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true  when error != Error.None:
            case false when error == Error.None:
                throw new InvalidOperationException(
                    $"Estado inválido: IsSuccess={isSuccess}, Error={error}");

            default:
                IsSuccess = isSuccess;
                Error     = error;
                break;
        }
    }
}

/// <summary>
/// Resultado tipado — carrega valor em caso de sucesso.
/// </summary>
public sealed class Result<T> : Result
{
    private readonly T? _value;

    internal Result(T? value, bool isSuccess, Error error)
        : base(isSuccess, error) => _value = value;

    /// <summary>
    /// Acessa o valor. Lança se IsFailure — verifique IsSuccess antes.
    /// </summary>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            $"Resultado falhou com erro '{Error.Code}' — sem valor disponível.");

    /// <summary>Converte null implicitamente para Failure.</summary>
    public static implicit operator Result<T>(T? value) => Create(value);

    /// <summary>
    /// Transforma o valor em caso de sucesso, propagando erro caso contrário.
    /// </summary>
    public Result<TOut> Map<TOut>(Func<T, TOut> mapper) =>
        IsSuccess
            ? Result.Success(mapper(Value))
            : Result.Failure<TOut>(Error);

    /// <summary>
    /// Encadeia operações que retornam Result — monadic bind.
    /// Evita aninhamento de ifs para validações sequenciais.
    /// </summary>
    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder) =>
        IsSuccess ? binder(Value) : Result.Failure<TOut>(Error);

    /// <summary>
    /// Executa um dos dois delegates dependendo do estado.
    /// Garante que ambos os casos sejam tratados.
    /// </summary>
    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error);
}