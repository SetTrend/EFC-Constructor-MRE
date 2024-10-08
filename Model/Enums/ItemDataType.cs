namespace Model.Enums;

/// <summary>
///		Type of data to be used for an item.
/// </summary>
public enum ItemDataType : byte
{
	/// <summary>
	///		Integer value. No decimal places are used or displayed.
	/// </summary>
	Integer,


	/// <summary>
	///		Decimal value. The number of decimal places is determined
	///		by precision.
	/// </summary>
	Float,

	/// <summary>
	///		Decimal value, displayed with currency sign. The number of
	///		decimal places is determined by precision.
	/// </summary>
	Currency,

	/// <summary>
	///		Decimal value, displayed with percentage sign. The number
	///		of decimal places is determined by precision.
	/// </summary>
	Percent,


	/// <summary>
	///		Date value, displayed in <see cref="System.DateTime.ToShortDateString"/>
	///		format.
	/// </summary>
	Date,

	/// <summary>
	///		Date value, displayed in "yyyy-MM" format.
	/// </summary>
	YearMonth,


	/// <summary>
	///		List of keywords, taken from a pool of keywords.
	/// </summary>
	KeywordList,

	/// <summary>
	///		Localized string value.
	/// </summary>
	String,

	/// <summary>
	///		List of localized strings.
	/// </summary>
	MultiString,

	/// <summary>
	/// Localized string, stored in Markdown format.
	/// </summary>
	Markdown
}