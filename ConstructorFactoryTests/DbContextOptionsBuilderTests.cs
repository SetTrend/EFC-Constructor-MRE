using System.Data;
using System.Reflection;

using ConstructorFactory;

using ConstructorFactoryInterfaces;

using Model;
using Model.Enums;

namespace ConstructorFactoryTests;

[TestClass]
public class DbContextOptionsBuilderTests
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
	///		Sample implementation demonstrating the use of
	///		<c>DbContextOptionsBuilder.UseConstructorFactory()</c>.
	/// </summary>
	/// <remarks>
	///		<para>
	///			This is rather a dummy test. It registers the
	///			<see cref="ConstructorFactoryImpl"/> factory and
	///			instantiates a sample object.
	///		</para>
	///		<para>
	///			The sample implementation of <see cref="IEntityFactory{TEntity}"/>
	///			uses a <c>Boolean</c> switch to return the parameterless constructor
	///			if it exists. This test sets this <c>Boolean</c> switch to
	///			<see langword="true"/> and checks if the parameterless entity
	///			constructor is being returned.
	///		</para>
	/// </remarks>
	[TestMethod]
	public void UseConstructorFactoryTest()
	{
		ConstructorFactoryImpl factory = new ConstructorFactoryImpl(true);

		_dTable.Rows.Add(1, true, (byte)ItemDataType.KeywordList, null, (byte)KnowledgeCategory.ComputerLanguage, 1);

		ConstructorInfo cInfo = factory.UseEntityFactory<ProjectItem>().FindConstructor(_dTable.Rows[0]);

		Assert.AreEqual(0, cInfo.GetParameters().Length);
	}
}
