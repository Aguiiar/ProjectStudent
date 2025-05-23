using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using projectStudent.Data;

namespace ProjectStudant.Services
{
/// <summary>
	/// Descrição resumida de AutoCompleteService
	///</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]
// Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir.
[System.Web.Script.Services.ScriptService]
public class AutoCompleteService : System.Web.Services.WebService
{

[WebMethod]
public List<string> GetStudentsName(string prefixText, int count)
        {
            using (var context = new StudentContext())
            {
                return context.Students
                    .Where(s => s.Name.ToLower().StartsWith(prefixText.ToLower()))
                    .Select(s => s.Name)
                    .Take(count)
                    .ToList();
            }
        }
    }
}