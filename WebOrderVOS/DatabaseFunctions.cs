using System.Data;
using System.Data.SqlClient;

namespace WebOrderVOS
{
    public class DatabaseFunctions
    {
        private IConfiguration configuration;
        public DatabaseFunctions(IConfiguration _config)
        {
            configuration = _config;
        }

        public string querySQL = "SELECT trm.treatment_id, trm.lockedby, techni.fname, techni.lname, trm.ds_status, trm.t_duedate, trm.t_date as 'Date', pt.fname as 'Patient First name', pt.lname as 'Patient last name', cl.name as 'Type of Treatment' From patients pt, clients cl, (Treatment trm LEFT JOIN technicians techni ON techni.lab_id = trm.tech_lab_id AND techni.tech_id = trm.tech_tech_id) WHERE pt.lab_id = trm.patient_lab_id AND pt.practice_id = trm.patient_practice_id AND pt.patient_id = trm.patient_patient_id AND cl.lab_id = trm.practice_lab_id AND cl.practice_id = trm.practice_pr_id";

        private SqlConnection ConnectDB()
        {
            string DBSERVER = configuration["SQLConnectionInfo:DatabaseServer"];

            string DBName = configuration["SQLConnectionInfo:DatabaseName"];

            string UserName = configuration["SQLConnectionInfo:DatabaseUserName"];

            string Password = configuration["SQLConnectionInfo:DatabasePassword"];

            return new SqlConnection("Data Source=" + DBSERVER + ";Initial Catalog=" + DBName + ";User Id=" + UserName + ";Password=" + Password + ";");
        }

        public bool CheckDatabase(out string er)
        {
            try
            {
                SqlConnection connection = ConnectDB();
                connection.Open();
                connection.Close();
                er = "";
                return true;
            }
            catch (Exception error)
            {
                er = error.Message;
                return false;
            }
        }

        public DataTable ExQuery(string query)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                SqlCommand command = new(query, connection);
                SqlDataReader dataReader = command.ExecuteReader();
                data.Load(dataReader);
                dataReader.Dispose();
                connection.Close();
            }
            return data;
        }

        public void ExNonQuery(string query)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                SqlCommand command = new(query, connection);
                _ = command.ExecuteNonQuery();
                connection.Close();
                command.Dispose();
            }
        }
    }
}
