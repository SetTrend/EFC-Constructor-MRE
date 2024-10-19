# Proposal for implementing factory based object instantiation in EF Core

This repository is a proposal and POC for Entity Framework Core to select the most appropriate constructor when instantiating entitites.

It is referenced by [EF Core issue #10789](https://github.com/dotnet/efcore/issues/10789#issuecomment-2395688107).

## Current Situation

Currently, Entity Framework Core prefers to invoke parameterless constructors, using property initializers for instantiating entities.

If an entity class is defining constructors with parameters only, the least specific constructor is used.

This way, it's impossible to harden an object model by validating entities on creation, for example.

## Desired Situation

### EF Core Should Be Able To Choose The Right Constructor When Instantiating Entities Having Multiple Constructors From DB

Different needs require different constructor selection. I propose to apply a factory pattern for selecting the right entity class constructor, based on a database row's current data.

By using a factory pattern, programmers will be able to write their own logic for having EF Core instantiate entities.

## Proposed Implementation

I propose to add a new method to `Microsoft.EntityFrameworkCore.DbContextOptionsBuilder`, allowing to provide an alternative implementation for selecting an entity's appropriate constructor.

The proposed `DbContextOptionsBuilder` method may be defined like this:

```c#
public class DbContextOptionsBuilder : IDbContextOptionsBuilderInfrastructure
{
  ...
  public virtual DbContextOptionsBuilder UseConstructorFactory(IConstructorFactory constructorFactory);
}
```

<br/>

The `IConstructorFactory` interface would specify a single method:

```c#
public interface IConstructorFactory
{
  IEntityFactory<TEntity> UseEntityFactory<TEntity>();
}
```

<br/>

The `IEntityFactory<TEntity>` interface, in turn, would return a specific constructor per entity and current data row:

```c#
public interface IEntityFactory<out TEntity>
{
  public ConstructorInfo FindConstructor(DataRow dataRow);
  public TEntity InvokeConstructorAndInitializers(ConstructorInfo constructor, DataRow dataRow);
  public IEnumerable<TEntity> CreateObjects(DataTable dataTable);
}
```

_(The above interface specification is based on `System.Data` for the sake of conciseness.)_

## Sample Solution

This repository is a concise yet comprehensive sample implementation of `IConstructorFactory` and `IEntityFactory<TEntity>`.

It comprises the following .NET Core 8.0 projects:

|Project name|Description|
|-|-|
|ConstructorFactoryInterfaces|Definition of `IConstructorFactory` and `IEntityFactory<TEntity>` interfaces.|
|ConstructorFactory|Sample implementations of `IConstructorFactory` and `IEntityFactory<TEntity>`, always choosing the most specific constructor.|
|ConstructorFactoryTests|Unit tests, testing each of the class' methods.|
|Model|Sample data model used for running the unit tests.|

### Implementation Remarks

The simple sample implementation of `MostSpecificConstructorEntityFactory` does not take data annotations or fluid API configuration for mapping column names to database columns into account.

### How To Run

Download this repository, then compile and run tests using `dotnet test`. The unit tests in this solution are the actual "programs" to run.

You may want to debug through the tests for reproducing the solution:

|Test Class|Description|
|-|-|
|DbContextOptionsBuilderTests|`UseConstructorFactory` sample implementation.|
|FindConstructorTests| Tests `MostSpecificConstructorEntityFactory<TEntity>` `.FindConstructor()`.|
|InvokeConstructorAndInitializersTests|Tests `MostSpecificConstructorEntityFactory<TEntity>` `.InvokeConstructorAndInitializers()`.|
|CreateObjectsTests|Tests `MostSpecificConstructorEntityFactory<TEntity>` `.CreateObjectsTests()`.|