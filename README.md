# Vp.FSharp.Sql.PostgreSql

The library providing specific native DB types definition for PostgreSQL and
relying on [`Npsql`](https://www.npgsql.org) and
providing a DB-specific module `PostgreSql`.

## Slagging Hype

We aim at following highly controversial practices to the best of our ability!

Status | Package
------ | ----------------------
OK     | [![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-green.svg)](https://conventionalcommits.org)
OK     | [![semver](https://img.shields.io/badge/semver-2.0.0-green)](https://semver.org/spec/v2.0.0.html)
TBD    | [![keep a changelog](https://img.shields.io/badge/keep%20a%20changelog-1.0.0-red)](https://keepachangelog.com/en/1.0.0)
TBD    | [![Semantic Release](https://img.shields.io/badge/Semantic%20Release-17.1.1-red)](https://semantic-release.gitbook.io/semantic-release)

[Conventional Commits]: https://conventionalcommits.org
[semver]: https://img.shields.io/badge/semver-2.0.0-blue
[Semantic Release]: https://semantic-release.gitbook.io/semantic-release
[keep a changelog]: https://keepachangelog.com/en/1.0.0

## NuGet Package

 Name                       | Version  | Command |
--------------------------- | -------- | ------- |
 `Vp.FSharp.Sql.PostgreSql` | [![NuGet Status](http://img.shields.io/nuget/v/Vp.FSharp.Sql.PostgreSql.svg)](https://www.nuget.org/packages/Vp.FSharp.Sql.PostgreSql) | `Install-Package Vp.FSharp.Sql.PostgreSql`

# How to use this library?

First of all you need to initiate a new connection: `use sqlConn = new NpgsqlConnection(…)`.\
Don't forget to keep always the `use` inside of the async computation expression.

The pivot module is `PostgreSqlCommand`.\
You can create a new alias to have a shorter and more generic name: `module SqlCmd = PostgreSqlCommand`.

### `PostgreSqlCommand.text`

Initialize a command definition with the given text contained in the given string.

`string -> PostgreSqlCommandDefinition`

```fsharp
"select product_name, unit_price from products"
|> SqlCmd.text
```

### `PostgreSqlCommand.queryList`

Return the sets of rows as a list accordingly to the command definition.

`NpgsqlConnection ->
    read: (SetIndex -> RecordIndex -> SqlRecordReader<NpgsqlDataReader> -> 'T) ->
    PostgreSqlCommandDefinition ->
    Async<'T list>`

- `SetIndex: int32`: Index of the set in case of multiple sql statements.
- `RecordIndex: int32`: Index of the row in a set.
- `SqlRecordReader<NpgsqlDataReader>`: Helpers for Npgsql DataReader.

```fsharp
"select product_name, unit_price from products"
|> SqlCmd.text
|> SqlCmd.queryList sqlConn (fun _ _ read ->
    {| Name  = read.Value<string>  "product_name"
       Price = read.Value<decimal> "unit_price" |})
// Output: [ { Name = "Chai"; Price = 18m } ]
```

### `PostgreSqlCommand.executeScalar`

Execute the command accordingly to its definition and,
- return the first cell value, if it is available and of the given type.
- throw an exception, otherwise.

```fsharp
"select count(*) from products"
|> SqlCmd.text
|> SqlCmd.executeScalar<int32> sqlConn
// Output: 77
```

### `PostgreSqlCommand.parameters`

Update the command definition with the given parameters.

```fsharp
"select product_name, unit_price from products
where unit_price <= @price"
|> SqlCmd.text
|> SqlCmd.parameters
    [ ("price", Decimal 5m) ]
|> SqlCmd.queryList sqlConn (fun _ _ read ->
    {| Name  = read.Value<string>  "product_name"
       Price = read.Value<decimal> "unit_price" |})
```

### `PostgreSqlCommand.transaction`

Update the command definition and sets whether the command should be wrapped in the given transaction.

```fsharp
let insert sqlConn transaction =
    "insert into shippers values (99, 'spacex')"
    |> SqlCmd.text
    |> SqlCmd.transaction transaction
    |> SqlCmd.executeNonQuery sqlConn
async {
    use sqlConn = new NpgsqlConnection ()
    return! PostgreSqlTransaction.defaultCommit sqlConn insert
}
```

### `TransactionScope`

```fsharp
let insert sqlConn =
    "insert into shippers values (99, 'spacex')"
    |> SqlCmd.text
    |> SqlCmd.executeNonQuery sqlConn
async {
    use sqlConn = new NpgsqlConnection ()
    return! TransactionScope.defaultComplete sqlConn insert
}
```

### Unified multiple sql statements `PostgreSqlCommand.queryList`

```fsharp
"select product_id  as id, product_name  as name from products   limit 2;
 select category_id as id, category_name as name from categories limit 2;"
|> SqlCmd.text
|> SqlCmd.queryList sqlConn (fun setIndex recordIndex read ->
    {| SetIndex    = setIndex
       RecordIndex = recordIndex
       Id   = read.Value<int32>  "id"
       Name = read.Value<string> "name" |})
```

### Diversified multiple sql statements `PostgreSqlCommand.querySetList`

```fsharp
"select product_name, quantity_per_unit from products   limit 2;
 select category_id,  category_name     from categories limit 2;"
|> SqlCmd.text
|> SqlCmd.querySetList2 sqlConn
    (fun _ read ->
        {| ProductName     = read.Value<string> "product_name"
           QuantityPerUnit = read.Value<string> "quantity_per_unit" |})
    (fun _ read ->
        {| CategoryId   = read.Value<int32>  "category_id"
           CategoryName = read.Value<string> "category_name" |})
```

### `PostgreSqlCommand.overrideLogger`

Update the command definition so that when executing the command, it use the given overriding logger, instead of the default one, aka the Global logger, if any.

```fsharp
"select count(*) from orders"
|> SqlCmd.text
|> SqlCmd.overrideLogger (function
    | ConnectionOpened  conn         -> printfn "Connection %i opened" conn.ProcessID
    | ConnectionClosed (conn, since) -> printfn "Connection closed after %O" since
    | CommandPrepared   cmd          -> printfn "Command prepared: %s" cmd.CommandText
    | CommandExecuted  (cmd, since)  -> printfn "Command executed in %f seconds" since.TotalSeconds)
|> SqlCmd.executeScalar<int32> sqlConn
```

### `SqlConfigurationCache`

A configuration cache holding a single value per set of generic constraints
and giving an access to a snapshot at any given point in time.\
Can serve and act as some sort of global configuration.

Global logger:
```fsharp
SqlConfigurationCache.Logger (function
    | CommandExecuted (_, since) -> printfn "Command executed in %f seconds" since.TotalSeconds
    | _ -> ())
```

# How to Contribute
Bug reports, feature requests, and pull requests are very welcome! Please read the [Contribution Guidelines](./CONTRIBUTION.md) to get started.

# Licensing
The project is licensed under MIT. For more information on the license see the [license file](./LICENSE).
