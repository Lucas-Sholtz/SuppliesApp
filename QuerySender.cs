using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SuppliesApp
{
    static class QuerySender
    {
        private const string DefaultQuery = "SELECT * FROM Supplies";
        private const string GroupByQueryTemplate = "SELECT {{PLACEHOLDER}}, SUM(Count) AS Count, SUM(Sum) AS Sum FROM Supplies GROUP BY {{PLACEHOLDER}}";
        private const string Placeholder= "{{PLACEHOLDER}}";
        private const string ConnectionStringName = "DefaultConnection";
        public static DataTable SendQuery(List<string> columns)
        {
            //если не выбрано ни одной колонки то отправляем запрос по умолчанию
            if (columns.Count == 0)
            {
                return SendDefaultQuery();
            }
            else
            {
                //иначе формируем запрос с группировкой
                return SendGroupByQuery(columns);
            }
        }
        private static DataTable SendDefaultQuery()
        {
            //получить строку подключения, использовать запрос по умолчанию
            string connectionString = GetConnectionString();
            string selectQueryString = DefaultQuery;
            //получить данные с сервера
            return GetDataFromServer(connectionString, selectQueryString);
        }
        private static DataTable SendGroupByQuery(List<string> columns)
        {
            //получить строку подключения, запрос сгенерировать
            string connectionString = GetConnectionString();
            string selectQueryString = GenerateGroupByQuery(columns);
            //получить данные с сервера
            return GetDataFromServer(connectionString, selectQueryString);
        }
        private static string GetConnectionString()
        {
            //по умолчанию строка пуста, если останется такой то будет ошибка
            string connectionString = null;

            //найти строку с соответствующим именем
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[ConnectionStringName];

            //если найдена, вернуть
            if (settings != null)
                connectionString = settings.ConnectionString;

            return connectionString;
        }
        private static string GenerateGroupByQuery(List<string> columns) //генерирует текст запроса в зависимости от выбранных колонок
        {
            string columnNames = "";
            //создать строку с именами колонок через ", "
            columnNames += columns[0];
            for (int i = 1; i < columns.Count; i++)
            {
                columnNames += ", " + columns[i];
            }
            //заменить плейсхолдеры в строках с SELECT и GROUP BY на полученную строку
            string groupByQueryString = GroupByQueryTemplate.Replace(Placeholder, columnNames);

            return groupByQueryString;
        }
        private static DataTable GetDataFromServer(string connectionString, string selectQueryString)
        {
            //cоздать соединение
            using (var connection = new SqlConnection(connectionString))
            {
                //отправить запрос
                using (var adapter = new SqlDataAdapter(selectQueryString, connection))
                {
                    //наполнить таблицу
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }
    }
}
