using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using System.Text.RegularExpressions;
using System.IO;
using FirebirdSql.Data.Services;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO.Compression;

namespace dataPump.det
{
    public partial class F99 : Form
    {
        delegate void SetTextCallback(int val_, string text);
        delegate void SetImageCallback(int val_, string text);
        #region Temp
        int skip_ = 0;
        bool is_may_close = true;

        //для прогрессбар 1
        int p_cur = 0;
        string p_text = "";
        int p_cur_2 = 0;
        string p_text_2 = "";
        //data
        bool is_close_analize = false;
        #endregion
        #region Соединения
        FbConnectionStringBuilder fc_old = new FbConnectionStringBuilder();
        FbConnectionStringBuilder fc_new = new FbConnectionStringBuilder();
        string temp_folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                            + @"\det\temp\Setup\DB\"
                            + DateTime.Now.ToString("yyyyMMddHHmm");
        string database_new = "";
        #endregion        
        #region Ошибки
        StringBuilder sb = new StringBuilder();
        #endregion
        #region Вспомогательные функции
        string get_field_type(string FTYPE, string FLEN, string FSCALE, string FSUBTYPE, string FSEGMENTSIZE, string FPRECISION, bool is_func)
        {
            //для функций есть ограничения для BLOB - их нужно просто так выводить
            string FIELD_SOURCE = "";
            if (FTYPE == "261")
            {
                FIELD_SOURCE = "BLOB";
            }
            else
            {
                try
                {
                    FIELD_SOURCE = get_field_type(FTYPE,
                                                        FLEN,
                                                        FSCALE,
                                                        FSUBTYPE,
                                                        FSEGMENTSIZE,
                                                        FPRECISION);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return FIELD_SOURCE;
        }
        string get_field_type(string FTYPE, string FLEN, string FSCALE, string FSUBTYPE, string FSEGMENTSIZE, string FPRECISION)
        {
            string FIELD_SOURCE = "";
            if (FPRECISION == string.Empty)
            {
                FPRECISION = "0";
            }
            if (FSUBTYPE == string.Empty)
            {
                FSUBTYPE = "0";
            }
            if (FSEGMENTSIZE == string.Empty)
            {
                FSEGMENTSIZE = "0";
            }
            try
            {
                FIELD_SOURCE = get_field_type(Convert.ToInt32(FTYPE),
                                                        Convert.ToInt32(FLEN),
                                                        Convert.ToInt32(FSCALE),
                                                        Convert.ToInt32(FSUBTYPE),
                                                        Convert.ToInt32(FSEGMENTSIZE),
                                                        Convert.ToInt32(FPRECISION));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return FIELD_SOURCE;
        }
        string get_field_type(int FTYPE, int FLEN, int FSCALE, int FSUBTYPE, int FSEGMENTSIZE, int FPRECISION)
        {
            string FIELD_SOURCE = "";
            int FCHARLEN = 0;
            if (FCHARLEN == 0)
            {
                FCHARLEN = FLEN;
            }

            if (FTYPE == 261)
            {
                FIELD_SOURCE = "BLOB SUB_TYPE " + FSUBTYPE.ToString() + " SEGMENT SIZE " + FSEGMENTSIZE.ToString();
            }
            else
                if (FTYPE == 14)
                {
                    FIELD_SOURCE = "CHAR(" + FCHARLEN + ")";
                }
                else
                    if (FTYPE == 37)
                    {
                        FIELD_SOURCE = "VARCHAR(" + FCHARLEN + ")";
                    }
                    else
                        if (FTYPE == 12)
                        {
                            FIELD_SOURCE = "DATE";
                        }
                        else
                            if (FTYPE == 13)
                            {
                                FIELD_SOURCE = "TIME";
                            }
                            else
                                if (FTYPE == 35)
                                {
                                    FIELD_SOURCE = "TIMESTAMP";
                                }
                                else
                                    if (FTYPE == 7)
                                    {
                                        if ((FSCALE < 0) || (FSUBTYPE == 1) || (FSUBTYPE == 2))
                                        {
                                            if (FSUBTYPE == 2)
                                            {
                                                FIELD_SOURCE = "DECIMAL";
                                            }
                                            else
                                            {
                                                FIELD_SOURCE = "NUMERIC";
                                            }
                                            if (FPRECISION > 0)
                                            {
                                                FIELD_SOURCE = FIELD_SOURCE + "(" + FPRECISION + "," + (FSCALE * -1) + ")";
                                            }
                                            else
                                            {
                                                FIELD_SOURCE = FIELD_SOURCE + "(4," + (FSCALE * -1) + ")";
                                            }
                                        }
                                        else
                                            FIELD_SOURCE = "SMALLINT";
                                    }
                                    else
                                        if (FTYPE == 8)
                                        {
                                            if ((FSCALE < 0) || (FSUBTYPE == 1) || (FSUBTYPE == 2))
                                            {
                                                if (FSUBTYPE == 2)
                                                {
                                                    FIELD_SOURCE = "DECIMAL";
                                                }
                                                else
                                                {
                                                    FIELD_SOURCE = "NUMERIC";
                                                }
                                                if (FPRECISION > 0)
                                                {
                                                    FIELD_SOURCE = FIELD_SOURCE + "(" + FPRECISION + "," + (FSCALE * -1) + ")";
                                                }
                                                else
                                                    FIELD_SOURCE = FIELD_SOURCE + "(9," + (FSCALE * -1) + ")";
                                            }
                                            else
                                            {
                                                FIELD_SOURCE = "INTEGER";
                                            }
                                        }
                                        else
                                            if (FTYPE == 27)
                                            {
                                                if ((FSCALE < 0) || (FSUBTYPE == 1) || (FSUBTYPE == 2))
                                                {
                                                    if (FSUBTYPE == 2)
                                                    {
                                                        FIELD_SOURCE = "DECIMAL";
                                                    }
                                                    else
                                                    {
                                                        FIELD_SOURCE = "NUMERIC";
                                                    }
                                                    if (FPRECISION > 0)
                                                    {
                                                        FIELD_SOURCE = FIELD_SOURCE + "(" + FPRECISION + "," + (FSCALE * -1) + ")";
                                                    }
                                                    else
                                                    {
                                                        FIELD_SOURCE = FIELD_SOURCE + "(9," + (FSCALE * -1) + ")";
                                                    }
                                                }
                                                else
                                                    FIELD_SOURCE = "DOUBLE PRECISION";
                                            }
                                            else
                                                if (FTYPE == 16)
                                                {
                                                    if ((FSCALE < 0) || (FSUBTYPE == 1) || (FSUBTYPE == 2))
                                                    {
                                                        if (FSUBTYPE == 2)
                                                        {
                                                            FIELD_SOURCE = "DECIMAL";
                                                        }
                                                        else
                                                        {
                                                            FIELD_SOURCE = "NUMERIC";
                                                        }
                                                        if (FPRECISION > 0)
                                                        {
                                                            FIELD_SOURCE = FIELD_SOURCE + "(" + FPRECISION + "," + (FSCALE * -1) + ")";
                                                        }
                                                        else
                                                        {
                                                            FIELD_SOURCE = FIELD_SOURCE + "(18," + (FSCALE * -1) + ")";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FIELD_SOURCE = "BIGINT";
                                                    }
                                                }
                                                else
                                                    if (FTYPE == 10)
                                                    {
                                                        FIELD_SOURCE = "FLOAT";
                                                    }
                                                    else
                                                        if (FTYPE == 40)
                                                        {
                                                            FIELD_SOURCE = "CSTRING(" + FLEN.ToString() + ")";
                                                        }

            return FIELD_SOURCE;
        }

        bool is_en_string(string st)
        {
            //проверяет - состоит ли строка только из English
            for (int i = 0; i <= st.Length - 1; i++) {
                if (!list_en.Contains(st[i])) return false;
            }
            // На самом деле в данном случае можно обойтись одной регуляркой, а именно:
            //return Regex.IsMatch(st, @"^\w+$");

            return true;
        }

        bool is_reserv(string st)
        {
            //зарезервированное слово?
            return list_reserv.Contains(st.Trim());
        }
        private void SetText(int val_, string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (this.progressBar1.InvokeRequired)
            {
                SetTextCallback d = SetText;
                this.Invoke(d, new object[] { val_, text });
            }
            else
            {
                this.progressBar1.Value = val_;
                this.l_.Text = text;
            }
        }
        private void SetImage(int val_, string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (this.treeView1.InvokeRequired)
            {
                SetImageCallback d = SetImage;
                this.Invoke(d, new object[] { val_, text });
            }
            else
            {
                if (text.StartsWith("Node3_"))
                {
                    this.treeView1.Nodes["Node3"].Nodes[text].ImageIndex = val_;
                }
                else
                {
                    this.treeView1.Nodes[text].ImageIndex = val_;
                }
            }
        }
        public string get_install(string programm_)
        {
            string install_dir = "";
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            string[] skeys = key.GetSubKeyNames(); // Все подключи из key
            int length = skeys.Length;
            // Проход по всем подключам
            for (int i = 0; i < length; i++)
            {
                // Получаем очередной подключ
                RegistryKey appKey = key.OpenSubKey(skeys[i]);
                string name;
                string install_;
                try // Пробуем получить значение DisplayName
                {
                    name = appKey.GetValue("DisplayName").ToString();
                    install_ = appKey.GetValue("InstallLocation").ToString();

                }
                catch (Exception)
                {
                    // Если не указано имя, то пропускаем ключ
                    continue;
                }

                if (name.ToUpper().StartsWith(programm_.ToUpper()))
                {
                    install_dir = install_;
                }
                appKey.Close();
            }
            key.Close();
            return install_dir;
        }
        public void copy_()
        {
            string from_ = Sett.Default.database_original; string to_ = Sett.Default.database_tmp;
            try
            {
                byte[] buffer = new byte[4096 * 4096]; // 1MB buffer
                bool cancelFlag = false;

                using (FileStream source = new FileStream(from_, FileMode.Open, FileAccess.Read))
                {
                    long fileLength = source.Length;
                    using (FileStream dest = new FileStream(to_, FileMode.Create, FileAccess.Write))
                    {
                        long totalBytes = 0;
                        int currentBlockSize = 0;

                        while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            totalBytes += currentBlockSize;
                            double persentage = totalBytes / (double)fileLength * 100.0;

                            dest.Write(buffer, 0, currentBlockSize);

                            cancelFlag = false;
                            p_cur = (int)((float)totalBytes / (float)fileLength * 100);
                            p_text = "Заменяем файл " + p_cur + @"%";

                            if (totalBytes >= fileLength)
                            {
                                cancelFlag = true;
                            }
                            if (cancelFlag == true)
                            {
                                // Delete dest file here
                                dest.Flush();
                                dest.Close();
                                source.Flush();
                                source.Close();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                sb.AppendLine("error COPY");
                sb.AppendLine(ex.Message);              
            }
        }
        public bool try_connection(string database)
        {
            bool yes_ = true;
            //в любом случае копируем базу!
            //возможно база находится на другом компутере
            //попробуем ее скопировать            
            //проверка соединения
                FbConnectionStringBuilder fc_ch = new FbConnectionStringBuilder();
                try
                {
                    fc_ch.Database = Sett.Default.database_tmp ;//база, которую нужно конвертировать                
                    fc_ch.Pooling = false; //пул соединения - отсутствует - для более быстрого освобождения базы
                    fc_ch.ServerType = FbServerType.Embedded;//встроенный сервер
                    fc_ch.ClientLibrary = ".\\fbembed.dll";
                    //fc_old.Charset = "win1251"; //кодировка для FB 1/5 не указывается - здесь нужно было переводить в форматы UTF
                    fc_ch.UserID = "sysdba";//пользователь по умолчанию
                    fc_ch.Password = "masterkey"; //Пароль можно не указывать
                }
                catch (Exception ex)
                {
                    yes_ = false;
                    MessageBox.Show(ex.Message, "Неверные параметры подключения к базе", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (yes_)
                {
                    using (FbConnection fb = new FbConnection(fc_ch.ConnectionString))
                    {
                        try
                        {
                            fb.Open();
                        }
                        catch (FbException ex)
                        {
                            yes_ = false;
                            MessageBox.Show(ex.Message, "Ошибка при подключения к базе", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            fb.Close();
                        }
                        fb.Dispose();
                    }
                }
            return yes_;
        }
        public bool check_sysdba(string pass_)
        {
            bool yes_ = true;
            //проверим - установлен ли сервер FB
            string dir_ = get_install("Firebird 2.5");
            if (dir_ != "")
            {

                FbConnectionStringBuilder fc_ch = new FbConnectionStringBuilder();
                fc_ch.Database = temp_folder + @"\det.fdb";//база, которую нужно конвертировать                
                fc_ch.Pooling = false; //пул соединения - отсутствует - для более быстрого освобождения базы                   
                //fc_old.Charset = "win1251"; //кодировка для FB 1/5 не указывается - здесь нужно было переводить в форматы UTF
                fc_ch.UserID = "sysdba";//пользователь по умолчанию
                fc_ch.Password = pass_; //Пароль можно не указывать
                try
                {
                    if (!Directory.Exists(temp_folder))
                    {
                        Directory.CreateDirectory(temp_folder);
                    }
                    FbConnection.CreateDatabase(fc_ch.ConnectionString, true);
                }
                catch (FbException ex)
                {
                    MessageBox.Show(ex.Message, "Неверный пароль SYSDBA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    yes_ = false;
                }
            }
            return yes_;
        }
        public void update_param(string pass_)
        {
            using (FbConnection fb = new FbConnection(fc_new.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand("select RF.RDB$FIELD_NAME "+
                                                              "  from RDB$RELATION_FIELDS RF "+
                                                              "  where RF.RDB$RELATION_NAME = 'PARAMS' and "+
                                                              "        RF.RDB$FIELD_NAME in ('PARAM_VALUE_DEF') ",fb,ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    using (FbCommand fcon_p = new FbCommand("update PARAMS P "+
                                                                            " set P.PARAM_VALUE_DEF = @a " +
                                                                            " where (P.MNEMO = 'SYSDBAPASS')",fb,ft))
                                    {
                                        fcon_p.Parameters.Add("@a", FbDbType.VarChar, 8);
                                        fcon_p.Parameters[0].Value = pass_;

                                        fcon_p.ExecuteNonQuery();

                                        fcon_p.Dispose();
                                    }
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("*****Ошибка при смене параметра пароль SYSDBA*****");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine("**********");
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        public void Compress(FileInfo fileToCompress)
        {
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                            Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                                fileToCompress.Name, fileToCompress.Length.ToString(), compressedFileStream.Length.ToString());
                        }
                    }
                }
            }
        }
        #endregion
        #region Наборы
        //символы
        char[] list_en = {'A','B','C','D','E','F','G','H','I','J','K','L','M'
                             ,'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
                             ,'1','2','3','4','5','6','7','8','9'
        					 ,'_'};
        string[] list_reserv = {"ABSOLUTE","ACTION","ABORT","ACTIVE","ADD","AFTER","ALL","ALLOCATE","ALTER","ANALYZE","AND","ANY","ARE","AS","ASC","ASCENDING","ASSERTION","AT","AUTHORIZATION","AUTO","AUTO_INCREMENT","AUTOINC","AVG",
                                "BACKUP","BEFORE","BEGIN","BETWEEN","BIGINT","BINARY","BIT","BLOB","BOOLEAN","BOTH","BREAK","BROWSE","BULK","BY","BYTES",
                                "CACHE","CALL","CASCADE","CASCADED","CASE","CAST","CATALOG","CHANGE","CHAR","CHARACTER","CHARACTER_LENGTH","CHECK","CHECKPOINT","CLOSE","CLUSTER","CLUSTERED","COALESCE","COLLATE","COLUMN","COLUMNS","COMMENT","COMMIT","COMMITTED","COMPUTE","COMPUTED","CONDITIONAL","CONFIRM","CONNECT","CONNECTION","CONSTRAINT","CONSTRAINTS","CONTAINING","CONTAINS","CONTAINSTABLE","CONTINUE","CONTROLROW","CONVERT","COPY","COUNT","CREATE","CROSS","CSTRING","CUBE","CURRENT","CURRENT_DATE","CURRENTJTIME","CURRENT_TIMESTAMP","CURRENT_USER","CURSOR",
                                "DATABASE","DATABASES","DATE","DATETIME","DAY","DBCC","DEALLOCATE","DECIMAL","DEBUG","DECLARE","DEC","DEFAULT","DELETE","DENY","DESC","DESCENDING","DISK","DIV","DESCRIBE","DISTINCT","DO","DISCONNECT","DISTRIBUTED","DOMAIN","DOUBLE","DROP","DUMMY","DUMP",
                                "ELSE","ELSEIF","ENCLOSED","END","ERRLVL","ERROREXIT","ESCAPE","ESCAPED","EXCEPT","EXCEPTION","EXEC","EXECUTE","EXISTS","EXIT","EXPLAIN","EXTEND","EXTERNAL","EXTRACT",
                                "FALSE","FETCH","FIELD","FIELDS","FILE","FILLFACTOR","FILTER","FLOAT","FLOPPY","FOR","FORCE","FOREIGN","FOUND","FREETEXT","FREETEXTTABLE","FROM","FULL","FUNCTION",
                                "GENERATOR","GET","GLOBAL","GO","GOTO","GRANT","GROUP","HAVING",
                                "HOLDLOCK","HOUR",
                                "IDENTITY","IF","IN","INACTIVE","INDEX","INDICATOR","INFILE","INNER","INOUT","INPUT","INSENSITIVE","INSERT","INT","INTEGER","INTERSECT","INTERVAL","INTO","IS","ISOLATION",
                                "JOIN",
                                "KEY","KILL",
                                "LANGUAGE","LAST","LEADING","LEFT","LENGTH","LEVEL","LIKE","LIMIT","LINENO","LINES","LISTEN","LOAD","LOCAL","LOCK","LOGFILE","LONG","LOWER",
                                "MANUAL","MATCH","MAX","MERGE","MESSAGE","MIN","MINUTE","MIRROREXIT","MODULE","MONEY","MONTH","MOVE",
                                "NAMES","NATIONAL","NATURAL","NCHAR","NEXT","NEW","NO","NOCHECK","NONCLUSTERED","NONE","NOT","NULL","NULLIF","NUMERIC",
                                "OF","OFF","OFFSET","OFFSETS","ON","ONCE","ONLY","OPEN","OPTION","OR","ORDER","OUTER","OUTPUT","OVER","OVERFLOW","OVERLAPS",
                                "PAD","PAGE","PAGES","PARAMETER","PARTIAL","PASSWORD","PERCENT","PERM","PERMANENT","PIPE","PLAN","POSITION","PRECISION","PREPARE","PRIMARY","PRINT","PRIOR","PRIVILEGES","PROC","PROCEDURE","PROCESSEXIT","PROTECTED","PUBLIC","PURGE",
                                "RAISERROR","READ","READTEXT","REAL","REFERENCES","REGEXP","RELATIVE","RENAME","REPEAT","REPLACE","REPLICATION","REQUIRE","RESERV","RESERVING","RESET","RESTORE","RESTRICT","RETAIN","RETURN","RETURNS","REVOKE","RIGHT","ROLLBACK","ROLLUP","ROWCOUNT","RULE",
                                "SAVE","SAVEPOINT","SCHEMA","SECOND","SECTION","SEGMENT","SELECT","SENSITIVE","SEPARATOR","SEQUENCE","SESSION_USER","SET","SETUSER","SHADOW","SHARED","SHOW","SHUTDOWN","SINGULAR","SIZE","SMALLINT","SNAPSHOT","SOME","SORT","SPACE","SQL","SQLCODE","SQLERROR","STABILITY","STARTING","STARTS","STATISTICS","SUBSTRING","SUM","SUSPEND",
                                "TABLE","TABLES","TAPE","TEMP","TEMPORARY","THEN","TO","TRAN","TRIGGER","TRUNCATE",
                                "UNIQUE","UPDATETEXT","USE",
                                "VALUE","VARIABLE","VIEW",
                                "WAITFOR","WHILE","WRITE",
                                "YEAR","TEXT","TIME","TOP","TRANSACTION","TRIM","UNCOMMITTED","UNTIL","UPPER","USER","VALUES","VARYING","VOLUME",
                                "WHEN","WITH","WRITETEXT","ZONE","TEXTSIZE","TIMESTAMP","TRAILING","TRANSLATE","TRUE","UNION","UPDATE","USAGE","USING","VARCHAR",
                                "VERBOSE","WAIT","WHERE","WORK","XOR"};
        #endregion
        
        #region Список комманд
        List<string> com = new List<string> { };//первые комманды - без триггеров
        List<string> com2 = new List<string> { };//наборы данных
        List<string> com3 = new List<string> { };//завершающий набор

        private Queue<string> q_data = new Queue<string> { };

        List<string> com_udf = new List<string> { };
        List<string> com_domains = new List<string> { };
        List<string> com_generators = new List<string> { };
        List<string> com_exceptions = new List<string> { };
        List<string> com_roles = new List<string> { };
        List<string> com_tables = new List<string> { };
        List<string> com_views = new List<string> { };
        List<string> com_procedures_prototype = new List<string> { };
        List<string> com_triggers_prototype = new List<string> { };
        List<string> com_grants = new List<string> { };
        List<string> com_procedures_content = new List<string> { };
        List<string> com_triggers_content = new List<string> { };
        List<string> com_primary_key = new List<string> { };
        List<string> com_foreign_key = new List<string> { };
        List<string> com_indexes = new List<string> { };
        #endregion
        #region Список таблиц
        List<string> tables_ = new List<string> { };
        string table_name = "";
        #endregion
        #region SQL
        string sql_udf = "select f.RDB$FUNCTION_NAME, " +
                        "  f.RDB$FUNCTION_TYPE, " +
                        "  f.RDB$QUERY_NAME, " +
                        "  f.RDB$DESCRIPTION, " +
                        "  f.RDB$MODULE_NAME, " +
                        "  f.RDB$ENTRYPOINT, " +
                        "  f.RDB$RETURN_ARGUMENT, " +
                        "  f.RDB$SYSTEM_FLAG from rdb$functions f  " +
                        "  order by 1";
        string sql_udf_a = "select fa.rdb$argument_position, " + //0
                        "   fa.rdb$mechanism, " +             //1 
                        "   fa.rdb$field_type, " +            //2 
                        "   fa.rdb$field_scale, " +           //3 
                        "   fa.rdb$field_length, " +          //4 
                        "   fa.rdb$field_sub_type, " +        //5 
                        "   c.rdb$bytes_per_character, " +    //6 
                        "   c.rdb$character_set_name " +      //7 
                        "   ,fa.rdb$field_precision " +       //8 
                        "   from rdb$functions f " +
                        "   left join rdb$function_arguments fa on f.rdb$function_name = fa.rdb$function_name " +
                        "   left join rdb$character_sets c on fa.rdb$character_set_id = c.rdb$character_set_id " +
                        "   where (f.rdb$function_name = @a) " + //здесь есть параметр @a в него подставляется название функции
                        "   order by fa.rdb$argument_position ";
        string sql_domains = " select f.rdb$FIELD_NAME, " +
                            "        f.rdb$QUERY_NAME, " +
                            "        f.rdb$VALIDATION_BLR, " +
                            "        f.rdb$VALIDATION_SOURCE, " +
                            "        f.rdb$COMPUTED_BLR, " +
                            "        f.rdb$COMPUTED_SOURCE, " +
                            "        f.rdb$DEFAULT_VALUE, " +
                            "        f.rdb$DEFAULT_SOURCE, " +
                            "        f.rdb$FIELD_LENGTH, " +
                            "        f.rdb$FIELD_SCALE, " +
                            "        f.rdb$FIELD_TYPE, " +
                            "        f.rdb$FIELD_SUB_TYPE, " +
                            "        f.rdb$MISSING_VALUE, " +
                            "        f.rdb$MISSING_SOURCE, " +
                            "        f.rdb$DESCRIPTION, " +
                            "        f.rdb$SYSTEM_FLAG, " +
                            "        f.rdb$QUERY_HEADER, " +
                            "        f.rdb$SEGMENT_LENGTH, " +
                            "        f.rdb$EDIT_STRING, " +
                            "        f.rdb$EXTERNAL_LENGTH, " +
                            "        f.rdb$EXTERNAL_SCALE, " +
                            "        f.rdb$EXTERNAL_TYPE, " +
                            "        f.rdb$DIMENSIONS, " +
                            "        f.rdb$NULL_FLAG, " +
                            "        f.rdb$CHARACTER_LENGTH, " +
                            "        f.rdb$COLLATION_ID, " +
                            "        f.rdb$CHARACTER_SET_ID, " +
                            "        f.rdb$FIELD_PRECISION, " +
                            "        ch.rdb$character_set_name, " +
                            "        'COLLATE ' || cl.rdb$collation_name " +
                            " from RDB$FIELDS F " +
                            " left join rdb$character_sets ch on f.rdb$character_set_id = ch.rdb$character_set_id " +
                            " left join rdb$collations cl on f.rdb$collation_id = cl.rdb$collation_id and f.rdb$character_set_id = cl.rdb$character_set_id " +
                            " where F.RDB$FIELD_NAME not like 'RDB$%' ";
        string sql_GENERATORS = " select G.RDB$GENERATOR_NAME " +
                                " from RDB$GENERATORS G " +
                                " where G.RDB$SYSTEM_FLAG is null ";
        string sql_GENERATORS_VAL = "select gen_id(@a,0) from rdb$database";//@a = name generator
        string sql_exceptions = " select E.RDB$EXCEPTION_NAME , E.RDB$MESSAGE " +
                                " from RDB$EXCEPTIONS E " +
                                " where E.RDB$SYSTEM_FLAG is null   ";//сразу готовая команда для создания
        string sql_roles = "select R.RDB$ROLE_NAME from RDB$ROLES R order by 1";
        string sql_tables = " select r.rdb$relation_name from rdb$relations r where r.rdb$view_source is null and r.rdb$system_flag = 0 order by 1";
        string sql_tables_f = " select RR.RDB$FIELD_NAME, RR.RDB$FIELD_SOURCE, RR.RDB$NULL_FLAG," +
                                "        RF.RDB$FIELD_TYPE, RF.RDB$FIELD_LENGTH, RF.RDB$FIELD_SCALE, RF.RDB$FIELD_SUB_TYPE, RF.RDB$SEGMENT_LENGTH,RF.RDB$FIELD_PRECISION " +
                                " from RDB$RELATION_FIELDS RR " +
                                " left join RDB$FIELDS RF on RR.RDB$FIELD_SOURCE = RF.RDB$FIELD_NAME " +
                                " left join RDB$TYPES RT on RF.RDB$FIELD_TYPE = RT.RDB$TYPE and RT.RDB$FIELD_NAME = 'RDB$FIELD_TYPE' " +
                                " where RR.RDB$RELATION_NAME = @a " + //name_table
                                " order by RR.RDB$FIELD_POSITION";
        string sql_views = " select r.rdb$relation_name , r. RDB$VIEW_SOURCE from rdb$relations r where r.rdb$view_source is not null and r.rdb$system_flag = 0";
        string sql_views_f = " select RR.RDB$FIELD_NAME, RR.RDB$FIELD_SOURCE, RR.RDB$NULL_FLAG," +
                            "        RF.RDB$FIELD_TYPE, RF.RDB$FIELD_LENGTH, RF.RDB$FIELD_SCALE, RF.RDB$FIELD_SUB_TYPE, RF.RDB$SEGMENT_LENGTH,RF.RDB$FIELD_PRECISION " +
                            " from RDB$RELATION_FIELDS RR " +
                            " left join RDB$FIELDS RF on RR.RDB$FIELD_SOURCE = RF.RDB$FIELD_NAME " +
                            " left join RDB$TYPES RT on RF.RDB$FIELD_TYPE = RT.RDB$TYPE and RT.RDB$FIELD_NAME = 'RDB$FIELD_TYPE' " +
                            " where RR.RDB$RELATION_NAME = @a " + //name_views
                            " order by RR.RDB$FIELD_POSITION";
        string sql_views_b = "select r.RDB$VIEW_SOURCE from rdb$relations r where R.RDB$RELATION_NAME = @a";//name_views
        string sql_procedures = " select P.RDB$PROCEDURE_NAME from RDB$PROCEDURES P";
        string sql_procedures_f = " select PP.RDB$PARAMETER_NAME, RDB$FIELD_SOURCE, PP.RDB$PARAMETER_TYPE, RF.RDB$FIELD_TYPE, RF.RDB$FIELD_LENGTH, " +
                                "          RF.RDB$FIELD_SCALE, RF.RDB$FIELD_SUB_TYPE, RF.RDB$SEGMENT_LENGTH, RF.RDB$FIELD_PRECISION " +
                                "   from RDB$PROCEDURE_PARAMETERS PP " +
                                "   left join RDB$FIELDS RF on PP.RDB$FIELD_SOURCE = RF.RDB$FIELD_NAME " +
                                "   where PP.RDB$PROCEDURE_NAME = @a " + //name_procedures
                                "   order by PP.RDB$PARAMETER_TYPE, PP.RDB$PARAMETER_NUMBER ";
        string sql_procedures_b = "select P.RDB$PROCEDURE_SOURCE  from RDB$PROCEDURES P where P.RDB$PROCEDURE_NAME = @a ";//name procedures
        string sql_triggers = "select T.RDB$TRIGGER_NAME, T.RDB$RELATION_NAME, T.RDB$TRIGGER_SEQUENCE, T.RDB$TRIGGER_TYPE, T.RDB$TRIGGER_SOURCE," +
                              "        T.RDB$TRIGGER_INACTIVE" +
                              " from RDB$TRIGGERS T " +
                              " where T.Rdb$system_flag IS NULL OR T.Rdb$system_flag = 0  ";
        string sql_triggers_b = "select t.RDB$TRIGGER_SOURCE from RDB$TRIGGERS T where t.rdb$TRIGGER_NAME = @a ";//trigger name
        string sql_primary_key = "select R.RDB$CONSTRAINT_NAME, R.RDB$RELATION_NAME, R.RDB$INDEX_NAME " +
                                                          "  from RDB$RELATION_CONSTRAINTS R " +
                                                          "  where R.RDB$CONSTRAINT_TYPE = 'PRIMARY KEY'  ";
        string sql_primary_key_f = "select I.RDB$FIELD_NAME from RDB$INDEX_SEGMENTS I where I.RDB$INDEX_NAME = @a";//primary key name
        string sql_foreign_key = "select 'alter table ', TRIM(A.RDB$RELATION_NAME), " +
                                                          "         ' add CONSTRAINT ' || TRIM(A.RDB$CONSTRAINT_NAME) || ' ' || ' FOREIGN KEY (' || TRIM(E.RDB$FIELD_NAME) || ') REFERENCES ', " +
                                                          "         TRIM(C.RDB$RELATION_NAME), '(' || TRIM(D.RDB$FIELD_NAME) || ')' || " +
                                                          "         case " +
                                                          "           when B.RDB$UPDATE_RULE <> 'RESTRICT' then ' on update ' || B.RDB$UPDATE_RULE " +
                                                          "           else '' " +
                                                          "         end || " +
                                                          "         case " +
                                                          "           when B.RDB$DELETE_RULE <> 'RESTRICT' then ' on delete ' || B.RDB$DELETE_RULE " +
                                                          "           else '' " +
                                                          "         end || " +
                                                          "         case " +
                                                          "           when A.RDB$CONSTRAINT_NAME <> A.RDB$INDEX_NAME then 'using index ' || TRIM(A.RDB$INDEX_NAME) " +
                                                          "           else '' " +
                                                          "         end " +
                                                          "  from RDB$REF_CONSTRAINTS B, RDB$RELATION_CONSTRAINTS A, RDB$RELATION_CONSTRAINTS C, RDB$INDEX_SEGMENTS D, RDB$INDEX_SEGMENTS E, RDB$INDICES I " +
                                                          "  where (A.RDB$CONSTRAINT_TYPE = 'FOREIGN KEY') and " +
                                                          "        (A.RDB$CONSTRAINT_NAME = B.RDB$CONSTRAINT_NAME) and " +
                                                          "        (B.RDB$CONST_NAME_UQ = C.RDB$CONSTRAINT_NAME) and " +
                                                          "        (C.RDB$INDEX_NAME = D.RDB$INDEX_NAME) and " +
                                                          "        (A.RDB$INDEX_NAME = E.RDB$INDEX_NAME) and " +
                                                          "        (A.RDB$INDEX_NAME = I.RDB$INDEX_NAME) " +
                                                          "  order by A.RDB$RELATION_NAME, A.RDB$CONSTRAINT_NAME, D.RDB$FIELD_POSITION, E.RDB$FIELD_POSITION  ";//sorry - rfunc.TRIM()
        string sql_index = "select 'create ' || " +
                                                          "         case  " +
                                                          "           when I.RDB$UNIQUE_FLAG = 1 then ' UNIQUE INDEX ' " +
                                                          "           else ' INDEX ' " +
                                                          "         end || ' ' || trim(I.RDB$INDEX_NAME) || ' ON ', " +
                                                          "          trim(I.RDB$RELATION_NAME),i.RDB$INDEX_NAME " +
                                                          "  from RDB$INDICES I " +
                                                          "  left join RDB$RELATION_CONSTRAINTS C on I.RDB$INDEX_NAME = C.RDB$INDEX_NAME " +
                                                          "  where (C.RDB$CONSTRAINT_NAME is null) and i.rdb$system_flag is null" +
                                                          "  order by I.RDB$RELATION_NAME, I.RDB$INDEX_NAME ";
        string sql_index_f = "select ISG.RDB$FIELD_NAME " +
                                                          "          from RDB$INDEX_SEGMENTS ISG " +
                                                          "          where ISG.RDB$INDEX_NAME = @a ";//index name
        string sql_users_grant = "select distinct 'GRANT ' || " +
                                                          "                  case " +
                                                          "                    when RU.RDB$PRIVILEGE = 'X' then ' EXECUTE ON ' " +
                                                          "                    when RU.RDB$PRIVILEGE = 'S' then ' SELECT ON ' " +
                                                          "                    when RU.RDB$PRIVILEGE = 'I' then ' INSERT ON ' " +
                                                          "                    when RU.RDB$PRIVILEGE = 'U' then ' UPDATE ON ' " +
                                                          "                    when RU.RDB$PRIVILEGE = 'D' then ' DELETE ON ' " +
                                                          "                    when RU.RDB$PRIVILEGE = 'R' then ' ALL ON ' " +
                                                          "                    when RU.RDB$PRIVILEGE = 'M' then '' " +
                                                          "                  end || " +
                                                          "                  case " +
                                                          "                    when RU.RDB$OBJECT_TYPE = '5' then ' PROCEDURE ' " +
                                                          "                    else '' " +
                                                          "                  end, " +
                                                          "                  trim(RU.RDB$RELATION_NAME),  ' TO ' || " +
                                                          "                  case " +
                                                          "                    when RU.rdb$user_type = '5' then ' PROCEDURE ' " +
                                                          "                    when RU.rdb$user_type = '2' then ' TRIGGER ' " +
                                                          "                    when RU.rdb$user_type = '1' then ' VIEW ' " +
                                                          "                   else '' " +
                                                          "                  end, " +
                                                          "          RU.RDB$USER, " +
                                                          "                  case " +
                                                          "                    when RU.RDB$GRANT_OPTION = '2' then ' WITH ADMIN OPTION ' " +
                                                          "                    when RU.RDB$GRANT_OPTION = '1' then ' WITH GRANT OPTION ' " +
                                                          "                    else '' " +
                                                          "                  end " +
                                                          "  from RDB$USER_PRIVILEGES RU " +
                                                          "  where RU.RDB$USER not in ( 'DBADMIN') ";
        string sql_data = "select first 100000 skip @skip * from @a";//name table and skip data
        string sql_user = " select distinct US.RDB$USER "+
                          " from RDB$USER_PRIVILEGES US "+
                          " where US.RDB$USER_TYPE = 8 and "+
                          "       US.RDB$USER not in ('DBADMIN', 'GUEST', 'REPLICAT', 'DBREPORT') ";
        #endregion
        #region Form
        int i = 1;
        public F99()
        {
            InitializeComponent();
        }

        private void F99_Load(object sender, EventArgs e)
        {
            this.tabControl1.ItemSize = new System.Drawing.Size(1, 1);
            this.treeView1.ExpandAll();
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowLines = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Enabled = false;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Завершить работу мастера?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.SelectDB.ShowDialog() == DialogResult.OK)
            {
                this.t_database.Text = SelectDB.FileName;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.t_database.Enabled = this.is_convert.Checked;
            this.button4.Enabled = this.is_convert.Checked;
            this.is_convert.Enabled = this.is_convert.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.t_programm.Enabled = this.is_install.Checked;
            this.button5.Enabled = this.is_install.Checked;
            //нижнее меню
            this.is_user.Enabled = this.is_install.Checked;
            this.checkBox4.Enabled = this.is_install.Checked;
            this.t_pass.Enabled = this.is_install.Checked;
            //sysdba
            this.t_sysdba.Enabled = this.is_install.Checked;
            this.label2.Enabled = this.is_install.Checked;
            //new sysdba
            this.is_new_sysdba.Enabled = this.is_install.Checked;
            this.t_sysdba_new.Enabled = this.is_install.Checked;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.SelectDir.ShowDialog() == DialogResult.OK)
            {
                this.t_programm.Text = this.SelectDir.SelectedPath;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBox4.Enabled = this.is_user.Checked;
            this.t_pass.Enabled = this.is_user.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
            {
                this.t_pass.PasswordChar = '\0';
            }
            else
            {
                this.t_pass.PasswordChar = '*';
            }
        }

        private async void b_next_Click(object sender, EventArgs e)
        {
            bool is_ok = true;
            i++;
            //проверки перед выполнением
            switch (i)
            {
                case 3:
                    if (this.is_convert.Checked)
                    {
                        if (!File.Exists(this.t_database.Text))
                        {
                            is_ok = false;
                            MessageBox.Show("Файла не существует\nпроверте правильность пути", "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    if (this.is_install.Checked && is_ok)
                    {
                        //проверим
                        //есть ли установленный сервер  FB25
                        is_ok = check_sysdba(this.t_sysdba.Text);
                    }
                    break;
            }
            if (is_ok)
            {
                set_tab(i);
                if (i == 3)
                {

                    is_may_close = false;
                    //теперь настройка содержания
                    if (is_convert.Checked)
                    {
                        treeView1.Nodes["Node3"].ImageIndex = 1;
                        treeView1.Nodes["Node3"].Nodes["Node3_1"].ImageIndex = 1;
                        treeView1.Nodes["Node3"].Nodes["Node3_2"].ImageIndex = 1;
                        treeView1.Nodes["Node3"].Nodes["Node3_3"].ImageIndex = 1;
                        treeView1.Nodes["Node3"].Nodes["Node3_4"].ImageIndex = 1;
                        treeView1.Nodes["Node3"].Nodes["Node3_5"].ImageIndex = 1;
                    }
                    if (is_install.Checked)
                    {
                        treeView1.Nodes["Node0"].ImageIndex = 1;
                        treeView1.Nodes["Node1"].ImageIndex = 1;
                        treeView1.Nodes["Node2"].ImageIndex = 1;
                    }
                    if (is_convert.Checked && is_install.Checked)
                    {
                        treeView1.Nodes["Node4"].ImageIndex = 1;
                    }
                    this.treeView1.Refresh();

                    if (this.is_install.Checked)
                    {
                                               
                        //если нужно установить сервер FB25
                        this.l_.Text = "Установка и деинсталляция сервера";
                        Action a = new Action(run_setup);
                        await Task.Run(a);
                        //восстанавливаем системных пользователей
                        if (this.is_new_sysdba.Checked)
                        {
                            //поставим новый пароль
                            fb_up(this.t_sysdba_new.Text);
                        } 
                        
                        this.l_.Text = "Восстанавливаем системных пользователей";
                        SetImage(2, "Node2");
                        fb_add("DBADMIN", "cnhfiysq", true, "");
                        fb_add("DBREPORT", "uyjvbr", true, "");
                        fb_add("REPLICAT", "dctv", true, "");
                        fb_add("GUEST", "gbpltw", true, "");
                        SetImage(3, "Node2");
                    }
                    if (this.is_convert.Checked)
                    {

                        if (!Directory.Exists(temp_folder))
                        {
                            Directory.CreateDirectory(temp_folder);
                        }
                        //копируем базу
                        this.l_.Text = "Копируем базу локально";
                        this.p_load.Visible = true;

                        Sett.Default.database_original = this.t_database.Text;
                        Sett.Default.database_tmp = this.t_database.Text;
                        /*
                        Sett.Default.database_tmp = temp_folder + @"\tsql_tmp.gdb";
                        Action a_c = new Action(copy_);
                        await Task.Run(a_c);
                        */
                        SetImage(2, "Node3");
                        //если нужно конвертировать базу
                        // Старая база
                        fc_old.Database = Sett.Default.database_tmp;//база, которую нужно конвертировать                
                        fc_old.Pooling = false; //пул соединения - отсутствует - для более быстрого освобождения базы
                        fc_old.ServerType = FbServerType.Embedded;//встроенный сервер
                        fc_old.ClientLibrary = ".\\fbembed.dll";
                        //fc_old.Charset = "win1251"; //кодировка для FB 1/5 не указывается - здесь нужно было переводить в форматы UTF
                        fc_old.UserID = "sysdba";//пользователь по умолчанию
                        fc_old.Password = "masterkey"; //Пароль можно не указывать

                        // Новая база
                        database_new = temp_folder + @"\tsql_2_5.gdb";
                        fc_new.Database = database_new;
                        fc_new.Pooling = false;
                        //и вот здесь у нас настройки
                        if (this.is_install.Checked)
                        {
                            //если пользователь воззжелал установить сервер, то будем в дальнейшем применять его
                            fc_new.ServerType = FbServerType.Default;
                            fc_new.DataSource = "localhost";
                        }
                        else
                        {
                            //иначе - встроенный сервер
                            fc_new.ServerType = FbServerType.Embedded;//встроенный сервер
                            fc_new.ClientLibrary = ".\\fbembed.dll";
                        }

                        //fc_new.ServerType = FbServerType.Embedded;
                        //fc_new.ClientLibrary = ".\\Fx64\\fbembed.dll";
                        fc_new.Charset = "win1251";//при создании новой базы кодировку желательно указывать
                        fc_new.UserID = "dbadmin";
                        fc_new.Password = "cnhfiysq";
                        //create database
                        try
                        {                            
                            FbConnection.CreateDatabase(fc_new.ConnectionString, true);
                        }
                        catch (FbException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        //теперь запускаем процесс анализа метаданных
                        SetImage(2, "Node3_1");
                        this.l_.Text = "Analyze database";
                        this.p_load.Visible = true;
                        Task t1 = run_();
                        await t1;
                        t1.Dispose();
                        SetImage(3, "Node3_1");
                        this.p_load.Image = null;
                        this.p_load.Visible = false;
                        this.progressBar1.Visible = true;
                        //создаем базу
                        //Накатываем первую часть
                        this.progressBar1.Maximum = 100;
                        this.l_.Text = "Create data";
                        //запустим таймер отображения
                        this.timer1.Enabled = true;
                        Task t2 = run2_();
                        await t2;
                        t2.Dispose();
                        //замена базы
                        if (sb.Length <= 0)
                        {                            
                            //обновляем параметр
                            update_param(this.t_sysdba_new.Text);
                            if (is_replace.Checked)
                            {
                                this.progressBar1.Value = 0;

                                //while(f.Attributes == FileAttributes.
                                SetImage(2, "Node3_5");
                                Action a_r = run_replace;
                                await Task.Run(a_r);
                                SetImage(3, "Node3_5");
                            }
                            //конец
                            SetImage(3, "Node3");
                        }

                    }
                    if (sb.Length <= 0 && is_convert.Checked && is_install.Checked)
                    {
                        SetImage(2, "Node4");
                        this.l_.Text = "Восстановление пользователей";
                        Action a = run_user;
                        await Task.Run(a);
                        SetImage(3, "Node4");
                    }
                    this.timer1.Enabled = false;
                    this.b_next.Enabled = true;
                    is_may_close = true;
                    this.b_next.PerformClick();
                }
                else
                    if (i == 5)
                    {
                        Application.Exit();
                    }
            }
            else
            {
                i--;
            }
        }
        private void b_prev_Click(object sender, EventArgs e)
        {
            i--;
            set_tab(i);
        }
        void set_tab(int j)
        {
            switch (j)
            {
                case 1:
                    this.tabControl1.SelectedIndex = j - 1;
                    this.b_prev.Enabled = false;
                    this.b_next.Enabled = true;
                    this.button3.Enabled = true;
                    break;
                case 2:
                    this.tabControl1.SelectedIndex = j - 1;
                    this.b_prev.Enabled = true;
                    this.b_next.Enabled = true;
                    this.button3.Enabled = true;
                    break;
                case 3:
                    this.tabControl1.SelectedIndex = j - 1;
                    this.b_prev.Enabled = false;
                    this.b_next.Enabled = false;
                    this.button3.Enabled = false;
                    break;
                case 4:
                    this.tabControl1.SelectedIndex = j - 1;
                    this.b_prev.Enabled = false;
                    this.b_next.Text = "Завершить";
                    this.b_next.Enabled = true;
                    this.button3.Visible = false;
                    this.richTextBox1.Clear();
                    if (sb.Length > 0)
                    {
                        
                        try
                        {
                            using (StreamWriter outfile = new StreamWriter(File.Create(temp_folder + @"\error.log"), Encoding.Default))
                            {
                                sb.Replace("{", "{{");
                                sb.Replace("}", "}}");
                                outfile.Write(sb.ToString(), Encoding.Default);
                            }
                            this.richTextBox1.AppendText(File.ReadAllText(temp_folder + @"\error.log",Encoding.Default));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        this.image_bad.Image = dataPump.det.Properties.Resources.l_fatal;
                    }
                    else
                    {
                        this.richTextBox1.Text = "Конвертация прошла успешно";
                        this.image_bad.Image = dataPump.det.Properties.Resources.l_done;                        
                    }

                    this.richTextBox1.Visible = true;
                    this.l_error.Visible = true;
                    this.image_bad.Visible = true;
                    break;
            }
        }
        #endregion
        #region run
        public async Task run_()
        {
            this.progressBar1.Maximum = 18;
            this.l_.Text = "analyze";
            this.progressBar1.Value++;
            Task t_udf = new Task(m_UDF);            
            Task t_domains = new Task(m_DOMAINS);
            Task t_generators = new Task(m_GENERATORS);
            Task t_exceptions = new Task(m_EXCEPTIONS);
            Task t_roles = new Task(m_ROLES);
            Task t_tables = new Task(m_TABLES);
            Task t_views = new Task(m_VIEWS);
            Task t_procedures_prototype = new Task(m_PROCEDURES_PROTOTYPE);
            Task t_triggers_prototype = new Task(m_TRIGGERS_PROTOTYPE);
            Task t_grants = new Task(m_GRANTS);
            Task t_procedures_content = new Task(m_PROCEDURES);
            Task t_triggers_content = new Task(m_TRIGGERS);
            Task t_primary_key = new Task(m_PRIMARY_KEY);
            Task t_foreign_key = new Task(m_FOREIGN_KEY);
            Task t_indexes = new Task(m_INDEXES);

            t_udf.Start();
            await t_udf;
            t_domains.Start();
            await t_domains;
            t_generators.Start();
            await t_generators;
            t_exceptions.Start();
            await t_exceptions;
            t_roles.Start();
            await t_roles;
            t_tables.Start();
            await t_tables;
            t_views.Start();
            await t_views;

            t_procedures_prototype.Start();
            await t_procedures_prototype;
            t_triggers_prototype.Start();
            await t_triggers_prototype;

            t_grants.Start();
            await t_grants;
            t_procedures_content.Start();
            await t_procedures_content;
            t_triggers_content.Start();
            await t_triggers_content;

            t_primary_key.Start();
            await t_primary_key;
            t_foreign_key.Start();
            await t_foreign_key;
            t_indexes.Start();        
            await t_indexes;

            //а вот теперь сформируем основные комманды
            com.Clear();
            com.AddRange(com_udf); com_udf.Clear();
            com.AddRange(com_generators); com_generators.Clear();
            com.AddRange(com_domains); com_domains.Clear();
            com.AddRange(com_exceptions); com_exceptions.Clear();
            com.AddRange(com_roles); com_roles.Clear();
            com.AddRange(com_tables); com_tables.Clear();
            com.AddRange(com_views); com_views.Clear();
            com.AddRange(com_procedures_prototype); com_procedures_prototype.Clear();
            com.AddRange(com_triggers_prototype); com_triggers_prototype.Clear();
            com.AddRange(com_grants); com_grants.Clear();
            com.AddRange(com_procedures_content); com_procedures_content.Clear();

            //а это последний блок
            com3.Clear();
            com3.AddRange(com_triggers_content); com_triggers_content.Clear();
            com3.AddRange(com_primary_key); com_primary_key.Clear();
            com3.AddRange(com_indexes); com_indexes.Clear();
            com3.AddRange(com_foreign_key); com_foreign_key.Clear();
            
        }
        #endregion
        #region run2
        public async Task run2_()
        {
            //Первый этап создания - основные DDL
            SetImage(2, "Node3_2");
            this.progressBar1.Value = 0;
            Action a = execute_com;
            await Task.Run(a);
            a = null;
            com.Clear();

            if (sb.Length <= 0)
            {
                this.progressBar1.Maximum = 100;
                SetImage(3, "Node3_2");
                SetImage(2, "Node3_3");
                int i_ = 0;
                int j_ = tables_.Count;
                this.progressBar2.Maximum = 100;
                this.progressBar2.Visible = true;
                this.l_count.Visible = true;
                //второй этап - данные
                //запускаем анализ
                Task t_prepare = new Task(go_data);
                t_prepare.Start();

                await t_prepare;

                this.progressBar2.Visible = false;
                this.l_count.Visible = false;
                com2.Clear();//очистим список значений - re:
                SetImage(3, "Node3_3");
                //and the last
                SetImage(2, "Node3_4");
                com = com3;
                this.progressBar1.Value = 0;
                a = execute_com;
                await Task.Run(a);
                a = null;
                com.Clear();
                com2.Clear();
                if (sb.Length > 0)
                {
                    SetImage(4, "Node3_4");
                }
                else
                {
                    SetImage(3, "Node3_4");
                }
            }
            else
            {
                SetImage(4, "Node3_2");
            }
        }
        #endregion
        #region run_setup
        public void run_setup()
        {
            string file_un = "";
            string file_run = "";
            //удаляем предыдущие серверы
            //сначала удалим Firebird 1.5
            //дислокация определяется из соображений обывателя
            string dir_ = get_install("Firebird 1.5");
            SetImage(2, "Node0"); //устанавливаем текущий процесс
            if (dir_ != "")
            {
                file_un = dir_ + @"\unins000.exe";
                if (File.Exists(file_un))
                {
                    
                    //удаление Firebird 1_5
                    //завершаем процесс
                    var ps1 = Process.GetProcessesByName("fbguard");
                    foreach (Process p1 in ps1) p1.Kill();

                    var ps2 = Process.GetProcessesByName("fbserver");
                    foreach (Process p1 in ps2) p1.Kill();
                    //запускаем в тихом тежиме
                    Process p = new Process();
                    p.StartInfo.FileName = file_un;
                    p.StartInfo.Arguments = @"/SILENT";
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = true;
                    p.Start();
                    p.WaitForExit();
                }                
            }
            SetImage(3, "Node0");//закончили деинсталл
            //теперь запустим тихий режим установки
            //установка Firebird 2_5
            SetImage(2, "Node1");
            dir_ = get_install("Firebird 2.5");
            if (dir_ == "")
            {
                if (System.Environment.Is64BitOperatingSystem)
                {
                    file_run = ".\\Setup\\x64\\F25x64.exe";
                }
                else
                {
                    file_run = ".\\Setup\\x86\\F25x86.exe";
                }
                Process p = new Process();
                p.StartInfo.FileName = file_run;
                //ключи
                //супер мега тихая установка
                //Папка назначения
                //Все остальные параметры - по умолчанию
                p.StartInfo.Arguments = @"/VERYSILENT /DIR="+  "\""+ this.t_programm.Text + "\"";
                //p.StartInfo.CreateNoWindow = true;
                //p.StartInfo.UseShellExecute = true;
                try
                {
                    p.Start();
                    p.WaitForExit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + file_run);
                }
                dir_ = get_install("Firebird 2.5");
            }
            //а теперь проверим наши библиотеки
            dir_ += @"\UDF";
            if (!File.Exists(dir_ + @"\faust_udf.dll"))
            {
                //здесь добавить забор для определенной версии ОС
                byte[] resf;
                if (System.Environment.Is64BitOperatingSystem)
                {
                    resf = File.ReadAllBytes(".\\Setup\\x64\\faust_udf.dll");
                }
                else
                {
                    resf = File.ReadAllBytes(".\\Setup\\x86\\faust_udf.dll");
                }

                File.WriteAllBytes(dir_ + @"\faust_udf.dll", resf);
            }
            if (!File.Exists(dir_ + @"\rfunc.dll"))
            {
                //здесь добавить забор для определенной версии ОС
                byte[] resf;
                if (System.Environment.Is64BitOperatingSystem)
                {
                    resf = File.ReadAllBytes(".\\Setup\\x64\\rfunc_64.dll");
                }
                else
                {
                    resf = File.ReadAllBytes(".\\Setup\\x86\\rfunc_32.dll");
                }

                File.WriteAllBytes(dir_ + @"\rfunc.dll", resf);
            }
            SetImage(3, "Node1");

        }
        #endregion
        #region run_sys_user
        public void run_user()
        {
            using (FbConnection fb = new FbConnection(fc_new.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_user, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    fb_add(fr[0].ToString(), this.t_pass.Text, false, null);
                                }
                            }
                            fcon.Dispose();
                        }
                        ft.Dispose();
                    }
                }
                catch { }
                finally { fb.Close(); }
                fb.Dispose();
            }
        }
        #endregion
        #region run_replace
        public void run_replace()
        {
            bool is_locked = true;
            while (is_locked)
            {               
               try
                {
                    p_cur = 0;
                    p_text = "Архивируем базу оригинал";
                    FileInfo file_ = new FileInfo(Sett.Default.database_original);
                    Compress(file_);

                    byte[] buffer = new byte[4096 * 4096]; // 1MB buffer
                    bool cancelFlag = false;

                    using (FileStream source = new FileStream(fc_new.Database, FileMode.Open, FileAccess.Read))
                    {
                        long fileLength = source.Length;
                        using (FileStream dest = new FileStream(Sett.Default.database_original, FileMode.Create, FileAccess.Write))
                        {

                            is_locked = false;
                            long totalBytes = 0;
                            int currentBlockSize = 0;

                            while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                totalBytes += currentBlockSize;
                                double persentage = totalBytes / (double)fileLength * 100.0;

                                dest.Write(buffer, 0, currentBlockSize);

                                cancelFlag = false;
                                p_cur = (int)((float)totalBytes / (float)fileLength * 100);
                                p_text = "Заменяем файл " + p_cur + @"%";

                                if (totalBytes >= fileLength)
                                {
                                    cancelFlag = true;
                                }
                                if (cancelFlag == true)
                                {
                                    // Delete dest file here
                                    dest.Flush();
                                    dest.Close();
                                    source.Flush();
                                    source.Close();
                                    break;
                                }
                            }
                        }
                    }
                }
                catch { System.Threading.Thread.Sleep(5000); }
            }
        }
        #endregion 
        #region FIRST COM
        #region UDF
        public void m_UDF()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        try
                        {
                            //сформируем список функций - их создание
                            using (FbCommand fcon = new FbCommand(sql_udf, fb, ft))
                            {
                                using (FbDataReader fr = fcon.ExecuteReader())
                                {
                                    while (fr.Read())
                                    {
                                        //будующая заготовка
                                        string dll_command = "DECLARE EXTERNAL FUNCTION " + fr[0].ToString().Trim();
                                        //будующий возврат
                                        string dll_return = "RETURNS ";
                                        //оконцовка
                                        string dll_end = "ENTRY_POINT '" + fr[5].ToString().Trim() + "'" + " MODULE_NAME '" + fr[4].ToString() + "'";
                                        //Теперь нам нужны возвращаемые параметры
                                        string dll_argument = "";
                                        #region Формирование аргументов
                                        using (FbCommand fcon_a = new FbCommand(sql_udf_a, fb, ft))
                                        {
                                            fcon_a.Parameters.Add("@a", FbDbType.VarChar, 31);
                                            fcon_a.Parameters[0].Value = fr[0].ToString();
                                            using (FbDataReader fr_a = fcon_a.ExecuteReader())
                                            {
                                                while (fr_a.Read())
                                                {
                                                    if (fr_a[0].ToString() == fr[6].ToString())
                                                    {
                                                        if (fr[6].ToString() == "0")
                                                        {
                                                            //Значит выходной
                                                            dll_return += get_field_type(fr_a[2].ToString(), fr_a[4].ToString(), fr_a[3].ToString(), fr_a[5].ToString(), "0", fr_a[8].ToString(), true);
                                                            //теперь как значение или как ссылка
                                                            if (fr_a[1].ToString() == "-1")
                                                            {
                                                                //FREE_IT
                                                                dll_return += " FREE_IT";
                                                            }
                                                            else
                                                                if (fr_a[1].ToString() == "0")
                                                                {
                                                                    dll_return += " BY VALUE";
                                                                }
                                                        }
                                                        else
                                                        {
                                                            dll_return += "PARAMETER " + fr_a[0].ToString();
                                                            if (dll_argument == "")
                                                                dll_argument += get_field_type(fr_a[2].ToString(), fr_a[4].ToString(), fr_a[3].ToString(), fr_a[5].ToString(), "0", fr_a[8].ToString(), true);
                                                            else
                                                            {
                                                                dll_argument += "," + "\n" + get_field_type(fr_a[2].ToString(), fr_a[4].ToString(), fr_a[3].ToString(), fr_a[5].ToString(), "0", fr_a[8].ToString(), true);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //а вот теперь начнем формировать строку создания
                                                        if (dll_argument == "")
                                                            dll_argument += get_field_type(fr_a[2].ToString(), fr_a[4].ToString(), fr_a[3].ToString(), fr_a[5].ToString(), "0", fr_a[8].ToString(), true);
                                                        else
                                                        {
                                                            dll_argument += "," + "\n" + get_field_type(fr_a[2].ToString(), fr_a[4].ToString(), fr_a[3].ToString(), fr_a[5].ToString(), "0", fr_a[8].ToString(), true);
                                                        }
                                                    }
                                                }
                                                fr_a.Dispose();
                                            }
                                            fcon_a.Dispose();
                                        }
                                        #endregion
                                        //и вот она наша собранная строка 
                                        dll_command += "\n" + dll_argument + "\n" + dll_return + "\n" + dll_end;
                                        dll_command = regexTrim.Replace(dll_command, "TRIM_");
                                        dll_command = regexIIF.Replace(dll_command, "IIF_");
                                        //теперь добавим ее в список команд
                                        com_udf.Add(dll_command);
                                    }
                                    fr.Dispose();
                                }
                                fcon.Dispose();
                            }
                        }
                        catch (FbException ex2)
                        {
                            MessageBox.Show(ex2.Message);
                        }
                        ft.Commit();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("udf");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                    fb.Dispose();
                }
            }

        }

        private static readonly Regex regexTrim = new Regex(@"\bTrim\b", RegexOptions.IgnoreCase);
        private static readonly Regex regexIIF = new Regex(@"\bIIF\b", RegexOptions.IgnoreCase);
        #endregion
        #region DOMAINS
        public void m_DOMAINS()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_domains, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    //основа
                                    string dll_command = "create domain " + fr[0].ToString() + "\n" + "as" + "\n";
                                    //теперь тип данных
                                    dll_command += get_field_type(fr[10].ToString(), fr[8].ToString(), fr[9].ToString(), fr[11].ToString(), fr[17].ToString(), fr[27].ToString()) + "\n";
                                    //есть ли у него Character set
                                    if (fr[28].ToString() != DBNull.Value.ToString())
                                    {
                                        dll_command += " character set " + fr[28].ToString() + "\n";
                                    }
                                    //дальше идет значение DEFAULT
                                    if (fr[7].ToString() != DBNull.Value.ToString())
                                    {
                                        dll_command += fr[7].ToString() + "\n";
                                    }
                                    //проверка
                                    if (fr[3].ToString() != DBNull.Value.ToString())
                                    {
                                        dll_command += fr[3].ToString() + "\n";
                                    }
                                    //COLLATE
                                    if (fr[29].ToString() != DBNull.Value.ToString())
                                    {
                                        dll_command += fr[29].ToString() + "\n";
                                    }
                                    //записываем
                                    com_domains.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("DOMAINS");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region GENERATORS
        public void m_GENERATORS()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_GENERATORS, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    //основа
                                    string dll_command = "CREATE GENERATOR " + fr[0].ToString() + "\n";
                                    //теперь значение
                                    string dll_val = "";
                                    using (FbCommand fcon_a = new FbCommand(sql_GENERATORS_VAL.Replace("@a", fr[0].ToString()), fb, ft))
                                    {
                                        using (FbDataReader fr_a = fcon_a.ExecuteReader())
                                        {
                                            while (fr_a.Read())
                                            {
                                                dll_val = "SET GENERATOR " + fr[0].ToString() + " TO " + fr_a[0].ToString();
                                            }
                                            fr_a.Dispose();
                                        }
                                        fcon_a.Dispose();
                                    }
                                    //добавляем наши команды
                                    com_generators.Add(dll_command);
                                    com_generators.Add(dll_val);
                                }
                                fr.Dispose();
                            }
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("GENERATORS");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region EXCEPTIONS
        public void m_EXCEPTIONS()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_exceptions, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "create exception " + fr[0].ToString() + " '" + fr[1].ToString() + "'";
                                    com_exceptions.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("EXCEPTIONS");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region ROLES
        public void m_ROLES()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_roles, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "create role " + fr[0].ToString();
                                    com_roles.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("ROLES");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region TABLES
        public void m_TABLES()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_tables, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    tables_.Add(fr[0].ToString());//добавляем сразу в список таблиц
                                    string dll_command = "create table ";
                                    if (is_reserv(fr[0].ToString()))
                                    {
                                        dll_command += "\"" + fr[0].ToString() + "\"" + "(" + "\n";
                                    }
                                    else
                                    {
                                        dll_command += fr[0].ToString() + "(" + "\n";
                                    }
                                    //спикос полей
                                    using (FbCommand fcon_a = new FbCommand(sql_tables_f, fb, ft))
                                    {
                                        fcon_a.Parameters.Add("@a", FbDbType.VarChar, 31);
                                        fcon_a.Parameters[0].Value = fr[0].ToString();
                                        using (FbDataReader fr_a = fcon_a.ExecuteReader())
                                        {
                                            string fields_ = "";
                                            while (fr_a.Read())
                                            {
                                                //если у нас поле происходит из простого типа, то найдем его
                                                if (fr_a[1].ToString().Substring(0, 4) == "RDB$")
                                                {
                                                    if (fields_ != "")
                                                    {
                                                        fields_ += "," + "\n";
                                                    }
                                                    fields_ += fr_a[0].ToString() + " " + get_field_type(fr_a[3].ToString(),
                                                                                                        fr_a[4].ToString(),
                                                                                                        fr_a[5].ToString(),
                                                                                                        fr_a[6].ToString(),
                                                                                                        fr_a[7].ToString(),
                                                                                                        fr_a[8].ToString());
                                                }
                                                else
                                                {
                                                    if (fields_ != "")
                                                    {
                                                        fields_ += "," + "\n";
                                                    }
                                                    fields_ += fr_a[0].ToString() + " " + fr_a[1].ToString();
                                                }
                                                //Обязательное?
                                                if (fr_a[2].ToString().Trim() == "1")
                                                {
                                                    fields_ += " NOT NULL";
                                                }
                                            }
                                            dll_command += fields_ + "\n" + ")";
                                            fr_a.Dispose();
                                        }
                                        fcon_a.Dispose();
                                    }

                                    com_tables.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region VIEWS
        public void m_VIEWS()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_views, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "create view ";
                                    if (is_reserv(fr[0].ToString()))
                                    {
                                        dll_command += "\"" + fr[0].ToString() + "\"" + "(" + "\n";
                                    }
                                    else
                                    {
                                        dll_command += fr[0].ToString() + "(" + "\n";
                                    }
                                    //поля
                                    using (FbCommand fcon_a = new FbCommand(sql_views_f, fb, ft))
                                    {
                                        fcon_a.Parameters.Add("@a", FbDbType.VarChar, 31);
                                        fcon_a.Parameters[0].Value = fr[0].ToString();
                                        using (FbDataReader fr_a = fcon_a.ExecuteReader())
                                        {
                                            string fields_ = "";
                                            while (fr_a.Read())
                                            {
                                                if (fields_ != "")
                                                {
                                                    fields_ += "," + "\n";
                                                }
                                                fields_ += fr_a[0].ToString();
                                            }
                                            dll_command += fields_ + "\n" + ")" + "\nAS\n";
                                            fr_a.Dispose();
                                        }
                                        fcon_a.Dispose();
                                    }
                                    //Содерживое
                                    using (FbCommand fcon_b = new FbCommand(sql_views_b, fb, ft))
                                    {
                                        fcon_b.Parameters.Add("@a", FbDbType.VarChar, 31);
                                        fcon_b.Parameters[0].Value = fr[0].ToString();
                                        using (FbDataReader fr_b = fcon_b.ExecuteReader())
                                        {
                                            while (fr_b.Read())
                                            {
                                                dll_command += "\n" + fr_b.GetString(0);
                                            }
                                            fr_b.Dispose();
                                        }
                                        fcon_b.Dispose();
                                    }
                                    com_views.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion

        #region PROCEDURES PROTOTYPE
        public void m_PROCEDURES_PROTOTYPE()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_procedures, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "create or alter procedure ";
                                    if (is_reserv(fr[0].ToString()))
                                    {
                                        dll_command += "\"" + fr[0].ToString() + "\"" + "\n";
                                    }
                                    else
                                    {
                                        dll_command += fr[0].ToString() + "\n";
                                    }
                                    //Список полей
                                    string fields_in = "";
                                    string fields_out = "";
                                    bool is_out = false;
                                    using (FbCommand fcon_a = new FbCommand(sql_procedures_f, fb, ft))
                                    {
                                        fcon_a.Parameters.Add("@a", FbDbType.VarChar, 31);
                                        fcon_a.Parameters[0].Value = fr[0].ToString();
                                        using (FbDataReader fr_a = fcon_a.ExecuteReader())
                                        {
                                            while (fr_a.Read())
                                            {
                                                //резервное?
                                                var fields_name = fr_a[0].ToString().Trim();
                                                if (is_reserv(fields_name))
                                                {
                                                    fields_name = "\"" + fields_name + "\"";
                                                }
                                                //Извлекаем тип данных
                                                string field_type = get_field_type(fr_a[3].ToString(), fr_a[4].ToString(), fr_a[5].ToString(), fr_a[6].ToString(), fr_a[7].ToString(), fr_a[8].ToString());
                                                //теперь в зависимости от типа параметра (вх/вых) - запишем в определенный блок
                                                if (fr_a[2].ToString() == "0")
                                                {
                                                    //входной

                                                    if (fields_in == "")
                                                    {
                                                        fields_in += "( " + fields_name + " " + field_type;
                                                    }
                                                    else
                                                    {
                                                        fields_in += ",\n " + fields_name + " " + field_type;
                                                    }
                                                }
                                                else
                                                {
                                                    //значит выходной
                                                    if (!is_out)
                                                    {
                                                        //ставим флаг, что есть выходные параметры
                                                        is_out = true;
                                                    }
                                                    if (fields_out == "")
                                                    {
                                                        fields_out += "returns ( " + fields_name + " " + field_type;
                                                    }
                                                    else
                                                    {
                                                        fields_out += ",\n " + fields_name + " " + field_type;
                                                    }
                                                }
                                            }
                                            fr_a.Dispose();
                                        }
                                        fcon_a.Dispose();
                                    }
                                    //теперь соединяем 
                                    if (fields_in != "")
                                    {
                                        fields_in += "\n )";
                                    }
                                    if (fields_out != "")
                                    {
                                        fields_out += "\n )";
                                    }
                                    dll_command += fields_in + fields_out + "\n AS\nBEGIN\n";
                                    if (is_out)
                                    {
                                        dll_command += "suspend;\n";
                                    }
                                    dll_command += "\nEND";
                                    com_procedures_prototype.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region TRIGGERS PROTOTYPE
        public void m_TRIGGERS_PROTOTYPE()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_triggers, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "create or alter trigger " + fr[0].ToString().Trim() + " FOR " + fr[1].ToString();
                                    //активность
                                    if (fr[5].ToString().Trim() == "1")
                                    {
                                        dll_command += "\n" + "INACTIVE";
                                    }
                                    else
                                    {
                                        dll_command += "\n" + "ACTIVE";
                                    }
                                    //теперь узнаем что за тип триггера
                                    switch (fr[3].ToString().Trim())
                                    {
                                        case "1":
                                            dll_command += " before insert ";
                                            break;
                                        case "2":
                                            dll_command += " after insert ";
                                            break;
                                        case "3":
                                            dll_command += " before update ";
                                            break;
                                        case "4":
                                            dll_command += " after update ";
                                            break;
                                        case "5":
                                            dll_command += " before delete ";
                                            break;
                                        case "6":
                                            dll_command += " after delete ";
                                            break;
                                        case "17":
                                            dll_command += " before insert or update ";
                                            break;
                                        case "25":
                                            dll_command += " before insert or delete";
                                            break;
                                        case "113":
                                            dll_command += " before insert or update or delete ";
                                            break;
                                        case "27":
                                            dll_command += " before update or delete ";
                                            break;
                                        case "18":
                                            dll_command += " after insert or update ";
                                            break;
                                        case "26":
                                            dll_command += " after insert or delete";
                                            break;
                                        case "114":
                                            dll_command += " after insert or update or delete ";
                                            break;
                                        case "28":
                                            dll_command += " after update or delete ";
                                            break;
                                    }
                                    //позиция триггера
                                    dll_command += " position " + fr[2].ToString().Trim();
                                    dll_command += "\nAS\nBEGIN\n\nEND";
                                    com_triggers_prototype.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("TRIGGERS PROTOTYPE");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region GRANTS
        public void m_GRANTS()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_users_grant, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                string dll_command = "";
                                while (fr.Read())
                                {
                                    dll_command = fr[0].ToString();
                                    if (is_reserv(fr[1].ToString()) || !is_en_string(fr[1].ToString()))
                                    {
                                        dll_command += "\"" + fr[1].ToString() + "\"";
                                    }
                                    else
                                    {
                                        dll_command += fr[1].ToString();
                                    }
                                    dll_command += fr[2].ToString();
                                    if (is_reserv(fr[3].ToString()) || !is_en_string(fr[3].ToString()))
                                    {
                                        dll_command += "\"" + fr[3].ToString() + "\"";
                                    }
                                    else
                                    {
                                        dll_command += fr[3].ToString();
                                    }
                                    dll_command += fr[4].ToString();
                                    com_grants.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region PROCEDURES
        public void m_PROCEDURES()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_procedures, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "create or alter procedure ";
                                    if (is_reserv(fr[0].ToString()))
                                    {
                                        dll_command += "\"" + fr[0].ToString() + "\"" + "\n";
                                    }
                                    else
                                    {
                                        dll_command += fr[0].ToString() + "\n";
                                    }
                                    //Список полей
                                    string fields_in = "";
                                    string fields_out = "";
                                    bool is_out = false;
                                    using (FbCommand fcon_a = new FbCommand(sql_procedures_f, fb, ft))
                                    {
                                        fcon_a.Parameters.Add("@a", FbDbType.VarChar, 31);
                                        fcon_a.Parameters[0].Value = fr[0].ToString();
                                        using (FbDataReader fr_a = fcon_a.ExecuteReader())
                                        {
                                            while (fr_a.Read())
                                            {
                                                //резервное?
                                                var fields_name = fr_a[0].ToString().Trim();
                                                if (is_reserv(fields_name))
                                                {
                                                    fields_name = "\"" + fields_name + "\"";
                                                }
                                                //Извлекаем тип данных
                                                string field_type = get_field_type(fr_a[3].ToString(), fr_a[4].ToString(), fr_a[5].ToString(), fr_a[6].ToString(), fr_a[7].ToString(), fr_a[8].ToString());
                                                //теперь в зависимости от типа параметра (вх/вых) - запишем в определенный блок
                                                if (fr_a[2].ToString() == "0")
                                                {
                                                    //входной

                                                    if (fields_in == "")
                                                    {
                                                        fields_in += "( " + fields_name + " " + field_type;
                                                    }
                                                    else
                                                    {
                                                        fields_in += ",\n " + fields_name + " " + field_type;
                                                    }
                                                }
                                                else
                                                {
                                                    //значит выходной
                                                    if (!is_out)
                                                    {
                                                        //ставим флаг, что есть выходные параметры
                                                        is_out = true;
                                                    }
                                                    if (fields_out == "")
                                                    {
                                                        fields_out += "returns ( " + fields_name + " " + field_type;
                                                    }
                                                    else
                                                    {
                                                        fields_out += ",\n " + fields_name + " " + field_type;
                                                    }
                                                }
                                            }
                                            fr_a.Dispose();
                                        }
                                        fcon_a.Dispose();
                                    }
                                    //теперь соединяем 
                                    if (fields_in != "")
                                    {
                                        fields_in += "\n )";
                                    }
                                    if (fields_out != "")
                                    {
                                        fields_out += "\n )";
                                    }
                                    dll_command += fields_in + fields_out + "\n AS\n";
                                    //Добавляем содержимое
                                    try
                                    {
                                        using (FbCommand fcon_b = new FbCommand(sql_procedures_b, fb, ft))
                                        {
                                            fcon_b.Parameters.Add("@a", FbDbType.VarChar, 31);
                                            fcon_b.Parameters[0].Value = fr[0].ToString();
                                            using (FbDataReader fr_b = fcon_b.ExecuteReader())
                                            {
                                                while (fr_b.Read()) {                                                    
                                                    dll_command += fr_b.GetString(0);
                                                    //.Replace("TRIM", "TRIM_").Replace("Trim", "TRIM_").Replace("trim", "TRIM_").Replace("IIF", "IIF_").Replace("iif", "IIF_").Replace("Iif", "IIF_");
                                                }
                                                fr_b.Dispose();
                                            }
                                            fcon_b.Dispose();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                    dll_command = regexTrim.Replace(dll_command, "TRIM_");
                                    dll_command = regexIIF.Replace(dll_command, "IIF_");
                                    com_procedures_content.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #endregion
        #region SECOND COM
        #region TRIGGERS CONTENT
        public void m_TRIGGERS()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_triggers, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "create or alter trigger " + fr[0].ToString().Trim() + " FOR " + fr[1].ToString();
                                    //активность
                                    if (fr[5].ToString().Trim() == "1")
                                    {
                                        dll_command += "\n" + "INACTIVE";
                                    }
                                    else
                                    {
                                        dll_command += "\n" + "ACTIVE";
                                    }
                                    //теперь узнаем что за тип триггера
                                    switch (fr[3].ToString().Trim())
                                    {
                                        case "1":
                                            dll_command += " before insert ";
                                            break;
                                        case "2":
                                            dll_command += " after insert ";
                                            break;
                                        case "3":
                                            dll_command += " before update ";
                                            break;
                                        case "4":
                                            dll_command += " after update ";
                                            break;
                                        case "5":
                                            dll_command += " before delete ";
                                            break;
                                        case "6":
                                            dll_command += " after delete ";
                                            break;
                                        case "17":
                                            dll_command += " before insert or update ";
                                            break;
                                        case "25":
                                            dll_command += " before insert or delete";
                                            break;
                                        case "113":
                                            dll_command += " before insert or update or delete ";
                                            break;
                                        case "27":
                                            dll_command += " before update or delete ";
                                            break;
                                        case "18":
                                            dll_command += " after insert or update ";
                                            break;
                                        case "26":
                                            dll_command += " after insert or delete";
                                            break;
                                        case "114":
                                            dll_command += " after insert or update or delete ";
                                            break;
                                        case "28":
                                            dll_command += " after update or delete ";
                                            break;
                                    }
                                    //позиция триггера
                                    dll_command += " position " + fr[2].ToString().Trim();
                                    //Добавляем содержимое
                                    using (FbCommand fcon_b = new FbCommand(sql_triggers_b, fb, ft))
                                    {
                                        fcon_b.Parameters.Add("@a", FbDbType.VarChar, 31);
                                        fcon_b.Parameters[0].Value = fr[0].ToString();
                                        using (FbDataReader fr_b = fcon_b.ExecuteReader())
                                        {
                                            while (fr_b.Read())
                                            {
                                                dll_command += "\n" + fr_b.GetString(0);
                                            }
                                            fr_b.Dispose();
                                        }
                                        fcon_b.Dispose();
                                    }
                                    dll_command = regexTrim.Replace(dll_command, "TRIM_");
                                    dll_command = regexIIF.Replace(dll_command, "IIF_");
                                    com_triggers_content.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("TRIGGERS Content");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region PRIMARY KEY
        public void m_PRIMARY_KEY()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_primary_key, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "";
                                    dll_command += "ALTER TABLE ";
                                    if (is_reserv(fr[1].ToString().Trim()))
                                    {
                                        dll_command += "\"" + fr[1].ToString().Trim() + "\"";
                                    }
                                    else
                                    {
                                        dll_command += fr[1].ToString().Trim();
                                    }
                                    if (fr[0].ToString().Substring(0, 5).ToUpper() == "INTEG")
                                    {
                                        //если это просто индекс без наименования - то есть созданный системой
                                        dll_command += " ADD  PRIMARY KEY ";
                                    }
                                    else
                                    {
                                        dll_command += " ADD CONSTRAINT " + fr[0].ToString().Trim() + " PRIMARY KEY ";
                                    }
                                    //соостав индекса
                                    string fields_ = "";
                                    using (FbCommand fcon_f = new FbCommand(sql_primary_key_f, fb, ft))
                                    {
                                        fcon_f.Parameters.Add("@a", FbDbType.VarChar, 31);
                                        fcon_f.Parameters[0].Value = fr[2].ToString();
                                        using (FbDataReader fr_f = fcon_f.ExecuteReader())
                                        {
                                            while (fr_f.Read())
                                            {
                                                if (fields_ == "")
                                                {
                                                    fields_ += "(" + fr_f[0].ToString();
                                                }
                                                else
                                                {
                                                    fields_ += "," + fr_f[0].ToString();
                                                }
                                            }
                                            fields_ += ")";
                                            fr_f.Dispose();
                                        }
                                        fcon_f.Dispose();
                                    }
                                    dll_command += " " + fields_;
                                    com_primary_key.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region FOREIGN KEY
        public void m_FOREIGN_KEY()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_foreign_key, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "";
                                    dll_command += fr[0].ToString();
                                    if (is_reserv(fr[1].ToString().Trim()))
                                    {
                                        dll_command += "\"" + fr[1].ToString().Trim() + "\"";
                                    }
                                    else
                                    {
                                        dll_command += fr[1].ToString().Trim();
                                    }
                                    dll_command += fr[2].ToString();
                                    if (is_reserv(fr[3].ToString().Trim()))
                                    {
                                        dll_command += "\"" + fr[3].ToString().Trim() + "\"";
                                    }
                                    else
                                    {
                                        dll_command += fr[3].ToString().Trim();
                                    }
                                    dll_command += fr[4].ToString();

                                    com_foreign_key.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region INDEX
        public void m_INDEXES()
        {
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_index, fb, ft))
                        {
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    string dll_command = "";
                                    dll_command += fr[0].ToString();
                                    if (is_reserv(fr[1].ToString().Trim()))
                                    {
                                        dll_command += "\"" + fr[1].ToString().Trim() + "\"";
                                    }
                                    else
                                    {
                                        dll_command += fr[1].ToString().Trim();
                                    }
                                    //соостав индекса
                                    string fields_ = "";
                                    using (FbCommand fcon_f = new FbCommand(sql_index_f, fb, ft))
                                    {
                                        fcon_f.Parameters.Add("@a", FbDbType.VarChar, 31);
                                        fcon_f.Parameters[0].Value = fr[2].ToString();
                                        using (FbDataReader fr_f = fcon_f.ExecuteReader())
                                        {
                                            while (fr_f.Read())
                                            {
                                                if (fields_ == "")
                                                {
                                                    fields_ += "(" + fr_f[0].ToString();
                                                }
                                                else
                                                {
                                                    fields_ += "," + fr_f[0].ToString();
                                                }
                                            }
                                            fields_ += ")";
                                            fr_f.Dispose();
                                        }
                                        fcon_f.Dispose();
                                    }
                                    dll_command += " " + fields_;

                                    com_indexes.Add(dll_command);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #endregion
        #region EXECUTE COM
        public void execute_com()
        {
            using (FbConnection fb = new FbConnection(fc_new.ConnectionString))
            {                
                string last_com = "";
                try
                {
                    fb.Open();

                    int i = 0;
                    int j = com.Count;
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        foreach (string cmd in com)
                        {
                            last_com = cmd;
                            i++;
                            p_cur = (int)(((float) i / (float) j) * 100);
                            p_text = i.ToString() + "/" + j.ToString();
                        
                            using (FbCommand fcon = new FbCommand(cmd, fb,ft))
                            {
                                
                                try
                                {
                                    fcon.ExecuteNonQuery();
                                }
                                catch(FbException ex)
                                {
                                    sb.AppendLine("**********");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine("**********");
                                    sb.AppendLine(cmd);
                                }
                                fcon.Dispose();
                            }
                        }
                      ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("EXECUTE COM");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine("**********Command");
                    sb.AppendLine(last_com);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        #endregion
        #region EXECUTE DATA
        public void go_data()
        {
            //запускаем заполнение списка
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    int i_ = 0;
                    int j_ = tables_.Count;
                    foreach (string t_ in tables_)
                    {
                        i_++;
                        p_text = i_.ToString() + "/" + j_.ToString() + " " + t_; //прогресс таблиц
                        p_cur = (int)(((float)i_ / (float)j_) * 100);

                        using (FbTransaction ft = fb.BeginTransaction())
                        {
                            using (FbCommand fcon = new FbCommand("Select * from " + t_,fb,ft))
                            {
                                switch (t_.Trim().ToUpper())
                                {
                                    case "DELETED":
                                        fcon.CommandText += " where Date_Deleted >= cast('NOW' as date) - 180";
                                        break;
                                    case "ERR_LOG":
                                        fcon.CommandText += " where id_err_log is null";
                                        break;
                                    case "BACKUP_LOG":
                                        fcon.CommandText += " where id_BACKUP_LOG is null";
                                        break;
                                    case "SHADOWGUARD":
                                        fcon.CommandText += " where id_SHADOW is null";
                                        break;
                                    case "USER_ACTIVITY":
                                        fcon.CommandText += " where id_user_activity is null";
                                        break;
                                    case "MESSAGES":
                                        fcon.CommandText += " where lastdate >= cast('NOW' as date) - 100";
                                        break;
                                    case "SCHEDULER":
                                        fcon.CommandText += " where id_status is null";
                                        break;
                                    case "USER_CONNECTIONS":
                                        fcon.CommandText += " where ID_USER_CONNECTIONS is null";
                                        break;
                                    case "REPORT_REPLICATION":
                                        fcon.CommandText += " where DATE_CREATE >= cast('NOW' as date) - 30";
                                        break;
                                }
                                using (FbDataReader fr = fcon.ExecuteReader())
                                {
                                    while (fr.Read())
                                    {
                                        var sqlinsert = "insert into " + t_ + " values(";
                                        for (int t = 0; t <= fr.FieldCount - 1; t++)
                                        {
                                            if (sqlinsert != "insert into " + t_ + " values(")
                                            {
                                                sqlinsert += ",";
                                            }
                                            if (fr[t] == DBNull.Value)
                                            {
                                                sqlinsert += "null";
                                            }
                                            else
                                            {
                                                if (fr[t].GetType().ToString().ToUpper() == "SYSTEM.DOUBLE")
                                                {

                                                    sqlinsert += fr[t].ToString().Replace(",", ".");
                                                }
                                                else
                                                    //заменяем все ' на ''
                                                    sqlinsert += "'" + fr[t].ToString().Replace("'", "''").Replace(" 0:00:00", " ") + "'";
                                            }
                                        }
                                        sqlinsert += ");";                                       
                                        q_data.Enqueue(sqlinsert);//добавляем INSERT                                        
                                        if (q_data.Count >= 100000)
                                        {
                                            //запускаем создание записей на базе
                                            using (Task t_load = new Task(go_load))
                                            {
                                                t_load.Start();
                                                t_load.Wait();
                                                t_load.Dispose();
                                                q_data.Clear();
                                            }
                                        }
                                    }
                                    fr.Dispose();
                                }
                                fcon.Dispose();
                            }
                            ft.Commit();
                            ft.Dispose();
                        }
                    }
                    //и в конце запустим еще раз
                    using (Task t_load = new Task(go_load))
                    {
                        t_load.Start();
                        t_load.Wait();
                        t_load.Dispose();
                        q_data.Clear();
                    }
                    p_text = "Анализ данных завершен"; //конец анализа
                    p_cur = 100;
                    is_close_analize = true;//флаг, что анализ данных завершен

                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        public void go_load()
        {
            using (FbConnection fb = new FbConnection(fc_new.ConnectionString))
            {
                try
                {
                    fb.Open();
                    int _i = 0;
                    int _j = q_data.Count;
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand("", fb, ft))
                        {
                            while (q_data.Count != 0)
                            {
                                _i++;
                                p_cur_2 = (int)(((float)_i / (float)_j) * 100);
                                p_text_2 = p_cur_2.ToString() + @"%";
                                fcon.CommandText = q_data.Dequeue();
                                fcon.ExecuteNonQuery();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        public int data_count()
        {
            int count_ = 0;
            string t_name = table_name;
            if (is_reserv(t_name))
            {
                t_name = "\"" + t_name + "\"";
            }
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand("select count(*) from " + t_name, fb, ft))
                        {
                            switch (table_name.Trim().ToUpper())
                            {
                                case "DELETED":
                                    fcon.CommandText += " where Date_Deleted >= cast('NOW' as date) - 180";
                                    break;
                                case "ERR_LOG":
                                    fcon.CommandText += " where id_err_log is null";
                                    break;
                                case "BACKUP_LOG":
                                    fcon.CommandText += " where id_BACKUP_LOG is null";
                                    break;
                                case "SHADOWGUARD":
                                    fcon.CommandText += " where id_SHADOW is null";
                                    break;
                                case "USER_ACTIVITY":
                                    fcon.CommandText += " where id_user_activity is null";
                                    break;
                                case "MESSAGES":
                                    fcon.CommandText += " where lastdate >= cast('NOW' as date) - 100";
                                    break;
                                case "SCHEDULER":
                                    fcon.CommandText += " where id_status is null";
                                    break;
                                case "USER_CONNECTIONS":
                                    fcon.CommandText += " where ID_USER_CONNECTIONS is null";
                                    break;
                                case "REPORT_REPLICATION":
                                    fcon.CommandText += " where DATE_CREATE >= cast('NOW' as date) - 30";
                                    break;
                            }
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    count_ = (int)fr[0];
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();

                    }
                }
                finally { fb.Close(); }
                fb.Dispose();
            }
            return count_;
        }
        public void PREPARE_DATA()
        {
            if (is_reserv(table_name))
            {
                table_name = "\"" + table_name + "\"";
            }
            using (FbConnection fb = new FbConnection(fc_old.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand(sql_data.Replace("@a", table_name), fb, ft))
                        {
                            fcon.Parameters.Add("@skip", FbDbType.Integer);
                            fcon.Parameters["@skip"].Value = skip_;
                            //дополнительные условия для отбора
                            switch (table_name.Trim().ToUpper())
                            {
                                case "DELETED":
                                    fcon.CommandText += " where Date_Deleted >= cast('NOW' as date) - 180";
                                    break;
                                case "ERR_LOG":
                                    fcon.CommandText += " where id_err_log is null";
                                    break;
                                case "BACKUP_LOG":
                                    fcon.CommandText += " where id_BACKUP_LOG is null";
                                    break;
                                case "SHADOWGUARD":
                                    fcon.CommandText += " where id_SHADOW is null";
                                    break;
                                case "USER_ACTIVITY":
                                    fcon.CommandText += " where id_user_activity is null";
                                    break;
                                case "MESSAGES":
                                    fcon.CommandText += " where lastdate >= cast('NOW' as date) - 100";
                                    break;
                                case "SCHEDULER":
                                    fcon.CommandText += " where id_status is null";
                                    break;
                                case "USER_CONNECTIONS":
                                    fcon.CommandText += " where ID_USER_CONNECTIONS is null";
                                    break;
                                case "REPORT_REPLICATION" :
                                    fcon.CommandText += " where DATE_CREATE >= cast('NOW' as date) - 30";
                                    break;
                            }
                            using (FbDataReader fr = fcon.ExecuteReader())
                            {
                                while (fr.Read())
                                {
                                    var sqlinsert = "insert into " + table_name + " values(";
                                    for (int t = 0; t <= fr.FieldCount - 1; t++)
                                    {
                                        if (sqlinsert != "insert into " + table_name + " values(")
                                        {
                                            sqlinsert += ",";
                                        }
                                        if (fr[t] == DBNull.Value)
                                        {
                                            sqlinsert += "null";
                                        }
                                        else
                                        {
                                            if (fr[t].GetType().ToString().ToUpper() == "SYSTEM.DOUBLE")
                                            {

                                                sqlinsert += fr[t].ToString().Replace(",", ".");
                                            }
                                            else
                                                //заменяем все ' на ''
                                                sqlinsert += "'" + fr[t].ToString().Replace("'", "''").Replace(" 0:00:00", " ") + "'";
                                        }
                                    }
                                    sqlinsert += ");";
                                    com2.Add(sqlinsert);
                                }
                                fr.Dispose();
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("");
                    sb.AppendLine(ex.Message);
                }
                finally
                {
                    fb.Close();
                }
                fb.Dispose();
            }
        }
        public void COPY_DATA()
        {
            
            int i = 0;
            int j = com2.Count;
            //а теперь накатываем
            using (FbConnection fb = new FbConnection(fc_new.ConnectionString))
            {
                try
                {
                    fb.Open();
                    using (FbTransaction ft = fb.BeginTransaction())
                    {
                        using (FbCommand fcon = new FbCommand("", fb, ft))
                        {
                            foreach (string cmd in com2)
                            {
                                i++;
                                p_cur = (int)(((float)i / (float)j) * 100);
                                p_text = table_name + " " + i.ToString() + "/" + j.ToString();
                                
                                fcon.CommandText = cmd;
                                try
                                {
                                    fcon.ExecuteNonQuery();
                                }
                                catch (FbException ex2)
                                {
                                    sb.AppendLine("**********");
                                    sb.AppendLine("ERROR DATA");
                                    sb.AppendLine("**********");
                                    sb.AppendLine(cmd);
                                    sb.AppendLine("");
                                    sb.AppendLine(ex2.Message);
                                    sb.AppendLine("**********");

                                }
                            }
                            fcon.Dispose();
                        }
                        ft.Commit();
                        ft.Dispose();
                    }
                }
                catch (FbException ex)
                {
                    sb.AppendLine("DATA - " + table_name);
                    sb.AppendLine(ex.Message);
                }
                fb.Dispose();
            }
        }
        #endregion
        #region USERS
        public void fb_add(string username, string pass, bool is_full, string database)
        {
            //теперь нужно создать пользователя, под которым будет производится установка
            FbConnectionStringBuilder fc = new FbConnectionStringBuilder();
            fc.DataSource = "localhost";
            fc.UserID = "SYSDBA";
            if (this.is_new_sysdba.Checked)
            {
                fc.Password = this.t_sysdba_new.Text;
            }
            else
            {
                fc.Password = this.t_sysdba.Text;
            }
            if (database != "")
            {
                fc.Database = database;
            }
            FbSecurity fb = new FbSecurity();

            fb.ConnectionString = fc.ConnectionString;

            FbUserData fu = new FbUserData();
            fu.UserName = username;
            if (is_full)
            {
                fu.RoleName = "FULL_ACCESS";
            }
            fu.UserPassword = pass;
            try
            {
                fb.AddUser(fu);
            }
            catch { }
            finally { }
        }
        public void fb_up(string pass)
        {
            //теперь нужно создать пользователя, под которым будет производится установка
            FbConnectionStringBuilder fc = new FbConnectionStringBuilder();
            fc.DataSource = "localhost";
            fc.UserID = "SYSDBA";
            fc.Password = this.t_sysdba.Text;

            FbSecurity fb = new FbSecurity();

            fb.ConnectionString = fc.ConnectionString;

            FbUserData fu = new FbUserData();
            fu.UserName = "SYSDBA";
            fu.UserPassword = pass;
            try
            {
                fb.ModifyUser(fu);
            }
            catch { }
            finally { }
        }
        #endregion 

        private void b_save_Click(object sender, EventArgs e)
        {
            saveAs.ShowDialog();
        }

        private void F99_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!is_may_close)
            {
                MessageBox.Show("Дождитесь окончания процесса!");
                e.Cancel = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
             this.progressBar1.Value = p_cur;
             this.l_.Text = p_text;

             this.l_count.Text = p_text_2;
             this.progressBar2.Value = p_cur_2;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            this.t_sysdba_new.Enabled = this.is_new_sysdba.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new F_a();
            f.ShowDialog();
        }
    }
}
