using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using Z3.Workspace;

namespace Z3.Util
{
    class FileExporter
    {
        public FileExporter(ReportType rt)
        {
            _rt = rt;
        }

        private ReportType _rt;

        public void export(string filename, bool colHeaders)
        {
            using (DbCommand command = _rt.CreateCommand())
            {
                export(command, filename, colHeaders);
            } 
        }
        

        public static void export(DbCommand cmd, string filename, bool colHeaders)
        {
            if (File.Exists(filename)) File.Delete(filename);

            using (StreamWriter outf = new StreamWriter(filename))
            {
                using (DbDataReader r = cmd.ExecuteReader())
                {
                    if (colHeaders)
                        writeColHeaders(r, outf);
                    export(r, outf);
                }
            }
        }

        public static void export(DbDataReader r, StreamWriter outf)
        {
            while (r.Read())
                writeLine(r, outf);
        }

        private static void writeLine(DbDataReader r, StreamWriter outf)
        {
            for (int i = 0; i < r.FieldCount; i++)
            {
                if (i > 0) outf.Write(',');
                outf.Write(r[i]);
            }
            outf.WriteLine();
        }

        private static void writeColHeaders(DbDataReader r, StreamWriter f)
        {
            for (int i = 0; i < r.FieldCount; i++)
            {
                if (i > 0) f.Write(',');
                f.Write(GetCSVSafeString(r.GetName(i)));
            }
            f.WriteLine();
        }

        public static string GetCSVSafeString(string input)
        {
            for (int i = 0; i < input.Length; i++)
                if (input[i] == ',' || input[i] == '\"' || input[i] == '\n' || input[i] == '\r')
                    return safeHelper(input);

            return input;
        }

        private static string safeHelper(string input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('\"');
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\"')
                    sb.Append('\"');
                sb.Append(input[i]);
            }
            sb.Append('\"');

            return sb.ToString();
        }
    }
}
