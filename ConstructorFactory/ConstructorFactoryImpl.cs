using System;
using System.Diagnostics;

using ConstructorFactoryInterfaces;

namespace ConstructorFactory;

/// <summary>
///		Provides methods for registering object
///		instantiation constructor factories per
///		<see cref="Type"/>.
/// </summary>
[DebuggerDisplay($"ShortcutOnDefault = {{{nameof(ShortcutOnDefault)}}}")]
public class ConstructorFactoryImpl : IConstructorFactory
{
	/// <summary>
	///		If <see langword="true"/>, prefers the parameterless
	///		constructor if available; <see langword="false"/> if
	///		the most specific constructor is always to be preferred.
	/// </summary>
	public bool ShortcutOnDefault { [DebuggerStepThrough()] get; [DebuggerStepThrough()] set; }



	/// <summary>
	///		Initializes a new <see cref="ConstructorFactoryImpl"/>
	///		object.
	/// </summary>
	/// <param name="shortcutOnDefault">
	///		If <see langword="true"/>, prefers the parameterless
	///		constructor if available; <see langword="false"/> if
	///		the most specific constructor is always to be preferred.
	/// </param>
#pragma warning disable IDE0290    // Disable warning "use primary constructor". We want distinct XML documentation for constructor and class.                                                                                                              
	public ConstructorFactoryImpl(bool shortcutOnDefault) => ShortcutOnDefault = shortcutOnDefault;
#pragma warning restore IDE0290



	/// <inheritdoc/>
	public IEntityFactory<TEntity> UseEntityFactory<TEntity>() => new MostSpecificConstructorEntityFactory<TEntity>(ShortcutOnDefault);
}
