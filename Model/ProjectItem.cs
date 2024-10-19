using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using Model.Enums;

namespace Model;

/// <summary>
///		Project grouping item, providing a container for a
///		single piece of information.
/// </summary>
public class ProjectItem
{
	// ====== Keys ======================================================

	/// <summary>
	///		Database Id for referencing this object.
	/// </summary>
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int Id { get; init; }


	// ====== Data ======================================================

	/// <summary>
	///		<see langword="true"/> if a value for this project item
	///		must be provided; <see langword="false"/> if this project
	///		item is optional.
	/// </summary>
	public bool IsRequired { get; set; }

	/// <summary>
	///		This project item's data type.
	/// </summary>
	public ItemDataType DataType { get; set; }

	/// <summary>
	///		A floating number's decimal places.
	/// </summary>
	/// <remarks>
	///		Only applicable if <see cref="DataType"/>
	///		is <see cref="ItemDataType.Float"/>,
	///		<see cref="ItemDataType.Currency"/> or
	///		<see cref="ItemDataType.Percent"/>.
	/// </remarks>
	public byte? Precision { get; set; }

	/// <summary>
	///		Keyword list category for selecting a group
	///		of keyword list items.
	/// </summary>
	/// <remarks>
	///		Only applicable if <see cref="DataType"/>
	///		is <see cref="ItemDataType.KeywordList"/>.
	/// </remarks>
	public KnowledgeCategory? KnowledgeCategory { get; set; }

	/// <summary>
	///		Used in unit tests. Don't use in code.
	/// </summary>
	public int? Test { get; set; }


	// ====== Computed Properties ======================================================

	/// <summary>
	///		<see langword="true"/>, if this project
	///		item's data type supports decimal places;
	///		<see langword="false"/> otherwise.
	/// </summary>
	public bool IsPrecisionType => HasPrecision(DataType);

	/// <summary>
	///		<see langword="true"/>, if this project item's
	///		data type supports a keyword list category;
	///		<see langword="false"/> otherwise.
	/// </summary>
	public bool IsCategoryType => HasCategory(DataType);


	// ====== Constructors ======================================================

	/// <summary>
	///		Creates a new, uninitialized <see cref="ProjectItem"/> object.
	/// </summary>
	/// <remarks>
	///		This constructor is required by Entity Framework Core.
	///		Don't use in code.
	/// </remarks>
	public ProjectItem() { }

	/// <summary>
	///		Initializes a new <see cref="ProjectItem"/> object
	///		for generic data types.
	/// </summary>
	/// <param name="id">
	///		Database Id for referencing this object.
	/// </param>
	/// <param name="isRequired">
	///		<see langword="true"/> if a value for this profile item
	///		must be provided; <see langword="false"/> if this profile
	///		item is optional.
	/// </param>
	/// <param name="dataType">
	///		This project item's data type.
	/// </param>
	/// <exception cref="ArgumentException">
	///		Thrown if the data type requires a precision or a category.
	/// </exception>
	public ProjectItem(int id, bool isRequired, ItemDataType dataType)
	{
		if (HasPrecision(dataType)) throw new ArgumentException($"You must provide a precision for decimal number data types. The current data type is: \"{dataType}\".", nameof(dataType));
		else if (HasCategory(dataType)) throw new ArgumentException($"You must provide a knowledge category for the keyword list data type. The current data type is: \"{dataType}\".", nameof(dataType));
		else
		{
			Id = id;
			IsRequired = isRequired;
			DataType = dataType;
		}
	}

