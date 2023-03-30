[<RequireQualifiedAccess>]
module Vp.FSharp.Sql.PostgreSql.PostgreSqlArrayDbValue

open NpgsqlTypes
open Vp.FSharp.Sql.PostgreSql


let toArray arrayOf =
    let tuple2 a b = (a, b)
    let toNpgDbType =
        function
        | ArrayOf.Uuid -> NpgsqlDbType.Uuid
        | ArrayOf.Integer -> NpgsqlDbType.Integer
        | ArrayOf.Text -> NpgsqlDbType.Text
        | ArrayOf.Varchar -> NpgsqlDbType.Varchar
        | ArrayOf.Boolean -> NpgsqlDbType.Boolean
        | ArrayOf.Char -> NpgsqlDbType.Char
        | ArrayOf.Date -> NpgsqlDbType.Date
        | ArrayOf.Double -> NpgsqlDbType.Double
        | ArrayOf.Money -> NpgsqlDbType.Money
        | ArrayOf.Numeric -> NpgsqlDbType.Numeric
        | ArrayOf.SmallInt -> NpgsqlDbType.Smallint
        | ArrayOf.BigInt -> NpgsqlDbType.Bigint
        | ArrayOf.Bit -> NpgsqlDbType.Bit
        | ArrayOf.Time -> NpgsqlDbType.Time
        | ArrayOf.Timestamp -> NpgsqlDbType.Timestamp
        | ArrayOf.TimestampTz -> NpgsqlDbType.TimestampTz
        | ArrayOf.TimeTz -> NpgsqlDbType.TimeTz
    let dbType = toNpgDbType arrayOf

    List.toArray
    >> (fun v -> v :> obj)
    >> tuple2 (NpgsqlDbType.Array ||| dbType)
    >> Custom
