using Acornima;
using Acornima.Ast;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using Newtonsoft.Json;
using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;

namespace WF.Mimetic.Domain.Models.Rules
{
    public class V8ClearScriptEngine : IRulesEngine
    {
        private readonly V8ScriptEngine _engine;

        public V8ClearScriptEngine()
        {
            _engine = new V8ScriptEngine();
        }

        public string Execute(string script, string msg)
        {
            return EvaluateOrThrow(script, msg);
        }

        private string EvaluateOrThrow(string script, string msg)
        {
            try
            {
                string secureFunction = PrintSecureFunction(script, msg);
                object result = _engine.Evaluate(secureFunction);

                if(result is ScriptObject scriptObject)
                {
                    return JsonConvert.SerializeObject(scriptObject);
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidValueException("Evaluate script error.", ex);
            }
        }

        public bool IsValidScript(string script)
        {
            return TryParse(script, out Script _);
        }

        public void SetValue(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new WrongOperationException("The global variable name cant be null/empty");
            }

            if (value is null)
            {
                throw new WrongOperationException("The global variable value cant be null");
            }

            _engine.AddHostObject(name, value);
        }

        private bool TryParse(string script, out Script result)
        {
            try
            {
                result = Parse(script);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        private Script Parse(string script)
        {
            string secureScript = PrintSecureFunction(script, "{\"msg\":{}, \"route\":{}, \"query\":{}}");

            return new Parser().ParseScript(secureScript);
        }

        private string PrintSecureFunction(string script, string msg)
        {
            return "(function() { let __inputMsg = " + msg + "; let msg = __inputMsg.msg; let body = __inputMsg.body; let query = __inputMsg.query; let route = __inputMsg.route; " + (script ?? "") + " })()";
        }
    }
}
