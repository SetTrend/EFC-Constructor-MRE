using System;

namespace ConstructorFactoryInterfaces;

/// <summary>
///		Provides methods for registering object
///		instantiation constructor factories per
///		<see cref="Type"/>.
/// </summary>
public interface IConstructorFactory
{
	/// <summary>
	///		Gets the object instantiation factory
	///		for objects of type <typeparamref name="TEntity"/>.
	/// </summary>
	/// <typeparam name="TEntity">
	///		<see cref="Type"/> of object to be created.
	/// </typeparam>
	/// <returns>
	///		Factory object for instantiating objects
	///		of type <typeparamref name="TEntity"/>.
	/// </returns>
	IEntityFactory<TEntity> UseEntityFactory<TEntity>();
}