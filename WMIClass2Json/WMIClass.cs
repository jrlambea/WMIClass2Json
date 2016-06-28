using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Management;
using System.Web.Script.Serialization;

namespace WMIClass2Json
{
    class WMIClass
    {
        private string _name = "";
        private Dictionary<string, string> _fields = new Dictionary<string, string>();

        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        public Dictionary<string, string> fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        public WMIClass() { }
        public WMIClass(string ClassName)
        {
            _name = ClassName;
        }
        public WMIClass(string ComputerName, string ClassName)
        {
            _name = ClassName;
            GetStruct(ComputerName);
        }
        public void GetStruct(string ComputerName)
        {
            _fields.Clear();

            // Connect with the WMI service and fetch the select WQL
            ManagementScope scope = new ManagementScope("\\\\" + ComputerName + "\\root\\cimv2");
            scope.Connect();
            SelectQuery sq = new SelectQuery("SELECT * FROM " + _name);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, sq);
            foreach (ManagementObject mo in searcher.Get())
            {
                foreach (PropertyData p in mo.Properties)
                {
                    _fields.Add(p.Name, p.Type.ToString());
                }
                break;
            }
        }

        private string convertWMItoSQLType(string WMIType)
        {
            switch (WMIType)
            {
                case "Boolean":
                    return "bit";
                case "DateTime":
                    return "datetime";
                case "SInt16":
                    return "smallint";
                case "String":
                    return "nvarchar(max)";
                case "UInt8":
                    return "tinyint";
                case "UInt32":
                    return "int";
                case "UInt64":
                    return "bigint";
                case "UInt16":
                    return "smallint";
                default:
                    throw new Exception("The WMI type " + WMIType + "does not exist.");
            }
        }

        /** Test if the table exist
         * If the table exist then true
         * If the table diesnt exist then false
         * If the table exist but the fields are different then null
         */
        private bool existInDB(string ConnectionString)
        {
            // Select to know if a table exists
            string SQLScript = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=@tablename";

            // Prepare SQL connection
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(SQLScript, sqlConnection);
            cmd.Parameters.AddWithValue("@tablename", System.Data.SqlDbType.NChar);
            cmd.Parameters["@tablename"].Value = _name;

            // Execute SQL script
            sqlConnection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            // If the query above results 1 then the table exist and begin to test the field names
            // If not return false
            if (dr.GetInt32(0) != 1)
            {
                sqlConnection.Close();
                return false;
            }

            // Succeed
            sqlConnection.Close();
            return true;
        }
        public int writeIntoDB(string ConnectionString)
        {
            try
            {
                // If the table exist not try to create the new table
                if (existInDB(ConnectionString) == true) { return -1; }

                // SQL script construction
                string SQLScript = "CREATE TABLE " + _name + "([ComputerName] nchar(50) NOT NULL, [Date] datetime NULL, ";
                foreach (string K in _fields.Keys)
                {
                    SQLScript += "[" + K + "] " + convertWMItoSQLType(_fields[K]) + " NULL, ";
                }
                SQLScript = SQLScript.Substring(0, SQLScript.Length - 2) + ");";

                // Prepare SQL connection
                SqlConnection sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand(SQLScript, sqlConnection);

                // Execute SQL script
                sqlConnection.Open();
                int result = cmd.ExecuteNonQuery();
                sqlConnection.Close();
                return result;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Serialize()
        {
            return new JavaScriptSerializer().Serialize(this);
        }
    }
}
