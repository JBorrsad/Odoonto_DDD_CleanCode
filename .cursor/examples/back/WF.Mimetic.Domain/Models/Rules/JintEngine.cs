namespace WF.Mimetic.Domain.Models.Rules;

using Acornima;
using Acornima.Ast;
using Jint;
using Jint.Native;
using Jint.Native.Json;
using Jint.Runtime;
using Newtonsoft.Json;
using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;

public class JintEngine : IRulesEngine
{
    private readonly Engine _engine;
    private readonly JsonParser _parser;

    public JintEngine()
    {
        _engine = new Engine();
        _parser = new JsonParser(_engine);
    }

    public bool IsValidScript(string script)
    {
        return TryParse(script, out Script _);
    }

    public string Execute(string script, string msg)
    {
        JsValue msgParsed = _parser.Parse(msg);
        JsValue result = EvaluateOrThrow(script, msgParsed);
        string resultMessage = ParseResult(result);

        return resultMessage;
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
        string secureScript = PrintSecureFunction(script);
        return new Parser().ParseScript(secureScript);
    }

    private JsValue EvaluateOrThrow(string script, JsValue msg)
    {
        try
        {
            var secureFunction = _engine.Evaluate(PrintSecureFunction(script));

            return _engine.Invoke(secureFunction, msg);
        }
        catch (JavaScriptException ex)
        {
            throw new InvalidValueException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new InvalidValueException("Evaluate script error.", ex);
        }
    }

    private string PrintSecureFunction(string script)
    {
        return "(function(__inputMsg) { let msg = __inputMsg.msg; let body = __inputMsg.body; let query = __inputMsg.query; let route = __inputMsg.route; " + (script ?? "") + " })";
    }

    private string ParseResult(JsValue result)
    {
        try
        {
            if (result.IsUndefined())
            {
                return result.ToString();
            }

            if (result.IsNull())
            {
                return result.ToString();
            }

            if (result.IsPrimitive())
            {
                return result.ToString();
            }

            return JsonConvert.SerializeObject(result.ToObject());
        }
        catch (Exception ex)
        {
            throw new WrongOperationException("Parse result error.", ex);
        }
    }

    public void SetValue(string name, object value)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new WrongOperationException("The global variable name cant be null/empty");
        }

        if(value is null)
        {
            throw new WrongOperationException("The global variable value cant be null");
        }

        _engine.SetValue(name, value);
    }
}
