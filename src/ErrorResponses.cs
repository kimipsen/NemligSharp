namespace NemligSharp;

public record ErrorResponse();
public record NotFoundResponse() : ErrorResponse();
public record UnknownErrorResponse() : ErrorResponse();
public record NotLoggedInErrorResponse() : ErrorResponse();
public record JsonParseErrorResponse(string Message) : ErrorResponse();