using CleanArchAcceleratorTools.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;

namespace CleanArchAcceleratorTools.Infrastructure.DataTables
{
    /// <summary>
    /// Helpers to materialize entities into <see cref="DataTable"/> and reorder its columns.
    /// </summary>
    /// <remarks>
    /// Resolves column names via EF Core metadata and includes only writable, non-<see cref="NotMappedAttribute"/> simple properties.
    /// </remarks>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Materializes an entity collection into a <see cref="DataTable"/> using EF Core column mappings and optional column ordering.
        /// </summary>
        /// <typeparam name="T">Entity type deriving from <see cref="Entity"/>.</typeparam>
        /// <param name="data">Entities to materialize.</param>
        /// <param name="columnsOrder">
        /// Optional mapping of property name to target column ordinal; unmapped columns keep their order.
        /// </param>
        /// <param name="dbContext">EF Core <see cref="DbContext"/> used to resolve mappings.</param>
        /// <returns>A <see cref="DataTable"/> named after the entity type with columns matching database column names.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="dbContext"/> has no mapping for the entity.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when a mapped, writable property lacks a database column.</exception>
        public static DataTable ToDataTable<T>(this ICollection<T> data, Dictionary<string, int> columnsOrder, DbContext dbContext) where T : Entity
        {
            var dataTable = new DataTable(typeof(T).Name);
            var order = new Dictionary<(string propName, string colName), int>();
            var entityType = dbContext.Model.FindEntityType(typeof(T));

            if (entityType == null) throw new ArgumentException(DomainMessages.EntityNotConfiguredInDbContext);

            var propertyMappings = entityType.GetProperties()
                                             .ToDictionary(p => p.Name, p => p.GetColumnName(StoreObjectIdentifier.Table(entityType.GetTableName()!)));

            var properties = typeof(T).GetProperties()
                .Where(p => (p.PropertyType.IsPrimitive ||
                            p.PropertyType.IsValueType ||
                            p.PropertyType == typeof(string))
                            && (p.CanWrite &&
                            p.GetCustomAttribute<NotMappedAttribute>() == null));

            foreach (var prop in properties)
            {
                if (propertyMappings.TryGetValue(prop.Name, out var columnName))
                {
                    var column = new DataColumn(columnName, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType)
                    {
                        AllowDBNull = true
                    };
                    dataTable.Columns.Add(column);

                    if (columnsOrder.TryGetValue(prop.Name, out var columnIndex))
                        order.Add((prop.Name!, columnName!), columnIndex);
                }
                else
                {
                    throw new KeyNotFoundException(DomainMessages.PropertyMissingColumnMapping.ToFormat(prop.Name));
                }
            }

            foreach (var item in data)
            {
                var values = new object[properties.Count()];
                int i = 0;

                foreach (var prop in properties)
                {
                    values[i] = prop.GetValue(item) ?? DBNull.Value;
                    i++;
                }

                dataTable.Rows.Add(values);
            }

            dataTable.ReorderDataTableColumns(order);
            return dataTable;
        }

        /// <summary>
        /// Reorders <see cref="DataTable"/> columns according to the provided mapping.
        /// </summary>
        /// <param name="table">DataTable whose columns will be reordered.</param>
        /// <param name="columnOrder">
        /// Mapping of (property name, column name) to desired ordinal.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="columnOrder"/> is null or references a missing column.</exception>
        public static void ReorderDataTableColumns(this DataTable table, Dictionary<(string propName, string colName), int> columnOrder)
        {
            if (columnOrder is null)
                throw new ArgumentException(DomainMessages.ColumnOrderMustBeDefined);

            foreach (var order in columnOrder)
                if (!table.Columns.Contains(order.Key.colName))
                    throw new ArgumentException(DomainMessages.ColumnDoesNotExistInDataTable.ToFormat(order.Key.colName));

            foreach (var order in columnOrder)
                table.Columns[order.Key.colName]!.SetOrdinal(order.Value);
        }
    }
}