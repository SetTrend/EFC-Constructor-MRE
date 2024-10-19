using System;
using System.Data;
using System.Reflection;

using ConstructorFactory;

using ConstructorFactoryInterfaces;

using Model;
using Model.Enums;

namespace ConstructorFactoryTests;

[TestClass]
public class FindConstructorTests
{
	private readonly DataTable _dTable = new DataTable();

	/// <summary>
	///		Valid data for project driven unit tests.
	/// </summary>
	private static Tuple<ProjectItem, int>[] ValidProjectItems =>
		[ new Tuple<ProjectItem,int>(new ProjectItem(1, true, ItemDataType.Currency, 2), 4)
		, new Tuple<ProjectItem,int>(new ProjectItem(2, true, ItemDataType.KeywordList, KnowledgeCategory.ComputerLanguage), 5)
		, new Tuple<ProjectItem,int>(new ProjectItem(3, true, ItemDataType.String), 3)
		];



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
	///		This test should return the parameterless constructor.
	/// </summary>
	/// <remarks>
	///		The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///		uses a <c>Boolean</c> switch to return the parameterless constructor
	///		if it exists. This test sets this <c>Boolean</c> switch to
	///		<see langword="true"/> and checks if the parameterless entity
	///		constructor is being returned.
	/// </remarks>
	[TestMethod]
	public void PreferDefaultConstructor()
	{
		_dTable.Rows.Add(1, true, (byte)ItemDataType.KeywordList, null, (byte)KnowledgeCategory.ComputerLanguage, 1);

		ConstructorInfo cInfo = new MostSpecificConstructorEntityFactory<ProjectItem>(true).FindConstructor(_dTable.Rows[0]);

		Assert.AreEqual(0, cInfo.GetParameters().Length);
	}

	/// <summary>
	///		This test should return the most specific constructor.
	/// </summary>
	/// <remarks>
	///		The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///		uses a <c>Boolean</c> switch to return the parameterless constructor
	///		if it exists. This test sets this <c>Boolean</c> switch to
	///		<see langword="false"/> and checks if the most specific available
	///		entity constructor is being returned.
	/// </remarks>
	[TestMethod]
	public void PreferMostSpecificConstructor()
	{
		_dTable.Rows.Add(1, true, (byte)ItemDataType.KeywordList, null, (byte)KnowledgeCategory.ComputerLanguage, 1);

		ConstructorInfo cInfo = new MostSpecificConstructorEntityFactory<ProjectItem>(false).FindConstructor(_dTable.Rows[0]);

		Assert.AreEqual(5, cInfo.GetParameters().Length);
	}

	/// <summary>
	///		This test should return the a specific constructor requiring
	///		all provided non-null data values.
	/// </summary>
	/// <remarks>
	///		The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///		uses a <c>Boolean</c> switch to return the parameterless constructor
	///		if it exists. This test sets this <c>Boolean</c> switch to
	///		<see langword="false"/> and checks if the specific available
	///		entity constructor is being returned.
	/// </remarks>
	[TestMethod]
	[DynamicData(nameof(ValidProjectItems))]
	public void SelectSpecificConstructor(ProjectItem itemData, int expected)
	{
		_dTable.Rows.Add(itemData.Id, itemData.IsRequired, itemData.DataType, itemData.Precision, itemData.KnowledgeCategory, itemData.Test);

		ConstructorInfo cInfo = new MostSpecificConstructorEntityFactory<ProjectItem>(false).FindConstructor(_dTable.Rows[0]);

		Assert.AreEqual(expected, cInfo.GetParameters().Length);
	}
}