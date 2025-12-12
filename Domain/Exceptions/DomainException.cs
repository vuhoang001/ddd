namespace Domain.Exceptions;

// Base Domain Exception
public abstract class DomainException : Exception
{
    public string ErrorCode { get; }
    public int StatusCode { get; }

    protected DomainException(string message, string errorCode, int statusCode = 400)
        : base(message)
    {
        ErrorCode  = errorCode;
        StatusCode = statusCode;
    }

    protected DomainException(string message, string errorCode, int statusCode, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode  = errorCode;
        StatusCode = statusCode;
    }
}

// 1. Validation Exception - Lỗi validate dữ liệu đầu vào
public class ValidationException : DomainException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(string message)
        : base(message, "VALIDATION_ERROR", 400)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.", "VALIDATION_ERROR", 400)
    {
        Errors = errors;
    }

    public ValidationException(string field, string error)
        : base($"Validation failed for {field}", "VALIDATION_ERROR", 400)
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, new[] { error } }
        };
    }
}

// 2. Business Logic Exception - Lỗi logic nghiệp vụ
public class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message, string errorCode = "BUSINESS_RULE_VIOLATION")
        : base(message, errorCode, 422)
    {
    }

    public BusinessRuleException(string message, string errorCode, Exception innerException)
        : base(message, errorCode, 422, innerException)
    {
    }
}

// 3. Not Found Exception - Không tìm thấy resource
public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with id '{key}' was not found.", "NOT_FOUND", 404)
    {
    }

    public NotFoundException(string message)
        : base(message, "NOT_FOUND", 404)
    {
    }
}

// 4. Conflict Exception - Xung đột dữ liệu (duplicate, race condition)
public class ConflictException : DomainException
{
    public ConflictException(string message, string errorCode = "CONFLICT")
        : base(message, errorCode, 409)
    {
    }

    public ConflictException(string entityName, object key)
        : base($"{entityName} with id '{key}' already exists.", "DUPLICATE_ENTITY", 409)
    {
    }
}

// 5. Unauthorized Exception - Không có quyền truy cập
public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message = "Unauthorized access.")
        : base(message, "UNAUTHORIZED", 401)
    {
    }
}

// 6. Forbidden Exception - Không có quyền thực hiện hành động
public class ForbiddenException : DomainException
{
    public ForbiddenException(string message = "You don't have permission to perform this action.")
        : base(message, "FORBIDDEN", 403)
    {
    }

    public ForbiddenException(string resource, string action)
        : base($"You don't have permission to {action} {resource}.", "FORBIDDEN", 403)
    {
    }
}

// 7. External Service Exception - Lỗi từ service bên ngoài
public class ExternalServiceException : DomainException
{
    public string ServiceName { get; }

    public ExternalServiceException(string serviceName, string message)
        : base($"External service '{serviceName}' error: {message}", "EXTERNAL_SERVICE_ERROR", 502)
    {
        ServiceName = serviceName;
    }

    public ExternalServiceException(string serviceName, string message, Exception innerException)
        : base($"External service '{serviceName}' error: {message}", "EXTERNAL_SERVICE_ERROR", 502, innerException)
    {
        ServiceName = serviceName;
    }
}

public class InvalidOperationException : DomainException
{
    public InvalidOperationException(string message)
        : base(message, "INVALID_OPERATION", 400)
    {
    }

    public InvalidOperationException(string entityName, string currentState, string attemptedAction)
        : base($"Cannot {attemptedAction} {entityName} in {currentState} state.", "INVALID_OPERATION", 400)
    {
    }
}