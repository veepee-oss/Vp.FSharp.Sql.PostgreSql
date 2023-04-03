module Tests

open NpgsqlTypes
open Vp.FSharp.Sql.PostgreSql
open Xunit


[<Fact>]
let ``My test`` () =
    let dbValue1 = PostgreSqlDbValue.Array<int> (NpgsqlDbType.Integer, [| 324 |])
    let dbValue2 = PostgreSqlDbValue.Array<int> (NpgsqlDbType.Integer, [| "sdf" |])
    Assert.True(true)
