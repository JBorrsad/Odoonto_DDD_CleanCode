namespace WF.Mimetic.Domain.Interfaces.Rules;

public interface IRulesEngine
{
    void SetValue(string name, object value);
    string Execute(string script, string msg);
    bool IsValidScript(string script);
}
