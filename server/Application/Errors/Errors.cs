using System.Collections.Generic;

using FluentResults;

namespace Application.Errors;

public class NotFoundError(string message) : Error(message);
public class ValidationError(string message) : Error(message);
public class ExternalServiceError(string message) : Error(message);
public class UnauthorizedError(string message) : Error(message);