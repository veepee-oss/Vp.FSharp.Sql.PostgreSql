namespace Vp.FSharp.Sql.PostgreSql

open System
open System.Data
open System.Net
open System.Threading
open System.Collections.Generic
open System.Net.NetworkInformation

open Npgsql
open NpgsqlTypes

open Vp.FSharp.Sql


/// Native PostgreSQL DB types.
/// See https://www.npgsql.org/doc/types/basic.html
/// and https://stackoverflow.com/a/845472/4636721
type PostgreSqlDbValue =
    | Null
    | Bit of bool
    | Boolean of bool
    | SmallInt of int16
    | Integer of int32
    | Oid of uint32
    | Xid of uint32
    | Cid of uint32
    | BigInt of int64

    | Real of single
    | Double of double

    | Money of decimal
    | Numeric of decimal

    | ByteA of uint8 array
    | OidVector of uint32 array

    | Uuid of Guid

    | INet of IPAddress
    | MacAddr of PhysicalAddress

    | TsQuery of NpgsqlTsQuery
    | TsVector of NpgsqlTsVector

    | Point of NpgsqlPoint
    | LSeg of NpgsqlLSeg
    | Path of NpgsqlPath
    | Polygon of NpgsqlPolygon
    | Line of NpgsqlLine
    | Circle of NpgsqlCircle
    | Box of NpgsqlBox

    | HStore of Dictionary<string, string>

    | Date of DateTime
    | Interval of TimeSpan
    | Time of DateTime
    | TimeTz of DateTime
    | Timestamp of DateTime
    | TimestampTz of DateTimeOffset

    | InternalChar of uint8

    | Char of string
    | VarChar of string

    | Name of string
    | CiText of string
    | Text of string
    | Xml of string
    | Json of string
    | Jsonb of string

    /// Only if the relevant Npgsql mapping for the Enum has been set up beforehand.
    /// See: https://www.npgsql.org/doc/types/enums_and_composites.html
    | Enum of Enum

type PostgreSqlCommandDefinition =
    CommandDefinition<
        NpgsqlConnection,
        NpgsqlCommand,
        NpgsqlParameter,
        NpgsqlDataReader,
        NpgsqlTransaction,
        PostgreSqlDbValue>

type PostgreSqlConfiguration =
    SqlConfigurationCache<
        NpgsqlConnection,
        NpgsqlCommand>

type PostgreSqlDependencies =
    SqlDependencies<
        NpgsqlConnection,
        NpgsqlCommand,
        NpgsqlParameter,
        NpgsqlDataReader,
        NpgsqlTransaction,
        PostgreSqlDbValue>


[<AbstractClass; Sealed>]
type internal Constants private () =

    static member DbValueToParameter name value =
        let parameter = NpgsqlParameter()
        parameter.ParameterName <- name
        match value with
        | Null ->
            parameter.Value <- DBNull.Value
        | Bit value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bit
        | Boolean value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Boolean
        | SmallInt value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Smallint
        | Integer value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Integer
        | Oid value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Oid
        | Xid value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Xid
        | Cid value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Cid
        | BigInt value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bigint

        | Real value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bigint
        | Double value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bigint

        | Money value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Money
        | Numeric value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Numeric

        | ByteA value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bytea
        | OidVector value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Oidvector

        | Uuid value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Uuid

        | INet value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Inet
        | MacAddr value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.MacAddr

        | TsQuery value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.TsQuery
        | TsVector value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.TsVector

        | Point value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Point
        | LSeg value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.LSeg
        | Path value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Path
        | Polygon value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Polygon
        | Line value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Line
        | Circle value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Circle
        | Box value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Box

        | HStore value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Hstore

        | Date value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Date
        | Interval value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Interval
        | Time value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Time
        | TimeTz value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.TimeTz
        | Timestamp value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Timestamp
        | TimestampTz value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.TimestampTz

        | InternalChar value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.InternalChar

        | Char value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Char
        | VarChar value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Varchar

        | Name value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Name
        | CiText value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Citext
        | Text value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Text
        | Xml value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Xml
        | Json value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Json
        | Jsonb value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Jsonb
        | Enum value ->
            parameter.Value <- value
        parameter

    static member Deps : PostgreSqlDependencies =
        let beginTransactionAsync
            (connection: NpgsqlConnection) (isolationLevel: IsolationLevel) (cancellationToken: CancellationToken) =
            connection.BeginTransactionAsync(isolationLevel, cancellationToken)

        { CreateCommand = fun connection -> connection.CreateCommand()
          SetCommandTransaction = fun command transaction -> command.Transaction <- transaction
          BeginTransactionAsync = beginTransactionAsync
          ExecuteReaderAsync = fun command -> command.ExecuteReaderAsync
          DbValueToParameter = Constants.DbValueToParameter }
