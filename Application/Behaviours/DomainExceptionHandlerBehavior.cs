using System.Reflection;
using Application.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using InvalidOperationException = Domain.Exceptions.InvalidOperationException;

namespace Application.Behaviours;

public class DomainExceptionHandlerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<DomainExceptionHandlerBehavior<TRequest, TResponse>> _logger;

    public DomainExceptionHandlerBehavior(ILogger<DomainExceptionHandlerBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (DomainException ex)
        {
            LogException(ex);

            if (IsResultType(typeof(TResponse)))
            {
                return CreateFailureResult(ex);
            }

            throw;
        }
    }

    private void LogException(DomainException exception)
    {
        switch (exception)
        {
            case ValidationException:
                _logger.LogWarning(exception, "Validation error: {Message}", exception.Message);
                break;

            case NotFoundException:
                _logger.LogWarning(exception, "Resource not found: {Message}", exception.Message);
                break;

            case UnauthorizedException:
            case ForbiddenException:
                _logger.LogWarning(exception, "Authorization error: {Message}", exception.Message);
                break;

            case ExternalServiceException:
                _logger.LogError(exception, "External service error: {Message}", exception.Message);
                break;

            default:
                _logger.LogError(exception, "Domain error: {Message}", exception.Message);
                break;
        }
    }

    private static bool IsResultType(Type responseType)
    {
        return responseType.IsGenericType &&
            responseType.GetGenericTypeDefinition() == typeof(Result<>);
    }

    private static TResponse CreateFailureResult(DomainException exception)
    {
        var responseType = typeof(TResponse);

        if (exception is ValidationException validationException && validationException.Errors.Any())
        {
            return CreateValidationFailureResult(validationException);
        }

        var methodName = exception switch
        {
            ValidationException       => "Validation",
            NotFoundException         => "NotFound",
            ConflictException         => "Conflict",
            UnauthorizedException     => "Unauthorized",
            ForbiddenException        => "Forbidden",
            BusinessRuleException     => "BadRequest",
            ExternalServiceException  => "Internal",
            InvalidOperationException => "BadRequest",
            _                         => "Failure"
        };

        var method = responseType.GetMethod(
            methodName,
            BindingFlags.Public | BindingFlags.Static,
            null,
            new[] { typeof(string), typeof(string) },
            null
        );

        if (method == null)
        {
            method = responseType.GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(string) },
                null
            );
        }

        if (method == null && methodName != "Failure")
        {
            method = responseType.GetMethod(
                "Failure",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(ErrorType), typeof(string), typeof(string) },
                null
            );
        }

        if (method == null)
        {
            throw new System.InvalidOperationException(
                $"Type {responseType.Name} does not contain method {methodName}. " +
                $"Available static methods: {string.Join(", ", responseType.GetMethods(BindingFlags.Public | BindingFlags.Static).Select(m => m.Name).Distinct())}");
        }

        var    parameters = method.GetParameters();
        object result;

        if (parameters.Length == 3 && parameters[0].ParameterType == typeof(ErrorType))
        {
            var errorType = MapExceptionToErrorType(exception);
            result = method.Invoke(null, new object?[] { errorType, exception.Message, exception.ErrorCode }) ?? "";
        }
        else if (parameters.Length == 2)
        {
            result = method.Invoke(null, new object?[] { exception.Message, exception.ErrorCode }) ?? "";
        }
        else if (parameters.Length == 1)
        {
            result = method.Invoke(null, new object?[] { exception.Message }) ?? "";
        }
        else
        {
            result = method.Invoke(null, null) ?? "";
        }

        return (TResponse)result;
    }

    private static ErrorType MapExceptionToErrorType(DomainException exception)
    {
        return exception switch
        {
            ValidationException       => ErrorType.Validation,
            NotFoundException         => ErrorType.NotFound,
            ConflictException         => ErrorType.Conflict,
            UnauthorizedException     => ErrorType.Unauthorized,
            ForbiddenException        => ErrorType.Forbidden,
            BusinessRuleException     => ErrorType.BadRequest,
            ExternalServiceException  => ErrorType.Internal,
            InvalidOperationException => ErrorType.BadRequest,
            _                         => ErrorType.Internal
        };
    }

    private static TResponse CreateValidationFailureResult(ValidationException exception)
    {
        var responseType = typeof(TResponse);

        var errorMessage = exception.Message;
        if (exception.Errors.Any())
        {
            errorMessage = string.Join("; ",
                                       exception.Errors.SelectMany(e => e.Value.Select(v => $"{e.Key}: {v}")));
        }

        var validationMethod = responseType.GetMethod(
            "Validation",
            BindingFlags.Public | BindingFlags.Static,
            null,
            new[] { typeof(string), typeof(string) },
            null
        );

        if (validationMethod != null)
        {
            var result = validationMethod.Invoke(null, new object?[]
            {
                errorMessage,
                exception.ErrorCode
            });
            return (TResponse)result!;
        }

        var failureMethod = responseType.GetMethod(
            "Failure",
            BindingFlags.Public | BindingFlags.Static
        );

        if (failureMethod != null)
        {
            var result = failureMethod.Invoke(null, new object[]
            {
                ErrorType.Validation,
                errorMessage,
                exception.ErrorCode
            });
            return (TResponse)result!;
        }

        throw new System.InvalidOperationException(
            $"Type {responseType.Name} does not contain Validation or Failure method");
    }
}