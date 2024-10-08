using System;
using System.Data;

using ConstructorFactory;

using Model;
using Model.Enums;

namespace ConstructorFactoryTests;

[TestClass]
public class CreateObjectsTests
{
	private readonly DataTable _dTable = new DataTable();



	[TestInitialize]
	public void InitDataTable()
	{
		_dTable.Columns.AddRange(
			[ new DataColumn("Id", typeof(int))
			, new DataColumn("IsRequired", typeof(bool))
			, new DataColumn("DataType", typeof(byte))
			, new DataColumn("Precision", typeof(byte))
			, new DataColumn("KnowledgeCategory", typeof(byte))
			, new DataColumn("Test", typeof(int))
			]);
	}


	/// <summary>
	///		This test should throw an <see cref="ArgumentException"/>
	///		for each of the invalid <paramref name="itemData"/> arguments.
	/// </summary>
	/// <remarks>
	///		The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///		uses a <c>Boolean</c> switch to return the parameterless constructor
	///		if it exists. This test sets this <c>Boolean</c> switch to
	///		<see langword="false"/>, so the corresponding constructor is 
	///		being invoked.
	/// </remarks>
	[TestMethod]
	public void CheckInvalidDataConstructorValidation()
	{
		_dTable.Rows.Add(1, true, ItemDataType.Integer, 2);
		_dTable.Rows.Add(1, true, ItemDataType.Integer, null, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(1, true, ItemDataType.Float);
		_dTable.Rows.Add(1, true, ItemDataType.Float, null, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(1, true, ItemDataType.Currency);
		_dTable.Rows.Add(1, true, ItemDataType.Currency, null, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(1, true, ItemDataType.Percent);
		_dTable.Rows.Add(1, true, ItemDataType.Percent, null, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(1, true, ItemDataType.Date, 1);
		_dTable.Rows.Add(1, true, ItemDataType.Date, null, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(1, true, ItemDataType.YearMonth, 50);
		_dTable.Rows.Add(1, true, ItemDataType.YearMonth, null, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(1, true, ItemDataType.KeywordList);
		_dTable.Rows.Add(1, true, ItemDataType.KeywordList, 150);
		_dTable.Rows.Add(1, true, ItemDataType.String, 1);
		_dTable.Rows.Add(1, true, ItemDataType.String, null, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(1, true, ItemDataType.MultiString, 2);
		_dTable.Rows.Add(1, true, ItemDataType.MultiString, null, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(1, true, ItemDataType.Markdown, 3);
		_dTable.Rows.Add(1, true, ItemDataType.Markdown, null, KnowledgeCategory.ComputerLanguage);

		MostSpecificConstructorEntityFactory<ProjectItem> factory = new MostSpecificConstructorEntityFactory<ProjectItem>(false);

		foreach (DataRow row in _dTable.Rows) Assert.ThrowsException<ArgumentException>(() => factory.InvokeConstructorAndInitializers(factory.FindConstructor(row), row));
	}


	/// <summary>
	///		This test should throw an <see cref="ArgumentException"/>
	///		for each of the invalid <paramref name="itemData"/> arguments.
	/// </summary>
	/// <remarks>
	///		The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///		uses a <c>Boolean</c> switch to return the parameterless constructor
	///		if it exists. This test sets this <c>Boolean</c> switch to
	///		<see langword="false"/>, so the corresponding constructor is 
	///		being invoked.
	/// </remarks>
	[TestMethod]
	public void CheckValidDataConstructorValidation()
	{
		_dTable.Rows.Add(1, true, ItemDataType.Currency, 2);
		_dTable.Rows.Add(1, true, ItemDataType.KeywordList, null, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(1, true, ItemDataType.String);

		MostSpecificConstructorEntityFactory<ProjectItem> factory = new MostSpecificConstructorEntityFactory<ProjectItem>(false);

		foreach (DataRow row in _dTable.Rows) factory.InvokeConstructorAndInitializers(factory.FindConstructor(row), row);
	}
}
