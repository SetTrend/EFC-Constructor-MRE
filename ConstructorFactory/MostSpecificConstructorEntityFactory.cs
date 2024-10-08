using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using ConstructorFactoryInterfaces;

namespace ConstructorFactory;

/// <summary>
///		Provides methods for creating and initalizing
///		new objects of type <typeparamref name="TEntity"/>
///		using data provided in <see cref="DataRow"/> objects.
/// </summary>
/// <typeparam name="TEntity">
///		<see cref="Type"/> of objects to be created.
/// </typeparam>
[DebuggerDisplay($"ShortcutOnDefault = {{{nameof(ShortcutOnDefault)}}}")]
public class MostSpecificConstructorEntityFactory<TEntity> : IEntityFactory<TEntity>
{
	/// <summary>
	///		If <see langword="true"/>, prefers the parameterless
	///		constructor if available; <see langword="false"/> if
	///		the most specific constructor is always to be preferred.
	/// </summary>
	public bool ShortcutOnDefault { [DebuggerStepThrough()] get; [DebuggerStepThrough()] set; }



	/// <summary>
	///		Initializes a new <see cref="MostSpecificConstructorEntityFactory{TEntity}"/>
	///		object.
	/// </summary>
	/// <param name="shortcutOnDefault">
	///		If <see langword="true"/>, prefers the parameterless
	///		constructor if available; <see langword="false"/> if
	///		the most specific constructor is always to be preferred.
	/// </param>
	[RequiresDynamicCode($"{nameof(MostSpecificConstructorEntityFactory<TEntity>)} isn't fully compatible with NativeAOT, and running the application may generate unexpected runtime failures.")]
	[RequiresUnreferencedCode($"{nameof(MostSpecificConstructorEntityFactory<TEntity>)} isn't fully compatible with trimming, and running the application may generate unexpected runtime failures.")]
#pragma warning disable IDE0290    // Disable warning "use primary constructor". We want distinct XML documentation for constructor and class.                                                                                                              
	public MostSpecificConstructorEntityFactory(bool shortcutOnDefault) => ShortcutOnDefault = shortcutOnDefault;
#pragma warning restore IDE0290



	/// <inheritdoc/>
	public ConstructorInfo FindConstructor(DataRow dataRow)
	{
		IEnumerable<ConstructorInfo> constructors = typeof(TEntity).GetConstructors();


		// ------ if ShortcutOnDefault == true, prefer parameterless constructor ------------------------------------

		if (ShortcutOnDefault && constructors.FirstOrDefault(constr => constr.GetParameters().Length == 0) is ConstructorInfo constructor) return constructor;


		// ------ return most specific constructor for current non-null data ------------------------------------

		List<DataColumn> nonNullCols = new List<DataColumn>(dataRow.ItemArray.Length);

		foreach (DataColumn dCol in dataRow.Table.Columns) if (dataRow[dCol] != DBNull.Value) nonNullCols.Add(dCol);

		string[] nonNullColNames = nonNullCols.Select(dcol => dcol.ColumnName).ToArray();   // tiny performance optimization

		return constructors
			.Where(c =>
			{
				ParameterInfo[] conParams = c.GetParameters();

				return conParams.All(param => Nullable.GetUnderlyingType(param.ParameterType) is not null || nonNullColNames.Any(n => string.Compare(n, param.Name, true) == 0));
			})
			.OrderByDescending(constr => constr.GetParameters().Length)
			.First()
			;
	}

	/// <inheritdoc/>
	public TEntity InvokeConstructorAndInitializers(ConstructorInfo constr, DataRow dataRow)
	{
		DataColumnCollection colColl = dataRow.Table.Columns;
		int colCount = colColl.Count;
		List<DataColumn> columns = new List<DataColumn>(colCount);


		// ------ copy DataColumnCollection to List, so that we can discern non-null from null valued columns below ------------------------------------

		foreach (DataColumn column in colColl) columns.Add(column);


		// ------ call constructor using the specified data ------------------------------------

		ParameterInfo[] paramList = constr.GetParameters();
		int paramCount = paramList.Length;
		object?[] arguments = new object?[paramCount];

		for (int i = 0;i < paramCount;++i)
		{
			ParameterInfo param = paramList[i];
			DataColumn column = columns.First(col => string.Compare(col.ColumnName, param.Name, true) == 0);
			object val = dataRow[column];

			arguments[i] = val == DBNull.Value ? null : val;

			columns.Remove(column);
		}

		try
		{
			TEntity newObject = (TEntity)constr.Invoke(arguments);


			// ------ initialize properties from data not used by constructor ------------------------------------

			PropertyInfo[] properties = typeof(TEntity).GetProperties();

			foreach (DataColumn column in columns)
			{
				object value = dataRow[column];

				if (value != DBNull.Value)
				{
					PropertyInfo propInfo = properties.First(prop => string.Compare(column.ColumnName, prop.Name, true) == 0);

					if (Nullable.GetUnderlyingType(propInfo.PropertyType) is Type nullType)
						if (nullType.IsEnum) propInfo.SetValue(newObject, Enum.Parse(nullType, value.ToString()!));
						else propInfo.SetValue(newObject, Convert.ChangeType(value, nullType));
					else propInfo.SetValue(newObject, value);
				}
			}
			return newObject;
		}
		catch (TargetInvocationException ex) { throw ex.InnerException ?? ex; } // did the object constructor throw an exception? If so, return the constructor's exception.
	}

	/// <inheritdoc/>
	public IEnumerable<TEntity> CreateObjects(DataTable dataTable)
	{
		TEntity[] items = new TEntity[dataTable.Rows.Count];

		for (int i = dataTable.Rows.Count;i > 0;)
		{
			DataRow row = dataTable.Rows[--i];
			items[i] = InvokeConstructorAndInitializers(FindConstructor(row), row);
		}

		return items;
	}
}
