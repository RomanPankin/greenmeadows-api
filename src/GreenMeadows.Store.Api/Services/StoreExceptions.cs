namespace GreenMeadows.Store.Api.Services;

/// <summary>A requested resource does not exist. Maps to HTTP 404.</summary>
public class NotFoundException(string message) : Exception(message);

/// <summary>A request is well-formed but breaks a business rule (e.g. insufficient stock). Maps to HTTP 409/422.</summary>
public class BusinessRuleException(string message) : Exception(message);
