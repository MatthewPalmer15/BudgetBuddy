using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace BudgetBuddy.Database.Entities;

/// <summary>
///     Represents a base entity with common properties such as ID, creation date, and status.
/// </summary>
public class BaseEntity<T> : IBaseEntity<T> where T : struct
{
    /// <summary>
    ///     Gets or sets the unique identifier of the entity.
    /// </summary>
    [Key]
    [XmlElement("Id")]
    [JsonProperty("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public T Id { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the entity has been marked as deleted.
    /// </summary>
    [XmlElement("Deleted")]
    [JsonProperty("deleted")]
    public bool Deleted { get; set; }
}

/// <summary>
///     Represents a base entity with common properties such as ID, creation date, and status.
/// </summary>
/// <typeparam name="T">The type of the entity's ID.</typeparam>
public interface IBaseEntity<T> where T : struct
{
    public T Id { get; set; }

    public bool Deleted { get; set; }
}