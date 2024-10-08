namespace Model.Enums;

/// <summary>
///		<para>
///			Keyword list category.
///		</para>
///		<para>
///			Used to filter the list of keywords, so only
///			keywords matching the corresponding category
///			will be displayed.
///		</para>
/// </summary>
/// <remarks>
///		Only applicable if <see cref="ItemDataType"/>
///		is <see cref="ItemDataType.KeywordList"/>.
/// </remarks>
public enum KnowledgeCategory : byte
{
	/// <summary>
	///		Provide all computer languages used.
	/// </summary>
	ComputerLanguage,

	/// <summary>
	///		Provide all frameworks and runtimes that has been coded for.
	/// </summary>
	FrameworkRuntime,

	/// <summary>
	///		Provide all infrastructural components used for execution.
	/// </summary>
	Infrastructure,

	/// <summary>
	///		Provide all operating systems worked with.
	/// </summary>
	OperatingSystem,

	/// <summary>
	///		Provide all products and tools that have been used.
	/// </summary>
	ProductTool
}
