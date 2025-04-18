using System.Reflection;
using BudgetBuddy.Infrastructure.Attributes;
using BudgetBuddy.Infrastructure.Enums.Encryption;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BudgetBuddy.Database.Internal.Encryption;

internal class EncryptedProperty
{
    private EncryptedProperty(IMutableProperty property, StorageFormat storageFormat)
    {
        Property = property;
        StorageFormat = storageFormat;
    }

    public IMutableProperty Property { get; }

    public StorageFormat StorageFormat { get; }

    public static EncryptedProperty Create(IMutableProperty property)
    {
        StorageFormat? storageFormat = null;

        var encryptedAttribute =
            property.PropertyInfo?.GetCustomAttribute<EncryptedAttribute>(false);

        if (encryptedAttribute != null) storageFormat = encryptedAttribute.Format;

        var encryptedAnnotation = property.FindAnnotation(EncryptionAnnotations.IsEncrypted);

        if (encryptedAnnotation != null && (bool)encryptedAnnotation.Value)
            storageFormat = (StorageFormat)property.FindAnnotation(EncryptionAnnotations.StorageFormat)?.Value;

        return storageFormat.HasValue ? new EncryptedProperty(property, storageFormat.Value) : null;
    }
}