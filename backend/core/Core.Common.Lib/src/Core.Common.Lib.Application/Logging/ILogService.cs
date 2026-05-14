namespace Core.Common.Lib.Application.Logging;

public interface ILogService<T>
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? ex = null);

    void LogInfo(string className, string methodName, string message);
    void LogWarning(string className, string methodName, string message);
    void LogError(string className, string methodName, string message, Exception? ex = null);

    /// <summary>
    /// Loga o inicio de um handler, service ou método, incluindo o nome da classe, método e parâmetros (se fornecidos).
    /// </summary>
    /// <param name="className">Nome da classe.</param>
    /// <param name="methodName">Nome do método.</param>
    /// <param name="parameters">Parâmetros do método.</param>
    void LogStarting(string className, string methodName, object? parameters = null);

    /// <summary>
    /// Loga a conclusão de um handler, service ou método, incluindo o nome da classe e método. Pode ser usado para medir o tempo de execução se combinado com o LogStarting.
    /// </summary>
    /// <param name="className">Nome da classe.</param>
    /// <param name="methodName">Nome do método.</param>
    void LogCompleted(string className, string methodName);

    /// <summary>
    /// Loga uma falha em um handler, service ou método, incluindo o nome da classe, método e detalhes da exceção. Pode ser usado para rastrear erros específicos e suas causas.
    /// </summary>
    /// <param name="className">Nome da classe.</param>
    /// <param name="methodName">Nome do método.</param>
    /// <param name="ex">Detalhes da exceção.</param>
    void LogFailed(string className, string methodName, Exception ex);
    void LogFailed(string className, string methodName, string message, Exception ex);
}
