using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XPlus;
using System.Linq;
using AlmCore.SQLModel;
using XExten.XCore;

namespace AlmCore
{
    public class SQLContext
    {
        public static SqlSugarClient Lite => Instance();

        public static SqlSugarClient Instance()
        {
            Extension.CreateFile(Extension.Connection);
            ConnectionConfig Config = new ConnectionConfig
            {
                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.Attribute,
                ConnectionString = $"DataSource={Extension.Connection}",
                IsAutoCloseConnection = true,
            };
            SqlSugarClient Db = new SqlSugarClient(Config);
            InitModel(Db);
            return Db;
        }
        private static void InitModel(SqlSugarClient Db)
        {
            if (InitDataBase())
            {
                var Types = XPlusEx.XAssembly(typeof(SQLContext).Assembly.GetName().Name).SelectMany(t => t.ExportedTypes.Where(x => x.GetInterfaces().Contains(typeof(ISQLModel)))).ToArray();
                Db.CodeFirst.InitTables(Types);
            }
        }

        private static bool InitDataBase()
        {
            Extension.CreateFile(Extension.InitDataBase);
            var Content = Extension.ReadContent(Extension.InitDataBase);
            if (Content.IsNullOrEmpty())
            {
                Extension.WriteContent(Extension.InitDataBase, $"{true}_{DateTime.Now.ToFmtDate()}".ToLzStringEnc());
                return true;
            }
            else
                return Convert.ToBoolean(Content.ToLzStringDec());
        }
    }
}
