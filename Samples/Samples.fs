module Samples

open Npgsql
open NUnit.Framework

open Vp.FSharp.Sql
open Vp.FSharp.Sql.PostgreSql

module SqlCmd = PostgreSqlCommand

let newSqlConn () =
    new NpgsqlConnection("Host=localhost;Database=northwind;User ID=northwind_user;Password=thewindisblowing")
let print title computation =
    Async.RunSynchronously computation
    |> printfn "%s: %A" title

[<Test; Explicit>]
let ``Simple query`` () =
    use sqlConn = newSqlConn ()
    "select product_name, unit_price from products limit 5"
    |> SqlCmd.text
    |> SqlCmd.queryList sqlConn (fun _ _ read ->
        {| Name  = read.Value<string>  "product_name"
           Price = read.Value<float32> "unit_price" |})
    |> print "result"

[<Test; Explicit>]
let ``Simple scalar`` () =
    use sqlConn = newSqlConn ()
    "select count(*) from products"
    |> SqlCmd.text
    |> SqlCmd.executeScalar<int32> sqlConn
    |> print "count"

[<Test; Explicit>]
let ``Simple parameters`` () =
    use sqlConn = newSqlConn ()
    "select product_name, unit_price from products
    where unit_price <= @price"
    |> SqlCmd.text
    |> SqlCmd.parameters
        [ ("price", Real 5f) ]
    |> SqlCmd.queryList sqlConn (fun _ _ read ->
        {| Name  = read.Value<string>  "product_name"
           Price = read.Value<float32> "unit_price" |})
    |> print "result"

[<Test; Explicit>]
let ``Simple transaction`` () =
    let insert sqlConn transaction =
        "insert into shippers values (99, 'spacex')"
        |> SqlCmd.text
        |> SqlCmd.transaction transaction
        |> SqlCmd.executeNonQuery sqlConn
    use sqlConn = newSqlConn ()
    insert
    |> PostgreSqlTransaction.defaultNotCommit sqlConn
    |> print "affected rows"

[<Test; Explicit>]
let ``Simple transaction scope`` () =
    let insert sqlConn =
        "insert into shippers values (99, 'spacex')"
        |> SqlCmd.text
        |> SqlCmd.executeNonQuery sqlConn
    use sqlConn = newSqlConn ()
    insert
    |> TransactionScope.defaultNotComplete sqlConn
    |> print "affected rows"

[<Test; Explicit>]
let ``Simple unified multiset`` () =
    use sqlConn = newSqlConn ()
    "select product_id  as id, product_name  as name from products   limit 2;
     select category_id as id, category_name as name from categories limit 2;"
    |> SqlCmd.text
    |> SqlCmd.queryList sqlConn (fun setIndex recordIndex read ->
        {| SetIndex    = setIndex
           RecordIndex = recordIndex
           Id   = read.Value<int32>  "id"
           Name = read.Value<string> "name" |})
    |> print "result"

[<Test; Explicit>]
let ``Simple diversified multiset`` () =
    use sqlConn = newSqlConn ()
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
    |> print "results"

[<Test; Explicit>]
let ``Simple logger`` () =
    use sqlConn = newSqlConn ()
    "select count(*) from orders"
    |> SqlCmd.text
    |> SqlCmd.overrideLogger (function
        | ConnectionOpened  conn         -> printfn "Connection %i opened" conn.ProcessID
        | ConnectionClosed (conn, since) -> printfn "Connection closed after %O" since
        | CommandPrepared   cmd          -> printfn "Command prepared: %s" cmd.CommandText
        | CommandExecuted  (cmd, since)  -> printfn "Command executed in %f seconds" since.TotalSeconds)
    |> SqlCmd.executeScalar<int32> sqlConn
    |> print "count"
