using AwesomeProxy.FilterAttribute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AwesomeProxy.Test.IntegrationObject
{
    internal class WithParameterClass : IWithParameterClass
    {
        private string _message1;
        private string _message2;

        internal WithParameterClass(string message1,string message2)
        {
            this._message1 = message1;
            this._message2 = message2;
        }

        string IWithParameterClass.RecordInfo(string messageContent)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(_message1);
            sb.AppendLine(_message2);
            sb.AppendLine(messageContent);
            return sb.ToString();
        }
    }

    internal interface IWithParameterClass {

        [MaskPassword]
        internal string RecordInfo(string messageContent);
    }

    public class MaskPasswordAttribute : AopBaseAttribute
    {
        public override void OnExecuting(ExecutingContext context)
        {
            string pattern = @"Enter the password: (?<password>.+)";

            for (int i = 0; i < context.Args.Length; i++)
            {
                context.Args[i] = Regex.Replace(context.Args[i].ToString(), pattern, match =>
                {
                    string password = match.Groups["password"].Value;
                    return $"Enter the password: {new string('*', password.Length)}";
                });
                
            }
        }
    }
}
