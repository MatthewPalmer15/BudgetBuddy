// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using ValidationResult = BudgetBuddy.Infrastructure.Validation.Results.ValidationResult;

namespace BudgetBuddy.Infrastructure.Validation;

public interface IDataAnnotationsValidator<in TEntity> where TEntity : class
{
    /// <summary>
    ///     Validates the specified entity synchronously using data annotations.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <returns>A ValidationResult object that contains the results of the validation.</returns>
    public ValidationResult Validate(TEntity entity);

    /// <summary>
    ///     Validates the specified entity asynchronously using data annotations.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a ValidationResult object that
    ///     contains the results of the validation.
    /// </returns>
    public Task<ValidationResult> ValidateAsync(TEntity entity);
}