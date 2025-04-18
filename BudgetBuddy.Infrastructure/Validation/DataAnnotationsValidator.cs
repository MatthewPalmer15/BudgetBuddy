using System.ComponentModel.DataAnnotations;
using ValidationResult = BlazorHybrid.Infrastructure.Validation.Results.ValidationResult;

namespace BlazorHybrid.Infrastructure.Validation;

public class DataAnnotationsValidator<TEntity> : IDataAnnotationsValidator<TEntity> where TEntity : class
{
    /// <summary>
    ///     Validates the specified entity synchronously using data annotations.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <returns>A ValidationResult object that contains the results of the validation.</returns>
    public ValidationResult Validate(TEntity entity)
    {
        List<System.ComponentModel.DataAnnotations.ValidationResult>? validationResults = new();

        ValidationContext? validationContext = new(entity, null, null);
        var isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);
        if (!isValid && validationResults.Count > 0)
            return ValidationResult.Failed(validationResults.Select(x => x.ErrorMessage ?? string.Empty));

        return ValidationResult.Success();
    }

    /// <summary>
    ///     Validates the specified entity asynchronously using data annotations.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a ValidationResult object that
    ///     contains the results of the validation.
    /// </returns>
    public async Task<ValidationResult> ValidateAsync(TEntity entity)
    {
        return await Task.Run(() => Validate(entity));
    }
}