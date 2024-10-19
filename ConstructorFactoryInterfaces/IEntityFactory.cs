using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace ConstructorFactoryInterfaces;

/// <summary>
///   Provides methods for creating and initalizing
///   a new object of type <typeparamref name="TEntity"/>
///   from data provided in a <see cref="DataRow"/>
///   object.
/// </summary>
/// <typeparam name="TEntity">
///   <see cref="Type"/> of object to be created.
/// </typeparam>
public interface IEntityFactory<out TEntity>
{
  /// <summary>
  ///   Finds a constructor that's most appropriate for
  ///   the data provided in <paramref name="dataRow"/>.
  /// </summary>
  /// <param name="dataRow">
  ///   Data to be mapped to a new object of type
  ///   <typeparamref name="TEntity"/>.
  /// </param>
  /// <returns>
  ///   A constructor for creating and initializing a new
  ///   object of type <typeparamref name="TEntity"/>
  ///   based on the data specified in <paramref name="dataRow"/>.
  /// </returns>
  /// <exception cref="InvalidOperationException">
  ///   Thrown if no matching constructor could be found.
  /// </exception>
  public ConstructorInfo FindConstructor(DataRow dataRow);

  /// <summary>
  ///   Creates a new object from the data provided
  ///   by <paramref name="dataRow"/> using the constructor
  ///   specified by <paramref name="constructor"/>.
  /// </summary>
  /// <param name="constructor">
  ///   Constructor to be used for creating a new object
  ///   of type <typeparamref name="TEntity"/>.
  /// </param>
  /// <param name="dataRow">
  ///   Data to be used for initializing the new object
  ///   of type <typeparamref name="TEntity"/>.
  /// </param>
  /// <returns>
  ///   A new, initialized object of type <typeparamref name="TEntity"/>,
  ///   created by the specified constructor.
  /// </returns>
  public TEntity InvokeConstructorAndInitializers(ConstructorInfo constructor, DataRow dataRow);

  /// <summary>
  ///   Creates a collection of new objects from data provided
  ///   in <paramref name="dataTable"/>.
  /// </summary>
  /// <param name="dataTable">
  ///   Data to be used for initializing new objects
  ///   of type <typeparamref name="TEntity"/>.
  /// </param>
  /// <returns>
  ///   A collection of new, initialized objects of type
  ///   <typeparamref name="TEntity"/>.
  /// </returns>
  public IEnumerable<TEntity> CreateObjects(DataTable dataTable);
}
