using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace JsonFormat {
    public class JsonFormat {
        public static string ListToJSON<T>(List<T> objlist, string classname) {
            string result = "{";
            if (classname.Equals(string.Empty)) {
                object o = objlist[0];
                classname = o.GetType().ToString();
            }
            result += "\"" + classname + "\":[";
            bool firstline = true;
            foreach (object oo in objlist) {
                if (!firstline) {
                    result = result + "," + OneObjectToJSON(oo);
                }
                else {
                    result = result + OneObjectToJSON(oo) + "";
                    firstline = false;
                }
            }
            return result + "]}";
        }

        private static string OneObjectToJSON(object o) {
            string result = "{";
            List<string> ls_propertys = new List<string>();
            ls_propertys = GetObjectProperty(o);
            foreach (string str_property in ls_propertys) {
                if (result.Equals("{")) {
                    result = result + str_property;
                }
                else {
                    result = result + "," + str_property + "";
                }
            }
            return result + "}";
        }

        private static List<string> GetObjectProperty(object o) {
            List<string> propertyslist = new List<string>();
            PropertyInfo[] propertys = o.GetType().GetProperties();
            foreach (PropertyInfo p in propertys) {
                propertyslist.Add("\"" + p.Name.ToString() + "\":\"" + p.GetValue(o, null) + "\"");
            }
            return propertyslist;
        }

        private static string ConvertJsonString(string str) {
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null) {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter) {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else {
                return str;
            }
        }
    }
}
