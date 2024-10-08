using System;
using System.Data;

using ConstructorFactory;

using ConstructorFactoryInterfaces;

using Model;
using Model.Enums;

namespace ConstructorFactoryTests;

[TestClass]
public class InvokeConstructorAndInitializersTests
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
	///		Instantiates an object using the parameterless constructor.
	///		No optional property is used.
	/// </summary>
	/// <remarks>
	///		The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///		uses a <c>Boolean</c> switch to return the parameterless constructor
	///		if it exists. This test sets this <c>Boolean</c> switch to
	///		<see langword="true"/> and checks if the parameterless entity
	///		constructor is being invoked.
	/// </remarks>
	[TestMethod]
	public void PreferDefaultConstructorWithoutOptionalProperty()
	{
		ProjectItem expectedItem = new ProjectItem(1, true, ItemDataType.KeywordList, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(expectedItem.Id, expectedItem.IsRequired, (byte)expectedItem.DataType, null, (byte)expectedItem.KnowledgeCategory!, expectedItem.Test);

		DataRow row = _dTable.Rows[0];

		MostSpecificConstructorEntityFactory<ProjectItem> factory = new MostSpecificConstructorEntityFactory<ProjectItem>(true);
		AssertAreEqual(expectedItem, factory.InvokeConstructorAndInitializers(factory.FindConstructor(row), row));
	}

	/// <summary>
	///		Instantiates an object using the parameterless constructor.
	///		The optional property <c>Test</c> is used to check if it is
	///		set by property initialization.
	/// </summary>
	/// <remarks>
	///		The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///		uses a <c>Boolean</c> switch to return the parameterless constructor
	///		if it exists. This test sets this <c>Boolean</c> switch to
	///		<see langword="true"/> and checks if the parameterless entity
	///		constructor is being invoked.
	/// </remarks>
	[TestMethod]
	public void PreferDefaultConstructorWithOptionalProperty()
	{
		ProjectItem expectedItem = new ProjectItem(1, true, ItemDataType.KeywordList, KnowledgeCategory.ComputerLanguage) { Test = 1 };
		_dTable.Rows.Add(expectedItem.Id, expectedItem.IsRequired, (byte)expectedItem.DataType, null, (byte)expectedItem.KnowledgeCategory!, expectedItem.Test);

		DataRow row = _dTable.Rows[0];

		MostSpecificConstructorEntityFactory<ProjectItem> factory = new MostSpecificConstructorEntityFactory<ProjectItem>(true);
		AssertAreEqual(expectedItem, factory.InvokeConstructorAndInitializers(factory.FindConstructor(row), row));
	}

	/// <summary>
	///		Instantiates an object using the most specific available constructor.
	///		No optional property is used.
	/// </summary>
	/// <remarks>
	///		The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///		uses a <c>Boolean</c> switch to return the parameterless constructor
	///		if it exists. This test sets this <c>Boolean</c> switch to
	///		<see langword="false"/> and checks if the most specific available
	///		entity constructor is being invoked.
	/// </remarks>
	[TestMethod]
	public void PreferMostSpecificConstructorWithoutOptionalProperty()
	{
		ProjectItem expectedItem = new ProjectItem(1, true, ItemDataType.KeywordList, KnowledgeCategory.ComputerLanguage);
		_dTable.Rows.Add(expectedItem.Id, expectedItem.IsRequired, (byte)expectedItem.DataType, null, (byte)expectedItem.KnowledgeCategory!, expectedItem.Test);

		DataRow row = _dTable.Rows[0];

		MostSpecificConstructorEntityFactory<ProjectItem> factory = new MostSpecificConstructorEntityFactory<ProjectItem>(false);
		AssertAreEqual(expectedItem, factory.InvokeConstructorAndInitializers(factory.FindConstructor(row), row));
	}

	/// <summary>
	///		Instantiates an object using the most specific available constructor.
	///		The optional property <c>Test</c> is used to check if it is
	///		set by property initialization.
	/// </summary>
	/// <remarks>
	///		The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///		uses a <c>Boolean</c> switch to return the parameterless constructor
	///		if it exists. This test sets this <c>Boolean</c> switch to
	///		<see langword="false"/> and checks if the most specific available
	///		entity constructor is being invoked.
	/// </remarks>
	[TestMethod]
	public void PreferMostSpecificConstructorWithOptionalProperty()
	{
		ProjectItem expectedItem = new ProjectItem(1, true, ItemDataType.KeywordList, KnowledgeCategory.ComputerLanguage) { Test = 1 };
		_dTable.Rows.Add(expectedItem.Id, expectedItem.IsRequired, (byte)expectedItem.DataType, null, (byte)expectedItem.KnowledgeCategory!, expectedItem.Test);

		DataRow row = _dTable.Rows[0];

		MostSpecificConstructorEntityFactory<ProjectItem> factory = new MostSpecificConstructorEntityFactory<ProjectItem>(false);
		AssertAreEqual(expectedItem, factory.InvokeConstructorAndInitializers(factory.FindConstructor(row), row));
	}



	private static void AssertAreEqual(ProjectItem expected, ProjectItem actual)
	{
		Assert.AreEqual(expected.Id, actual.Id);
		Assert.AreEqual(expected.IsRequired, actual.IsRequired);
		Assert.AreEqual(expected.DataType, actual.DataType);
		Assert.AreEqual(expected.Precision, actual.Precision);
		Assert.AreEqual(expected.KnowledgeCategory, actual.KnowledgeCategory);
		Assert.AreEqual(expected.Test, actual.Test);
	}
}
