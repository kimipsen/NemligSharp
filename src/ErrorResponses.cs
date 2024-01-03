namespace NemligSharp;

public record ErrorResponse();
public record NotFoundResponse() : ErrorResponse();
public record  UnknownErrorResponse() : ErrorResponse();