	/// <summary>
	///		Initializes a new <see cref="ProjectItem"/> object
	///		for <see cref="ItemDataType.Float"/>,
	///		<see cref="ItemDataType.Currency"/> or
	///		<see cref="ItemDataType.Percent"/> data types.
	/// </summary>
	/// <param name="id">
	///		Database Id for referencing this object.
	/// </param>
	/// <param name="isRequired">
	///		<see langword="true"/> if a value for this profile item
	///		must be provided; <see langword="false"/> if this profile
	///		item is optional.
	/// </param>
	/// <param name="dataType">
	///		This project item's data type.
	/// </param>
	/// <param name="precision">
	///		Decimal places to display for this project item.
	/// </param>
	/// <exception cref="ArgumentException">
	///		Thrown if the data type doesn't support a decimal precision.
	/// </exception>
	public ProjectItem(int id, bool isRequired, ItemDataType dataType, byte precision)
	{
		if (!HasPrecision(dataType)) throw new ArgumentException($"A precision may only be provided for decimal number data types. The current data type is: \"{dataType}\".", nameof(dataType));

		Id = id;
		IsRequired = isRequired;
		DataType = dataType;
		Precision = precision;
	}

	/// <summary>
	///		Initializes a new <see cref="ProjectItem"/> object for
	///		the <see cref="ItemDataType.KeywordList"/> data type.
	/// </summary>
	/// <param name="id">
	///		Database Id for referencing this object.
	/// </param>
	/// <param name="isRequired">
	///		<see langword="true"/> if a value for this profile item
	///		must be provided; <see langword="false"/> if this profile
	///		item is optional.
	/// </param>
	/// <param name="dataType">
	///		This project item's data type.
	/// </param>
	/// <param name="knowledgeCategory">
	///		This project items keyword list category.
	/// </param>
	/// <exception cref="ArgumentException">
	///		Thrown if the data type doesn't support a keyword list category.
	/// </exception>
	public ProjectItem(int id, bool isRequired, ItemDataType dataType, KnowledgeCategory knowledgeCategory)
	{
		if (!HasCategory(dataType)) throw new ArgumentException($"A knowledge category may only be provided for the keyword list data type. The current data type is: \"{dataType}\".", nameof(dataType));

		Id = id;
		IsRequired = isRequired;
		DataType = dataType;
		KnowledgeCategory = knowledgeCategory;
	}

	/// <summary>
	///		<para>
	///			Initializes a new <see cref="ProjectItem"/> object for
	///			the <see cref="ItemDataType.KeywordList"/> data type.
	///		</para>
	///		<para>
	///			This constructor is only used for testing purposes.
	///			Don't use in code.
	///		</para>
	/// </summary>
	/// <param name="id">
	///		Database Id for referencing this object.
	/// </param>
	/// <param name="isRequired">
	///		<see langword="true"/> if a value for this profile item
	///		must be provided; <see langword="false"/> if this profile
	///		item is optional.
	/// </param>
	/// <param name="dataType">
	///		This project item's data type.
	/// </param>
	/// <param name="precision">
	///		Decimal places to display for this project item.
	/// </param>
	/// <param name="knowledgeCategory">
	///		This project items keyword list category.
	/// </param>
	/// <exception cref="ArgumentException">
	///		Thrown if the data type doesn't support a keyword list category.
	/// </exception>
	public ProjectItem(int id, bool isRequired, ItemDataType dataType, byte? precision, KnowledgeCategory knowledgeCategory) : this(id, isRequired, dataType, knowledgeCategory)
	{
		if (precision is not null) Precision = precision;
	}



	public override string ToString()
	{
		StringBuilder sb = new StringBuilder(100);

		sb
				.Append(Id)
				.Append(", ")
				.Append(IsRequired ? "" : "not ")
				.Append("required, ")
				.Append(DataType)
				;

		if (HasPrecision(DataType))
			sb
					.Append(", Precision = ")
					.Append(Precision)
					;
		else if (HasCategory(DataType))
			sb
					.Append(", Category = ")
					.Append(KnowledgeCategory);

		return sb.ToString();
	}



	private static bool HasPrecision(ItemDataType dataType) => dataType is ItemDataType.Currency or ItemDataType.Float or ItemDataType.Percent;

	private static bool HasCategory(ItemDataType dataType) => dataType is ItemDataType.KeywordList;
}