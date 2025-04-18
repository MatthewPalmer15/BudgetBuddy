// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace BlazorHybrid.Infrastructure.Validation.Results;

public class ValidationResult
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ValidationResult" /> class.
    /// </summary>
    /// <param name="isValid">Indicates whether the response is valid.</param>
    /// <param name="errors">Optional list of error messages associated with the response.</param>
    private ValidationResult(bool isValid, List<string>? errors = null)
    {
        IsValid = isValid;
        Errors = errors ?? [];
    }

    /// <summary>
    ///     Gets a value indicating whether the response is valid.
    /// </summary>
    public bool IsValid { get; private set; }

    /// <summary>
    ///     Gets the list of error messages associated with the response.
    /// </summary>
    public List<string> Errors { get; private set; }

    /// <summary>
    ///     Creates a successful response result.
    /// </summary>
    /// <returns>A successful response result.</returns>
    public static ValidationResult Success()
    {
        return new ValidationResult(true);
    }

    /// <summary>
    ///     Creates a failed response result with the specified error messages.
    /// </summary>
    /// <param name="errors">The collection of error messages associated with the failure.</param>
    /// <returns>A failed response result.</returns>
    public static ValidationResult Failed(IEnumerable<string> errors)
    {
        return new ValidationResult(false, errors.ToList());
    }
}